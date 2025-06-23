using System.Collections.Generic;
using Solution.Tickets;

namespace Solution.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public List<Ticket> Tickets { get; set; } = new();

        // Empty Constructor for Ef Core
        public User() { }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
