using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NFKApplication.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace NFKApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto info)
        {
            if (!_authService.TryValidate(info.Username, info.Password, out var message))
            {
                _logger.LogWarning($"Login attempt failed. {JsonSerializer.Serialize(info)} Message: {message}");
                return Unauthorized();
            }

            var claims = new[]
            {
                    new Claim(ClaimTypes.Name, info.Username)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BALAJFMCAOSPDJA198VNAOCP91AVZOLB1PPANDl1NAV99KL"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7282/",
                audience: "https://localhost:7282/",
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            Response.Cookies.Append(
                "auth",
                jwtToken,
                new CookieOptions
                {
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(24)
                }
            );

            return RedirectToPage("/Index");
        }
    }
    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
