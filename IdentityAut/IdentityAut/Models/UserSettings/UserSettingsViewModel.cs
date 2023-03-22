using System.ComponentModel.DataAnnotations;
using Business_Logic.ValidationRules;
using Microsoft.AspNetCore.Mvc;

namespace Business_Logic.Models.UserSettings
{
    public class UserSettingsViewModel : ValidatePatterns, ValidateErrors
    {
        [Required(ErrorMessage = ValidateErrors.NoName)]
        [RegularExpression(ValidatePatterns.NamePattern
            , ErrorMessage = ValidateErrors.BadName)]
        public String Name { get; set; }

        [Remote("", "Settings",
            ErrorMessage = ValidateErrors.EmailExist)]
        public String Email { get; set; }

        public String Theme { get; set; }
        public Int32 PositiveRate { get; set; }
        public Int32 PositiveRateFilter { get; set; }
        public String? ProfilePicture { get; set; }
        public String Role { get; set; }
    }
}
