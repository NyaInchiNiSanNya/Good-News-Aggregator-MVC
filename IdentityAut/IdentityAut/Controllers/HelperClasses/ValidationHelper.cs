using Business_Logic.Models.UserSettings;
using Business_Logic.ValidationRules;
using FluentValidation;
using FluentValidation.Results;
using Repositores;
using UserConfigRepositores;

namespace Business_Logic.Controllers.HelperClasses
{
    internal static class AccountValidationHelper
    {

        internal async static Task<ValidationResult> AccountLoginValidator
            (UserLoginViewModel model, IIdentityService _IdentityService)
        {
            UserLoginViewValidate validator = new UserLoginViewValidate(_IdentityService);

            var result = validator.Validate(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));


            if (result.IsValid)
            {
                result = await validator.ValidateAsync(model,
                    options =>
                        options.IncludeRuleSets("IsExist"));
            }


            return result;
        }


        internal async static Task<ValidationResult> AccountRegistrationValidator
            (UserRegistrationViewModel model, IIdentityService _IdentityService)
        {

            UserRegistrationViewValidate validator = new UserRegistrationViewValidate(_IdentityService);

            var result = validator.Validate(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));


            if (result.IsValid)
            {
                result = await validator.ValidateAsync(model,
                    options =>
                        options.IncludeRuleSets("IsExist"));
            }

            return result;
        }


        internal async static Task<ValidationResult> InfoSettingsValidator
            (UserSettingsViewModel model, IUserInfoAndSettingsService _userConfigService)
        {

            UserSettingsViewValidate validator = new UserSettingsViewValidate(_userConfigService);

            var result = await validator.ValidateAsync(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));
            return result;
        }
    }
}
