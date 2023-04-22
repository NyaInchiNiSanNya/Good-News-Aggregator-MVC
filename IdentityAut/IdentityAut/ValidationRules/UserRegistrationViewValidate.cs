using FluentValidation;
using Repositores;

namespace Business_Logic.ValidationRules
{
    internal class UserRegistrationViewValidate :
        AbstractValidator<UserRegistrationViewModel>
        , ValidatePatterns, ValidateErrors
    {


        internal UserRegistrationViewValidate()
        {

            RuleSet("PatternsCheck", () =>
            {
                RuleFor(x => x.Name)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoName)
                    .Matches(ValidatePatterns.NamePattern)
                    .WithMessage(ValidateErrors.BadName);

                RuleFor(x => x.Email)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoEmail)
                    .EmailAddress()
                    .WithMessage(ValidateErrors.BadEmail);

                RuleFor(x => x.Password)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoPassword)
                    .Matches(ValidatePatterns.PasswordPattern)
                    .WithMessage(ValidateErrors.BadPassword);

                RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password)
                    .WithMessage(ValidateErrors.BadConfirm);
            });

        }
    }
}
