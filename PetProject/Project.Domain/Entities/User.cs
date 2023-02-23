namespace Project.Domain.Entities.User
{
    public class User
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Config { get; set; } = "Default";
        public String Role { get; set; } = "User";
    }
}