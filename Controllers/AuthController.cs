using BCrypt.Net;
using MessengerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace MessengerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            if (_context.Users.Any(u => u.UserName == model.Username))
                return Conflict(new { Error = "Пользователь с таким именем уже существует." });

            
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            
            var newUser = new User
            {
                UserName = model.Username,
                HashedPassword = hashedPassword
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { Success = true, Message = "Пользователь успешно зарегистрирован." });
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);
            if (user == null)
                return Unauthorized(new { Error = "Неправильное имя пользователя или пароль." });

            
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.HashedPassword);
            if (!isValidPassword)
                return Unauthorized(new { Error = "Неправильное имя пользователя или пароль." });

            
            var jwtToken = GenerateJwtToken(user);
            return Ok(jwtToken);
        }

        
        private object GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a-much-more-complicated-and-long-secret-key-at-least-32-bytes-long")); 
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.UserName), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) };


                var token = new JwtSecurityToken(
                issuer: "myauthserver",
                audience: "messenger-api",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), 
                signingCredentials: credentials
            );
                
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
            
        }
    }

    
    

    
}