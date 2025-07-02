using System.Collections.Generic;
using System.Threading.Tasks;
using Solution.DTOs;

namespace Solution.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDTO>> GetTickets();
        Task<TicketDTO?> GetTicket(int id);
        Task<TicketDTO> CreateTicket(TicketDTO ticketDto, int userId);
        Task<TicketDTO?> UpdateTicket(int id, TicketDTO ticket);
        Task<bool> DeleteTicket(int id);
    }
}
