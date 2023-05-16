using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Core.DTOs.Account;
using System.IO.Pipelines;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Repositores;
using Serilog;

namespace MVC.Filters.Validation
{
    public class RegistrationValidationFilterAttribute : ActionFilterAttribute
    {


        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var FormObject =
                context.ActionArguments.SingleOrDefault(p =>
                    p.Value is UserRegistrationViewModel);

            var validationResult = await AccountValidationHelper
                .AccountRegistrationValidator((UserRegistrationViewModel)FormObject.Value);


            if (!validationResult.IsValid)
            {
                var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
                
                Log.Warning("Validation error occurred during user registration. IP: {0}", ip);
                
                foreach (var Errors in validationResult.Errors)
                {
                    context.ModelState.AddModelError(Errors.PropertyName, Errors.ErrorMessage);
                }
                
                context.Result = new ViewResult
                {
                    ViewName = "Registration",
                };
            }



        }
    }
}
