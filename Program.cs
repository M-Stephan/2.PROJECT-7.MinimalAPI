// using namespaces
using Solution;
using Solution.Controllers;
using Solution.Users;
using Solution.Tickets;
using Solution.Services;
using Solution.Data;
// Scalar
using Scalar.AspNetCore;
using Microsoft.OpenApi.Models;

// EF Core
using Microsoft.EntityFrameworkCore;
// JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// Encrypt password
using BCrypt.Net;

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

// Configure Swagger / OpenAPI with JWT security
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ticket API",
        Version = "v1",
        Description = "Api for create tickets with a login user",
        Contact = new OpenApiContact
        {
            Name = "Support API",
            Email = "ndc.dev.code@gmail.com",
            Url = new Uri("https://github.com/M-Stephan")
        }
    });

    // Add JWT Bearer auth in Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Define context
string? context = builder.Configuration.GetConnectionString("DefaultConnection");

// -- Add DbContext with the connection string --
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(context));

// Important : Register controllers support
builder.Services.AddControllers();

// Initialize Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();

// Build app
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    // Swagger endpoints (expose swagger json à cette URL)
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket API V1");
        c.RoutePrefix = "swagger"; // Swagger UI à /swagger
    });

    // Scalar API Reference configuré pour utiliser swagger.json exposé ci-dessus
    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
        options.AddPreferredSecuritySchemes("Bearer");
        options.AddHttpAuthentication("Bearer", auth =>
        {
            auth.Token = string.Empty; // vide par défaut
        });
    });
}
else
{
    // En prod, tu peux décider ce que tu veux
    app.MapScalarApiReference();
}

app.MapOpenApi(); // Si tu veux garder aussi cette ligne, elle est OK

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
            new User { FirstName = "John", LastName = "Doe", Email = "Johndoe@domain.be", PasswordHash = BCrypt.Net.BCrypt.HashPassword("jd123") },
            new User { FirstName = "Hector", LastName = "Lastor", Email = "Hector@domain.be", PasswordHash = BCrypt.Net.BCrypt.HashPassword("hector123") },
            new User { FirstName = "Alice", LastName = "Gretchen", Email = "alice@domain.be", PasswordHash = BCrypt.Net.BCrypt.HashPassword("alice123") },
            new User { FirstName = "Sophie", LastName = "Clapton", Email = "sophie@domain.be", PasswordHash = BCrypt.Net.BCrypt.HashPassword("sophie123") }
        );
        db.SaveChanges();
    }

    // Seed tickets if Tickets table is empty
    if (!db.Tickets.Any())
    {
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
