using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Config;
using OrderService.Data;
using OrderService.Models;
using OrderService.Models.Requests;

namespace OrderService.Controllers
{
    /// <summary>
    /// Controller for authentication-related actions, including user registration and login.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ErpDbContext _context;
        private readonly Utilities _utilities;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpiresIn;

        public AuthController(ErpDbContext context, Utilities utilities)
        {
            _context = context;
            _utilities = utilities;
            _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            _jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            _jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            _jwtExpiresIn = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES_IN") ?? "60");
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="request">The data containing the new user's information.</param>
        /// <returns>An IActionResult indicating the result of the registration process.</returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Username == request.Username || u.Email == request.Email))
                return BadRequest("Username or email already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered successfully.");
        }

        /// <summary>
        /// Logs a user into the system.
        /// </summary>
        /// <param name="request">The data containing the login credentials.</param>
        /// <returns>An IActionResult indicating the result of the login process.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = _utilities.GenerateJwtToken(user, _jwtSecret, _jwtIssuer, _jwtAudience, _jwtExpiresIn);
            return Ok(new
            {
                message = "Please, save this token",
                jwt = token
            });
        }
    }
}