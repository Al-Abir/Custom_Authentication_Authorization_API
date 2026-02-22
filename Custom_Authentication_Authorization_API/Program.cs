using Custom_Authentication_Authorization_API.Data;
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

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ?? Important: Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();