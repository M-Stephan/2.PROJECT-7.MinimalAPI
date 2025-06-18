using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using Solution.Tickets;

namespace Solution.Endpoints;

public static class TicketEndpoints
{
    public static void MapTicketEndpoints(this WebApplication appTicket, List<Ticket> tickets)
    {
        appTicket.MapGet("/tickets", () =>
        {
            return !tickets.Any() ? Results.NotFound("No Tickets Found") : Results.Ok(tickets);
        });

        appTicket.MapGet("/ticket/{id}", (int id) =>
        {
            Ticket? ticket = tickets.FirstOrDefault(p => p.Id == id);
            return ticket == null ? Results.NotFound("Ticket Not Found") : Results.Ok(ticket);
        });

        appTicket.MapPost("/ticket", (Ticket ticket) =>
        {
            if (string.IsNullOrWhiteSpace(ticket.Title) || string.IsNullOrWhiteSpace(ticket.Description))
                return Results.BadRequest("Title and Description are required");

            tickets.Add(ticket);
            return Results.Created($"/ticket/{ticket.Id}", ticket);
        });

        appTicket.MapPut("/ticket/{id}", (int id, Ticket updatedTicket) =>
        {
            if (string.IsNullOrWhiteSpace(updatedTicket.Title) || string.IsNullOrWhiteSpace(updatedTicket.Description))
                return Results.BadRequest("Title and Description are required");

            var ticket = tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null) return Results.NotFound("Ticket Not Found");

            ticket.Title = updatedTicket.Title;
            ticket.Description = updatedTicket.Description;
            ticket.Status = updatedTicket.Status;

            return Results.Ok(ticket);
        });


        appTicket.MapDelete("/ticket/{id}", (int id) =>
        {
            var ticket = tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null) return Results.NotFound("Ticket Not Found");

            tickets.Remove(ticket);
            return Results.Ok($"Ticket {id} deleted");
        });
    }
}
