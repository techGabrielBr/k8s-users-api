using MassTransit;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Extensions;
using UsersAPI.Infrastructure.Data;
using UsersAPI.Infrastructure.Repositories;
using UsersAPI.Infrastructure.Security;
using UsersAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ======================
// Database (Sqlite)
// ======================
var defaultConnection = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(defaultConnection)
);

// ======================
// JWT
// ======================
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// ======================
// Controllers + Swagger
// ======================
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Users API",
        Version = "v1"
    });
});

builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ======================
// MassTransit Config
// ======================
builder.Services.AddMassTransit(x =>
{
    var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
    var username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
    var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

    if (host == null || username == null || password == null)
    {
        throw new Exception("RabbitMQ configuration is missing. Please set environment variables");
    }
    else
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(host, "/", h =>
            {
                h.Username(username);
                h.Password(password);
            });
        });
    }
});

// ======================
var app = builder.Build();
// ======================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();