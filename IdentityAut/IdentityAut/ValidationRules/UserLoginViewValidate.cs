using System.Data;
using AutoMapper;
using Core.DTOs.Account;
using FluentValidation;
using Repositores;
using Services.Account;

namespace Business_Logic.ValidationRules
{
    internal class UserLoginViewValidate : AbstractValidator<UserLoginViewModel>, ValidatePatterns, ValidateErrors
    {
        internal UserLoginViewValidate(IMapper _mapper,IIdentityService _IdentityService)
        {

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
            RuleSet("LoginCheck", () =>
            {
                RuleFor(x => x)
                .MustAsync(async (x, concellation) =>
                {

                    if (await _IdentityService.Login(_mapper.Map<UserLoginDTO>(x)))
                    {
                        return true;
                    }


                    return false;
                }).WithMessage(ValidateErrors.BadTry);
            });

        }

    }
}
