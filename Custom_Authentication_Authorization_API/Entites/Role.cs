using Custom_Authentication_Authorization_API.Entites;

namespace Custom_Authentication_Authorization_API.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
