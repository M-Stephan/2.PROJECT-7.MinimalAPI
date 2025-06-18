// Entities > Ticket.cs
using System;
using System.Collections.Generic;

namespace Solution.Tickets
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Ticket(int id, string title, string description, int userId)
        {
            Id = id;
            Title = title;
            Description = description;
            Status = "To do!";
            UserId = userId;
            CreatedAt = DateTime.Now;
        }
    }
}