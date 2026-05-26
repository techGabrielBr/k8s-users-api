using Amazon.SQS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using UsersAPI.Domain.Entities;
using UsersAPI.Events.Publishers;
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
// SQS Config
// ======================

var AWS_SERVICE_URL = Environment.GetEnvironmentVariable("AWS_SERVICE_URL");
var AWS_USER = Environment.GetEnvironmentVariable("AWS_USER");
var AWS_PASSWORD = Environment.GetEnvironmentVariable("AWS_PASSWORD");

builder.Services.AddSingleton<IAmazonSQS>(_ =>
{
    return new AmazonSQSClient(
        AWS_USER,
        AWS_PASSWORD,
        new AmazonSQSConfig
        {
            ServiceURL = AWS_SERVICE_URL
        }
    );
});

builder.Services.AddSingleton<SqsEventPublisher>();

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

app.UseHttpMetrics();

app.MapControllers();
app.MapMetrics();
app.Run();