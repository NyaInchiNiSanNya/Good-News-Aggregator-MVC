using Business_Logic.Models.UserSettings;
using FluentValidation;
using Microsoft.Identity.Client;
using Repositores;
using Services.Account;
using UserConfigRepositores;

namespace Business_Logic.ValidationRules
{
    internal class UserSettingsViewValidate :
        AbstractValidator<UserSettingsViewModel>
        , ValidatePatterns
        , ValidateErrors
    {
        private IUserInfoAndSettingsService _userInfoAndSettingsService;


        internal UserSettingsViewValidate(IUserInfoAndSettingsService userInfoAndSettingsService)
        {
            _userInfoAndSettingsService = userInfoAndSettingsService;

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

                        if (x.Equals("Dark") || x.Equals("Default"))
                        {
                            return true;
                        }


                        return false;
                    });
            });


        }
    }
}
