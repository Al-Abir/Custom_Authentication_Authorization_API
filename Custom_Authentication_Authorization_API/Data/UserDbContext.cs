using Custom_Authentication_Authorization_API.Entites;
using Microsoft.EntityFrameworkCore;

namespace Custom_Authentication_Authorization_API.Data
{
    public class UserDbContext :DbContext

    {

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
       
        public DbSet<User> Users { get; set; }
    }
}
