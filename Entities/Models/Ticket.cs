using System;
using System.Collections.Generic;
using Solution.Users;
using System.ComponentModel.DataAnnotations;

namespace Solution.Tickets
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(80)]
        [Required]
        public string Title { get; set; } = string.Empty;

        [MaxLength(230)]
        [Required]
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Open";

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Ticket() { }

        public Ticket(string title, string description, int userId)
        {
            Title = title;
            Description = description;
            UserId = userId;
        }
    }
}
