using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using Solution.Users;

namespace Solution.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app, List<User> users)
    {
        app.MapGet("/users", () =>
        {
            return !users.Any() ? Results.NotFound("No Users Found") : Results.Ok(users);
        });

        app.MapGet("/user/{id}", (int id) =>
        {
            User? user = users.FirstOrDefault(p => p.Id == id);
            return user == null ? Results.NotFound("User Not Found") : Results.Ok(user);
        });

        app.MapPost("/user", (User user) =>
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
                return Results.BadRequest("Name and Email are required");

            if (!user.Email.Contains("@") || !user.Email.Contains("."))
                return Results.BadRequest("Email is not valid");

            users.Add(user);
            return Results.Created($"/user/{user.Id}", user);
        });

        app.MapPut("/user/{id}", (int id, User updatedUser) =>
        {
            if (string.IsNullOrWhiteSpace(updatedUser.Name) || string.IsNullOrWhiteSpace(updatedUser.Email))
                return Results.BadRequest("Name and Email are required");

            if (!updatedUser.Email.Contains("@") || !updatedUser.Email.Contains("."))
                return Results.BadRequest("Email is not valid");

            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return Results.NotFound("User Not Found");

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;

            return Results.Ok(user);
        });

        app.MapDelete("/user/{id}", (int id) =>
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return Results.NotFound("User Not Found");

            users.Remove(user);
            return Results.Ok($"User {id} deleted");
        });
    }
}
