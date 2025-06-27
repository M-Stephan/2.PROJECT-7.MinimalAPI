using System.Collections.Generic;
using System.Threading.Tasks;
using Solution.Tickets;

namespace Solution.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetTickets();
        Task<Ticket?> GetTicket(int id);
        Task<Ticket> CreateTicket(Ticket ticket);
        Task<Ticket?> UpdateTicket(int id, Ticket ticket);
        Task<bool> DeleteTicket(int id);
    }
}
