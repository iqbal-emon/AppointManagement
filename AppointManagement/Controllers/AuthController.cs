using AppointManagement.Context;
using AppointManagement.Models;
using AppointManagement.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppointManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public AuthController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO loginRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
                {
                    return BadRequest(new { Message = "Username and password are required." });
                }

                var existingUser = _context.Users.FirstOrDefault(u => u.Username == loginRequest.Username);

                if (existingUser == null)
                {
                    return Unauthorized(new { Message = "Invalid username or password." });
                }

                var passwordHasher = new PasswordHasher<User>();
                var verificationResult = passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, loginRequest.Password);

                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return Unauthorized(new { Message = "Invalid username or password." });
                }

                var accessToken = GenerateAccessToken(existingUser.Username, existingUser.UserId);

                return Ok(new { Token = accessToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request. Please try again later." });
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDTO userDto)
        {
            try
            {
                if (userDto == null || string.IsNullOrWhiteSpace(userDto.Username) || string.IsNullOrWhiteSpace(userDto.Password))
                {
                    return BadRequest(new { Message = "Invalid registration data. Username and password are required." });
                }

                if (_context.Users.Any(u => u.Username == userDto.Username))
                {
                    return Conflict(new { Message = "Username already exists. Please choose a different username." });
                }

                var passwordHasher = new PasswordHasher<UserDTO>();
                var hashedPassword = passwordHasher.HashPassword(userDto, userDto.Password);

                var user = new User
                {
                    Username = userDto.Username,
                    PasswordHash = hashedPassword
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok(new { Message = "Registration successful." });
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., to a file or monitoring system)
                return StatusCode(500, new { Message = "An error occurred while registering the user. Please try again later." });
            }
        }

        private string GenerateAccessToken(string userName, int? userId)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim("UserId", userId?.ToString() ?? string.Empty)
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15), // Access token validity
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error generating access token", ex);
            }
        }
    }
}
