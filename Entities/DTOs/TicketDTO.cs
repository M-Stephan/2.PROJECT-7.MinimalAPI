using System;
using System.Collections.Generic;

namespace Solution.DTOs
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; } 
    }
}