using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MVC.Filters.Validation.ValidationRules;
using MVC.Models.AccountModels;
using MVC.Models.comment;
using Serilog;

namespace MVC.Filters.Validation
{
    public class CommentValidationFilterAttribute : ActionFilterAttribute
    {

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var formObject =
                context.ActionArguments.SingleOrDefault(p =>
                    p.Value is CommentModel);

            var validationResult = await AccountValidationHelper
                .CommentValidator(((CommentModel)formObject.Value!));

            if (!validationResult.IsValid)
            {
                foreach (var errors in validationResult.Errors)
                {
                    context.ModelState.AddModelError(errors.PropertyName, errors.ErrorMessage);
                }
            }

        }
    }
}
