using Events.Models;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.API.Controllers.Models;
using UsersAPI.Domain.Entities;
using UsersAPI.Infrastructure.Repositories;
using UsersAPI.Infrastructure.Security;
using UsersAPI.Infrastructure.Validators;

namespace UsersAPI.API.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController(
        IUserRepository userRepository,
        JwtTokenGenerator jwtService,
        IPublishEndpoint publishEndpoint) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly JwtTokenGenerator _jwtService = jwtService;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var errors = UserValidator.Validate(
                request.Name,
                request.Email,
                request.Password
            );

            if (errors.Count != 0)
                return BadRequest(new { errors });

            if (await _userRepository.GetByEmailAsync(request.Email) != null)
                return Conflict(new { error = "Usuário já existe" });

            var user = new User(
                request.Name,
                request.Email,
                string.Empty
            );

            var hasher = new PasswordHasher<User>();
            user.SetPassword(
                hasher.HashPassword(user, request.Password)
            );

            await _userRepository.AddAsync(user);

            await _publishEndpoint.Publish(new UserCreatedEvent
            {
                Id = user.Id,
                Email = user.Email,
                CreatedAt = DateTime.UtcNow
            });

            return Created("", new
            {
                user.Id,
                user.Name,
                user.Email
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
                return Unauthorized(new { error = "Credenciais inválidas" });

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { error = "Credenciais inválidas" });

            var token = _jwtService.Generate(user);

            return Ok(new { token });
        }
    }
}
