namespace Core.DTOs
{
    public class GetUserInfoWithSettingsDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Theme { get; set; } = "Default";
    }
}