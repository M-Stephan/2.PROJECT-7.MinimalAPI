using System.Collections.Generic;

namespace Solution.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public List<string> TicketTitles { get; set; } = [];
    }
}