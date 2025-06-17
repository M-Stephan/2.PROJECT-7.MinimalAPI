using Solution;
using Solution.User;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

User user1 = new(1, "John", "Johndoe@domain.be");
User user2 = new(2, "Hector", "Hector@domain.be");
User user3 = new(3, "Alice", "alice@domain.be");
User user4 = new(4, "Sophie", "sophie@domain.be");

List<User> users = new List<User>
{
    user1, user2, user3, user4
};

app.MapGet("/users", () =>
{
    return !users.Any() ? Results.NotFound("Aucun utilisateur trouvé.") : Results.Ok(users);
});

app.MapGet("/user/{id}", (int id) =>
{
    User? user = users.Where(p => p.Id == id).FirstOrDefault();
    return user == null ? Results.NotFound("User Not Found") : Results.Ok(user);
});

// Don't forget to add this line at the end of program, otherwise your api will never start.
app.Run();