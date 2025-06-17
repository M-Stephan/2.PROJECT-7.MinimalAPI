namespace Solution.User
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public void GetName()
        {
            Console.WriteLine($"Id: {Id}\nName: {Name}\nEmail: {Email}");
        }
    }
}


