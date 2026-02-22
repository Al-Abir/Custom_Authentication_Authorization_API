using Custom_Authentication_Authorization_API.Data;
using Custom_Authentication_Authorization_API.Entites;
using Custom_Authentication_Authorization_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Custom_Authentication_Authorization_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserDbContext _Context;
        private readonly IConfiguration _Configuration;
        public AuthService(UserDbContext context, IConfiguration configuration)
        {
            _Context = context;
            _Configuration = configuration;
        }

       

        public async Task<string?> LoginAsync(UserDto request)
        {

            var user = await _Context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user is null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return CreateToken(user);
            
        }

        public async Task<User?> ReagisterAsync(UserDto request)
        {
            if (await _Context.Users.AnyAsync(u => u.Username == request.Username)) { 
               return null;
            }
            
            var user = new User();
            var hashPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashPassword;
            _Context.Users.Add(user);
            await _Context.SaveChangesAsync();
            return user;
        }

       private string CreateToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Aud, _Configuration["AppSettings:Audience"]!) // audience add
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_Configuration["AppSettings:Token"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _Configuration["AppSettings:Issuer"],
                audience: _Configuration["AppSettings:Audience"], // match with ValidateAudience
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
