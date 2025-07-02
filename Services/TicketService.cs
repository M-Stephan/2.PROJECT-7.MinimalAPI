using Microsoft.EntityFrameworkCore;
using Solution.Data;
using Solution.Tickets;
using Solution.DTOs;
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

        public async Task<IEnumerable<TicketDTO>> GetTickets()
        {
            var tickets = await _context.Tickets.ToListAsync();
            var ticketDtos = tickets.Select(t => new TicketDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            }).ToList();

            return ticketDtos;
        }


        public async Task<TicketDTO?> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return null;

            return new TicketDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                CreatedAt = ticket.CreatedAt
            };
        }

        public async Task<TicketDTO> CreateTicket(TicketDTO ticketDto, int userId)
        {
            // Optionnel : vérifier que userId existe bien dans DB
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists) throw new Exception("User not found");

            var ticket = new Ticket
            {
                Title = ticketDto.Title ?? string.Empty,
                Description = ticketDto.Description ?? string.Empty,
                Status = "Open",
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            ticketDto.Id = ticket.Id;
            ticketDto.Status = ticket.Status;
            ticketDto.CreatedAt = ticket.CreatedAt;

            return ticketDto;
        }

        public async Task<TicketDTO?> UpdateTicket(int id, TicketDTO ticketDto)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return null;

            ticket.Title = ticketDto.Title ?? string.Empty;
            ticket.Description = ticketDto.Description ?? string.Empty;
            ticket.Status = ticketDto.Status ?? "Open";

            await _context.SaveChangesAsync();

            ticketDto.Id = ticket.Id;
            ticketDto.CreatedAt = ticket.CreatedAt;

            return ticketDto;
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
