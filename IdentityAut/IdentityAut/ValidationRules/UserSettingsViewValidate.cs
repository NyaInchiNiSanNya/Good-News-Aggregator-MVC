using Business_Logic.Models.UserSettings;
using FluentValidation;
using Microsoft.Identity.Client;
using Repositores;
using UserConfigRepositores;

namespace Business_Logic.ValidationRules
{
    internal class UserSettingsViewValidate :
        AbstractValidator<UserSettingsViewModel>
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

                //нарушен солид
                RuleFor(x => x.Theme)
                    .MustAsync(async (x, concellation) =>
                    {

                        if (x.Equals("dark") || x.Equals("default"))
                        {
                            return true;
                        }


                        return false;
                    });

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
