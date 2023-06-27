using AutoMapper;
using Core.DTOs.Account;
using FluentValidation;
using IServices.Services;
using MVC.Models.AccountModels;
using MVC.Models.comment;
using MVC.ValidationRules;

namespace MVC.Filters.Validation.ValidationRules
{
    internal class CommentModelValidate :
        AbstractValidator<CommentModel>
        , IValidatePatterns, ValidateErrors
    {


        internal CommentModelValidate()
        {

            RuleSet("PatternsCheck", () =>
            {
                RuleFor(x => x.text)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoText)
                    .MaximumLength(50)
                    .WithMessage(ValidateErrors.ToMuchText)
                    .Matches(IValidatePatterns.TextPattern)
                    .WithMessage(ValidateErrors.BadText); ;

                RuleFor(x => x.id)
                    .GreaterThan(0)
                    .WithMessage(ValidateErrors.IncorrectId);
            });

        }
    }
}
