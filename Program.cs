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
                Console.WriteLine($"SCRIPT ERROR: {ex}");
                throw;
            }

            Thread.Sleep(3000);
        }
    }

    // Seed users si la table Users est vide
    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Name = "John", Email = "Johndoe@domain.be" },
            new User { Name = "Hector", Email = "Hector@domain.be" },
            new User { Name = "Alice", Email = "alice@domain.be" },
            new User { Name = "Sophie", Email = "sophie@domain.be" }
        );
        db.SaveChanges(); // sauvegarde pour générer les Ids
    }

    // Seed tickets si la table Tickets est vide
    if (!db.Tickets.Any())
    {
        // On récupère les vrais Id des utilisateurs
        var john = db.Users.First(u => u.Name == "John");
        var hector = db.Users.First(u => u.Name == "Hector");
        var alice = db.Users.First(u => u.Name == "Alice");

        db.Tickets.AddRange(
            new Ticket { Title = "Can't connect to PC", Description = "User cannot log in to Windows.", UserId = john.Id, Status = "To do!", CreatedAt = DateTime.Now },
            new Ticket { Title = "Outlook crash", Description = "Outlook closes instantly after launch.", UserId = hector.Id, Status = "To do!", CreatedAt = DateTime.Now },
            new Ticket { Title = "Printer not working", Description = "The printer in room 201 doesn't respond.", UserId = alice.Id, Status = "To do!", CreatedAt = DateTime.Now }
        );
        db.SaveChanges();
    }
}

// **Important** : Mapping des endpoints et démarrage de l’app en dehors du scope
app.MapUserEndpoints();
app.MapTicketEndpoints();

app.Run();
