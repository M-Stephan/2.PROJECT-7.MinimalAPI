using Solution;
using Solution.Endpoints;
using Scalar.AspNetCore;
using Solution.Users;
using Solution.Tickets; 

// -- Build ASP.NET environnement --
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
var app = builder.Build();

// -- Use scalar reference in the project is in development --
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// -- Users --
User user1 = new(1, "John", "Johndoe@domain.be");
User user2 = new(2, "Hector", "Hector@domain.be");
User user3 = new(3, "Alice", "alice@domain.be");
User user4 = new(4, "Sophie", "sophie@domain.be");

// -- List of all users --
List<User> users = new List<User> { user1, user2, user3, user4 };
app.MapUserEndpoints(users);

// -- Tickets --
Ticket ticket1 = new(1, "Can't connect to PC", "User cannot log in to Windows.", user1.Id);
Ticket ticket2 = new(2, "Outlook crash", "Outlook closes instantly after launch.", user2.Id);
Ticket ticket3 = new(3, "Printer not working", "The printer in room 201 doesn't respond.", user3.Id);

// -- List of all tickets --
List<Ticket> tickets = new List<Ticket> { ticket1, ticket2, ticket3 };
app.MapTicketEndpoints(tickets); // Register the endpoints

// -- Run doing stay to the end --
app.Run();
