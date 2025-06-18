namespace Solution.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        // Empty Constructor for Ef Core
        public User() { }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
