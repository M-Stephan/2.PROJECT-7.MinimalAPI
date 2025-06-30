using System.Collections.Generic;
using Solution.Tickets;
using System.ComponentModel.DataAnnotations;
using System.Security;


namespace Solution.Users
{
    public class User
    {
        // Unique Key
        [Key]
        public int Id { get; set; }

        // Required and max 25 characters
        [MaxLength(25)]
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [MaxLength(25)]
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        // Verification email
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Password hashed
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // List of all tickets of the User
        
        public List<Ticket>? Tickets { get; set; }

        // Empty Constructor for Ef Core
        public User() { }

        // Constructor
        public User(string firstname, string lastname, string email, string passwordHash)
        {
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            PasswordHash = passwordHash;

            Tickets = [];
        }
    }
}
