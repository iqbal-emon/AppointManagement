using AppointManagement.Context;
using AppointManagement.Models;
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
        public IActionResult Login([FromBody] User loginRequest)
        {
            // Fetch the user based on the username
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == loginRequest.Username);

            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password."); 
            }

            // Verify the provided password against the stored hash
            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, loginRequest.PasswordHash);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid username or password."); // Password mismatch
            }

            // Generate JWT token
            var accessToken = GenerateAccessToken(existingUser.Username, existingUser.UserId);

            // Return the token
            return Ok(new { AccessToken = accessToken });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {

            var passwordHasher = new PasswordHasher<User>();

            var hashedPassword = passwordHasher.HashPassword(user, user.PasswordHash);
            user.PasswordHash = hashedPassword;

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(new {Message="Registraion Successful"});
        }

        private string GenerateAccessToken(string userName, int? userId)
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

    }



}
