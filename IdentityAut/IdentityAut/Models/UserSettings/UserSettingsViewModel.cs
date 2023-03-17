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
        public string Name { get; set; }

        [Remote("CheckUserExist", "Settings",
            ErrorMessage = ValidateErrors.EmailExist)]
        public string Email { get; set; }

        public string Theme { get; set; }
    }
}
