namespace Core.DTOs.Account
{
    public class userInfoWithSettingsDTO
    {
        public string Name { get; set; }
        public string Theme { get; set; }
        public List<string> AllThemes { get; set; }
        public int PositiveRate { get; set; }
        public int PositiveRateFilter { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}