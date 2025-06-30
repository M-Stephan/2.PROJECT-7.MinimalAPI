// using namespaces
using Solution;
using Solution.Controllers;
using Solution.Users;
using Solution.Tickets;
using Solution.Services;
using Solution.Data;
// Scalar
using Scalar.AspNetCore;
// EF Core
using Microsoft.EntityFrameworkCore;
// JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// -- Build ASP.NET environment --
var builder = WebApplication.CreateBuilder(args);

// Configure Authentication and JWT Bearer.

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),

        ValidateAudience = true,
        ValidAudience = jwtSettings.GetValue<string>("Audience"),

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add OpenAPI
builder.Services.AddOpenApi();

// Define context
string? context = builder.Configuration.GetConnectionString("DefaultConnection");

// -- Add DbContext with the connection string --
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(context));

// Important : Register controllers support
builder.Services.AddControllers();

// Initialize Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();

// Builder
var app = builder.Build();

// JWT Auth
app.UseAuthentication();
app.UseAuthorization();

// OpenAPI Map
app.MapOpenApi();
app.MapScalarApiReference();

// -- Initialize DbContext and apply migrations --
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var retryCount = 0;
    const int maxRetries = 10;

    while (retryCount < maxRetries)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            Console.WriteLine($"Waiting for SQL Server... ({retryCount}/{maxRetries})");

            if (retryCount == maxRetries)
            {
                Console.WriteLine($"SCRIPT ERROR: {ex}");
                throw;
            }

            Thread.Sleep(3000);
        }
    }

    // Seed users if Users table is empty
    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { FirstName = "John", LastName = "Doe", Email = "Johndoe@domain.be", PasswordHash = "jd123" },
            new User { FirstName = "Hector", LastName = "Lastor", Email = "Hector@domain.be", PasswordHash = "hector123" },
            new User { FirstName = "Alice", LastName = "Gretchen", Email = "alice@domain.be", PasswordHash = "alice123" },
            new User { FirstName = "Sophie", LastName = "Clapton", Email = "sophie@domain.be", PasswordHash = "sophie123" }
        );
        db.SaveChanges(); // save to generate IDs
    }

    // Seed tickets if Tickets table is empty
    if (!db.Tickets.Any())
    {
        // Retrieve real User IDs
        var john = db.Users.First(u => u.FirstName == "John");
        var hector = db.Users.First(u => u.FirstName == "Hector");
        var alice = db.Users.First(u => u.FirstName == "Alice");

        db.Tickets.AddRange(
            new Ticket { Title = "Can't connect to PC", Description = "User cannot log in to Windows.", UserId = john.Id, Status = "Open", CreatedAt = DateTime.Now },
            new Ticket { Title = "Outlook crash", Description = "Outlook closes instantly after launch.", UserId = hector.Id, Status = "Open", CreatedAt = DateTime.Now },
            new Ticket { Title = "Printer not working", Description = "The printer in room 201 doesn't respond.", UserId = alice.Id, Status = "Open", CreatedAt = DateTime.Now }
        );
        db.SaveChanges();
    }
}

// Map controller endpoints automatically
app.MapControllers();

app.Run();