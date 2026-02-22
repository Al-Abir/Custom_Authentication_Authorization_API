using Custom_Authentication_Authorization_API.Entites;
using Custom_Authentication_Authorization_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Custom_Authentication_Authorization_API.Data
{
    public class UserDbContext :DbContext

    {

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
       
        public DbSet<User> Users { get; set; }
        // Roles table(missing)
        public DbSet<Role> Roles { get; set; } = null!;
    }
}
