using Business_Logic.Models.UserSettings;
using FluentValidation;
using IServices;
using Microsoft.Identity.Client;
using Repositores;
using UserConfigRepositores;

namespace Business_Logic.ValidationRules
{
    internal class UserSettingsViewValidate :
        AbstractValidator<NewUserSettingsViewModel>
        , ValidatePatterns
        , ValidateErrors
    {

        internal UserSettingsViewValidate()
        {

            RuleSet("PatternsCheck", () =>
            {
                RuleFor(x => x.Name)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoName)
                    .Matches(ValidatePatterns.NamePattern)
                    .WithMessage(ValidateErrors.BadName);


                RuleFor(x => x.PositiveRateFilter).NotNull()
                    .WithMessage(ValidateErrors.NoRateFilter)
                    .GreaterThan(0)
                    .WithMessage(ValidateErrors.BadRateFilter)
                    .LessThan(10)
                    .WithMessage(ValidateErrors.BadRateFilter);
            });


        }
    }
}
