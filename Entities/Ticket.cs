// Entities > Ticket.cs
using System;
using System.Collections.Generic;
using Solution.Users;

namespace Solution.Tickets
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // necessary empty constructor for EF Core
        public Ticket() { }

        public Ticket(int id, string title, string description, int userId)
        {
            Id = id;
            Title = title;
            Description = description;
            Status = "Open";
            UserId = userId;
            CreatedAt = DateTime.Now;
        }
    }
}