using Microsoft.AspNetCore.Mvc;
using Solution.Services;
using Solution.Tickets;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Solution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            var tickets = await _ticketService.GetTickets();
            if (tickets == null || !tickets.Any()) return NotFound("No Tickets Found");
            return Ok(tickets);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicket(id);
            if (ticket == null) return NotFound("Ticket Not Found");
            return Ok(ticket);
        }

        // Secure this action - only authenticated users can create tickets
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicket(Ticket ticket)
        {
            if (string.IsNullOrWhiteSpace(ticket.Title) || string.IsNullOrWhiteSpace(ticket.Description))
                return BadRequest("Title and Description are required");

            // Get userId from JWT claims
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
                return Unauthorized("User ID claim missing");

            ticket.UserId = int.Parse(userIdClaim.Value);

            var created = await _ticketService.CreateTicket(ticket);
            return CreatedAtAction(nameof(GetTicket), new { id = created.Id }, created);
        }

        // Secure update too
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Ticket>> UpdateTicket(int id, Ticket updatedTicket)
        {
            if (string.IsNullOrWhiteSpace(updatedTicket.Title) || string.IsNullOrWhiteSpace(updatedTicket.Description))
                return BadRequest("Title and Description are required");

            // Optionally, you can ensure user can only update their own tickets
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
                return Unauthorized("User ID claim missing");

            updatedTicket.UserId = int.Parse(userIdClaim.Value);

            var ticket = await _ticketService.UpdateTicket(id, updatedTicket);
            if (ticket == null) return NotFound("Ticket Not Found");

            return Ok(ticket);
        }

        // Secure delete too
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            var deleted = await _ticketService.DeleteTicket(id);
            if (!deleted) return NotFound("Ticket Not Found");

            return Ok($"Ticket {id} deleted");
        }
    }
}
