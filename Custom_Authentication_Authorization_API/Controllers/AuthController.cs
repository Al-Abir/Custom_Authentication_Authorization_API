using Custom_Authentication_Authorization_API.Entites;
using Custom_Authentication_Authorization_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Custom_Authentication_Authorization_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration Configuration) : ControllerBase
    {

        public static User user = new User();
        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            var hashPassword = new PasswordHasher<User>()
                .HashPassword(user,request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashPassword;

            return Ok(user);

        }
        [HttpPost("login")]
        public ActionResult<string> Login(UserDto request)
        {
            if (user.Username != request.Username) {
                return BadRequest("User Not Found");
            }

            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return BadRequest("Wrong Password");
            }
            string token = CreateToken(user);
            return Ok(token);
        }


        
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<String>("AppSettings:Token")!));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: Configuration.GetValue<string>("AppSettings.Issuer"),
                audience: Configuration.GetValue<string>("AppSettings.Audience"),
                claims: claims,
                expires:DateTime.UtcNow.AddDays(1),
                signingCredentials: creds

                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
