using AutoMapper;
using Business_Logic.Controllers.HelperClasses;
using Business_Logic.Models.UserSettings;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Repositores;

namespace MVC.Filters.Validation
{
    public class SettingFilter
    {
        public class SettingsValidationFilterAttribute : ActionFilterAttribute
        {

            public override async void OnActionExecuting(ActionExecutingContext context)
            {
                var FormObject =
                    context.ActionArguments.SingleOrDefault(p =>
                        p.Value is UserSettingsViewModel);

                var validationResult = await AccountValidationHelper
                    .InfoSettingsValidator((UserSettingsViewModel)FormObject.Value);


                if (!validationResult.IsValid)
                {
                    foreach (var Errors in validationResult.Errors)
                    {
                        context.ModelState.AddModelError(Errors.PropertyName, Errors.ErrorMessage);
                    }
                    context.Result = new ViewResult
                    {
                        ViewName = "Settings",
                    };
                }



            }
        }
    }
}
