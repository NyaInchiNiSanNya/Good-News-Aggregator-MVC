﻿using AutoMapper;
using Business_Logic.Models.UserSettings;
using Business_Logic.ValidationRules;
using FluentValidation;
using FluentValidation.Results;
using IServices;
using Repositores;
using UserConfigRepositores;

namespace MVC.Filters.Validation
{
    internal static class AccountValidationHelper
    {

        internal async static Task<ValidationResult> AccountLoginValidator
            (UserLoginViewModel model, IMapper mapper, IIdentityService _identityService)
        {
            UserLoginViewValidate validator = new UserLoginViewValidate(mapper, _identityService);

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
            UserRegistrationViewValidate validator = new UserRegistrationViewValidate();

            var result = await validator.ValidateAsync(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));



            return result;
        }


        internal async static Task<ValidationResult> InfoSettingsValidator
            (NewUserSettingsViewModel model)
        {

            UserSettingsViewValidate validator = new UserSettingsViewValidate();

            var result = await validator.ValidateAsync(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));
            return result;
        }
    }
}
