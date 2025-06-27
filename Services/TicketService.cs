using Microsoft.EntityFrameworkCore;
using Solution.Data;
using Solution.Tickets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solution.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Ticket?> GetTicket(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<Ticket> CreateTicket(Ticket ticket)
        {
            ticket.Status = "Open";
            ticket.CreatedAt = DateTime.Now;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task<Ticket?> UpdateTicket(int id, Ticket updatedTicket)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return null;

            ticket.Title = updatedTicket.Title;
            ticket.Description = updatedTicket.Description;
            ticket.Status = updatedTicket.Status;

            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<bool> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
