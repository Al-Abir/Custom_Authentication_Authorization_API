using Custom_Authentication_Authorization_API.Entites;
using Custom_Authentication_Authorization_API.Models;
using Custom_Authentication_Authorization_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Authentication_Authorization_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        public static User user = new User();
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(RegisterDto request)
        {
            var user = await authService.RegisterAsync(request);

            if (user == null)
                return BadRequest("User Name Already Exists");

            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
           var token = await authService.LoginAsync(request);

            if (token == null) {
                return BadRequest("Invaild User name or Password wrong");
            }
            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        
        public IActionResult AuthticatedOnlyEndPoint()
        {
             return Ok("You are authenticted");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("Only Admin can access this endpoint");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("admin-or-user")]
        public IActionResult AdminOrUser()
        {
            return Ok("Admin and User both can access");
        }
    }
}
