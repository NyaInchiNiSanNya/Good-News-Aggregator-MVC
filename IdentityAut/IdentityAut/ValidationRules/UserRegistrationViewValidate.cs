using FluentValidation;
using Repositores;

namespace Business_Logic.ValidationRules
{
    internal class UserRegistrationViewValidate :
        AbstractValidator<UserRegistrationViewModel>
        , ValidatePatterns, ValidateErrors
    {
        private IIdentityService _identityService;


        internal UserRegistrationViewValidate(IIdentityService identityService)
        {
            _identityService = identityService;

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
                    .NotNull()
                    .WithMessage(ValidateErrors.BadConfirm)
                    .Equal(x => x.Password)
                    .WithMessage(ValidateErrors.BadConfirm);
            });

            RuleSet("IsExist", () =>
            {
                RuleFor(x => x)
                    .MustAsync(async (x, concellation) =>
                    {

                        bool exists = await _identityService.Registration(x.Name, x.Email, x.Password);

                        return exists;
                    }).WithMessage(ValidateErrors.EmailExist);
            });

        }
    }
}
