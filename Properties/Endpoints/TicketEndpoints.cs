using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Solution.Tickets;
using Solution.Data;

namespace Solution.Endpoints;

public static class TicketEndpoints
{
    public static void MapTicketEndpoints(this WebApplication app)
    {
        // GET all tickets
        app.MapGet("/tickets", async (ApplicationDbContext db) =>
        {
            var tickets = await db.Tickets.ToListAsync();
            return tickets.Count == 0 ? Results.NotFound("No Tickets Found") : Results.Ok(tickets);
        });

        // GET ticket by id
        app.MapGet("/ticket/{id:int}", async (int id, ApplicationDbContext db) =>
        {
            var ticket = await db.Tickets.FindAsync(id);
            return ticket == null ? Results.NotFound("Ticket Not Found") : Results.Ok(ticket);
        });

        // POST create ticket
        app.MapPost("/ticket", async (Ticket ticket, ApplicationDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(ticket.Title) || string.IsNullOrWhiteSpace(ticket.Description))
                return Results.BadRequest("Title and Description are required");

            ticket.Status = "To do!"; // par défaut
            ticket.CreatedAt = DateTime.Now;

            db.Tickets.Add(ticket);
            await db.SaveChangesAsync();

            return Results.Created($"/ticket/{ticket.Id}", ticket);
        });

        // PUT update ticket
        app.MapPut("/ticket/{id:int}", async (int id, Ticket updatedTicket, ApplicationDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(updatedTicket.Title) || string.IsNullOrWhiteSpace(updatedTicket.Description))
                return Results.BadRequest("Title and Description are required");

            var ticket = await db.Tickets.FindAsync(id);
            if (ticket == null) return Results.NotFound("Ticket Not Found");

            ticket.Title = updatedTicket.Title;
            ticket.Description = updatedTicket.Description;
            ticket.Status = updatedTicket.Status;

            await db.SaveChangesAsync();
            return Results.Ok(ticket);
        });

        // DELETE ticket
        app.MapDelete("/ticket/{id:int}", async (int id, ApplicationDbContext db) =>
        {
            var ticket = await db.Tickets.FindAsync(id);
            if (ticket == null) return Results.NotFound("Ticket Not Found");

            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();
            return Results.Ok($"Ticket {id} deleted");
        });
    }
}
