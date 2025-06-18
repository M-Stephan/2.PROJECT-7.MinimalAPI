using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Solution.Users;
using Solution.Data;

namespace Solution.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/users", async (ApplicationDbContext db) =>
        {
            var users = await db.Users.ToListAsync();
            return users.Count == 0 ? Results.NotFound("No Users Found") : Results.Ok(users);
        });

        app.MapGet("/user/{id:int}", async (int id, ApplicationDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);
            return user == null ? Results.NotFound("User Not Found") : Results.Ok(user);
        });

        app.MapPost("/user", async (User user, ApplicationDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
                return Results.BadRequest("Name and Email are required");
            if (!user.Email.Contains("@") || !user.Email.Contains("."))
                return Results.BadRequest("Email is not valid");

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Created($"/user/{user.Id}", user);
        });

        app.MapPut("/user/{id:int}", async (int id, User updatedUser, ApplicationDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(updatedUser.Name) || string.IsNullOrWhiteSpace(updatedUser.Email))
                return Results.BadRequest("Name and Email are required");
            if (!updatedUser.Email.Contains("@") || !updatedUser.Email.Contains("."))
                return Results.BadRequest("Email is not valid");

            var user = await db.Users.FindAsync(id);
            if (user == null) return Results.NotFound("User Not Found");

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            await db.SaveChangesAsync();

            return Results.Ok(user);
        });

        app.MapDelete("/user/{id:int}", async (int id, ApplicationDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);
            if (user == null) return Results.NotFound("User Not Found");

            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Results.Ok($"User {id} deleted");
        });
    }
}
