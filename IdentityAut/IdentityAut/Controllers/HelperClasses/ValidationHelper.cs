using AutoMapper;
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
            (UserLoginViewModel model,IMapper mapper,IIdentityService _identityService)
        {
            UserLoginViewValidate validator = new UserLoginViewValidate(mapper,_identityService);

            var result = await validator.ValidateAsync(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));

            if (result.IsValid)
            {
                result = await validator.ValidateAsync(model,
                    options =>
                        options.IncludeRuleSets("LoginCheck"));
            }

            return result;
        }


        internal async static Task<ValidationResult> AccountRegistrationValidator
            (UserRegistrationViewModel model)
        {
            Thread.Sleep(20000);
            UserRegistrationViewValidate validator = new UserRegistrationViewValidate();

            var result = await validator.ValidateAsync(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));

           

            return result;
        }


        internal async static Task<ValidationResult> InfoSettingsValidator
            (UserSettingsViewModel model)
        {

            UserSettingsViewValidate validator = new UserSettingsViewValidate();

            var result = await validator.ValidateAsync(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));
            return result;
        }
    }
}
