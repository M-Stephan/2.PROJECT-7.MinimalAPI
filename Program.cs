using Solution;
using Solution.Endpoints;
using Scalar.AspNetCore;
using Solution.Users;
using Solution.Tickets; 
using Microsoft.EntityFrameworkCore;
using Solution.Data;

// -- Build ASP.NET environnement --
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// -- Add DbContext with the connection string --
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

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
                Console.WriteLine("Could not connect to the database.");
                throw;

            Thread.Sleep(3000);
        }
    }


    if (!db.Tickets.Any())
    {
        db.Tickets.AddRange(
            new Ticket { Title = "Can't connect to PC", Description = "User cannot log in to Windows.", UserId = 1, Status = "To do!", CreatedAt = DateTime.Now },
            new Ticket { Title = "Outlook crash", Description = "Outlook closes instantly after launch.", UserId = 2, Status = "To do!", CreatedAt = DateTime.Now },
            new Ticket { Title = "Printer not working", Description = "The printer in room 201 doesn't respond.", UserId = 3, Status = "To do!", CreatedAt = DateTime.Now }
        );
    }

    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Name = "John", Email = "Johndoe@domain.be" },
            new User { Name = "Hector", Email = "Hector@domain.be" },
            new User { Name = "Alice", Email = "alice@domain.be" },
            new User { Name = "Sophie", Email = "sophie@domain.be" }
        );
    }

    db.SaveChanges();
}

app.MapUserEndpoints();

app.MapTicketEndpoints();

app.Run();
