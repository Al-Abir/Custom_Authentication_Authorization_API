using Custom_Authentication_Authorization_API.Entites;
using Custom_Authentication_Authorization_API.Models;

namespace Custom_Authentication_Authorization_API.Services
{
    public interface IAuthService
    {

        Task<User?> ReagisterAsync(UserDto request);
        Task<string?> LoginAsync(UserDto request);
    }
}
