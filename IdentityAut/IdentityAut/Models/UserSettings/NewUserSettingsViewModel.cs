using System.ComponentModel.DataAnnotations;
using Business_Logic.ValidationRules;
using Microsoft.AspNetCore.Mvc;

namespace Business_Logic.Models.UserSettings
{
    public class NewUserSettingsViewModel : ValidatePatterns, ValidateErrors
    {

        public String Name { get; set; }

        public String Theme { get; set; }

        public Int32 PositiveRateFilter { get; set; }
    }
}
