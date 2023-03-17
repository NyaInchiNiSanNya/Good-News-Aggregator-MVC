namespace Entities_Context.Entities.UserNews
{
    public class UserInformation
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public Int32 UserConfigId { get; set; }
        public UserConfig UserConfig { get; set; }
    }
}
