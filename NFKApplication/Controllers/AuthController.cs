using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NFKApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Constructor and other methods can be added here

        [HttpPost("login")]
        public IActionResult Login([FromForm] UserLoginDto userLogin)
        {
            // Replace with your own validation
            if (userLogin.Username == "admin" && userLogin.Password == "password")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, userLogin.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BALAJFMCAOSPDJA198VNAOCP91AVZOLB1PPANDl1NAV99KL"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:7282/",
                    audience: "https://localhost:7282/",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
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
                        Expires = DateTimeOffset.MaxValue
                    }
                );

                return Ok(new { success = true });
            }

            return Unauthorized();
        }
    }
    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
