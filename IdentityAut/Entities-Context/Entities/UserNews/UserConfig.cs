namespace Entities_Context.Entities.UserNews
{
    public class UserConfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Config { get; set; } = "Default";
    }
}