using Microsoft.AspNetCore.Mvc;
using Solution.Services;
using Solution.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
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
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets()
        {
            var tickets = await _ticketService.GetTickets();
            if (tickets == null || !tickets.Any()) return NotFound("No Tickets Found");
            return Ok(tickets);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TicketDTO>> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicket(id);
            if (ticket == null) return NotFound("Ticket Not Found");
            return Ok(ticket);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TicketDTO>> CreateTicket(TicketDTO ticketDto)
        {
            if (string.IsNullOrWhiteSpace(ticketDto.Title) || string.IsNullOrWhiteSpace(ticketDto.Description))
                return BadRequest("Title and Description are required");

            // Get userId from JWT claims
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
                return Unauthorized("User ID claim missing");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("User ID claim invalid");

            var createdTicket = await _ticketService.CreateTicket(ticketDto, userId);
            return CreatedAtAction(nameof(GetTicket), new { id = createdTicket.Id }, createdTicket);
        }


        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TicketDTO>> UpdateTicket(int id, TicketDTO updatedTicketDto)
        {
            if (string.IsNullOrWhiteSpace(updatedTicketDto.Title) || string.IsNullOrWhiteSpace(updatedTicketDto.Description))
                return BadRequest("Title and Description are required");

            var updatedTicket = await _ticketService.UpdateTicket(id, updatedTicketDto);
            if (updatedTicket == null) return NotFound("Ticket Not Found");

            return Ok(updatedTicket);
        }

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
