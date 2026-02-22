using Custom_Authentication_Authorization_API.Data;
using Custom_Authentication_Authorization_API.Entities;
using Custom_Authentication_Authorization_API.Extensions;
using Custom_Authentication_Authorization_API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Swagger & JWT setup via Extensions
builder.Services.AddSwaggerWithJwt();
builder.Services.AddJwtAuthentication(builder.Configuration);

// Database
builder.Services.AddDbContext<UserDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("UserDataBase")));

// Dependency Injection
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Seed Roles (Important: Do this before any User operation)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();

    // Roles ???
    if (!context.Roles.Any())
    {
        var adminRole = new Role { Id = Guid.NewGuid(), Name = "Admin" };
        var userRole = new Role { Id = Guid.NewGuid(), Name = "User" };

        context.Roles.AddRange(adminRole, userRole);
        context.SaveChanges();
    }

    // ???? Users ?? RoleId ??? ??? (??? Users already ????)
    var defaultRoleId = context.Roles.First().Id; // Admin
    foreach (var user in context.Users.Where(u => !context.Roles.Any(r => r.Id == u.RoleId)))
    {
        user.RoleId = defaultRoleId;
    }
    context.SaveChanges();
}

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();