using System.Data;
using FluentValidation;
using Repositores;

namespace Business_Logic.ValidationRules
{
    internal class UserLoginViewValidate : AbstractValidator<UserLoginViewModel>, ValidatePatterns, ValidateErrors
    {
        private IIdentityService _identityService;


        internal UserLoginViewValidate(IIdentityService identityService)
        {
            _identityService = identityService;

            RuleSet("PatternsCheck", () =>
            {
                RuleFor(x => x.Email).NotNull()
                    .WithMessage(ValidateErrors.NoEmail)
                    .EmailAddress()
                    .WithMessage(ValidateErrors.BadEmail);

                RuleFor(x => x.Password)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoPassword)
                    .Matches(ValidatePatterns.PasswordPattern)
                    .WithMessage(ValidateErrors.BadPassword);
            });


            RuleSet("IsExist", () =>
            {
                RuleFor(x => x)
                    .MustAsync(async (x, concellation) =>
                    {

                        bool exists = await _identityService.Login(x.Email, x.Password);

                        return exists;
                    }).WithMessage(ValidateErrors.BadTry);
            });
        }

    }
}
