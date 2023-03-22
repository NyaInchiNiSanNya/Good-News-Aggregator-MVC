namespace Core.DTOs
{
    public class GetUserInfoWithSettingsDTO
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public String Theme { get; set; }
        public Int32 PositiveRate { get; set; }
        public Int32 PositiveRateFilter { get; set; }
        public String? ProfilePicture { get; set; }
        public String Role { get; set; }
    }
}