namespace Custom_Authentication_Authorization_API.Models
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}
