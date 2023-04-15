using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Repositores;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace MVC.Filters.Validation
{
    public class LoginValidationFilterAttribute : ActionFilterAttribute
    {

        private readonly IIdentityService _IdentityService;
        private readonly IMapper _mapper;

        public LoginValidationFilterAttribute(
            IIdentityService identityService,
            IMapper mapper)
        {
            if (identityService is null)
            {
                throw new NullReferenceException(nameof(identityService));

            }
            _IdentityService = identityService;


            if (mapper is null)
            {
                throw new NullReferenceException(nameof(mapper));

            }
            _mapper = mapper;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {

            var FormObject = 
                context.ActionArguments.SingleOrDefault(p =>
                    p.Value is UserLoginViewModel);

            var validationResult = await AccountValidationHelper
                .AccountLoginValidator(
                    (UserLoginViewModel)FormObject.Value, _mapper, _IdentityService);

            
            if (!validationResult.IsValid)
            {
                var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();

                if (context.ActionArguments.TryGetValue("user", out var userObj) && userObj is UserLoginViewModel user)
                {
                    Log.Warning("Validation error when logging in with IP: {0}.Email:{1}", ip,user.Email);
                }
                else
                {
                    Log.Warning("Validation error when logging in with IP: {0}", ip);
                }

                foreach (var Errors in validationResult.Errors)
                {
                    context.ModelState.AddModelError(Errors.PropertyName, Errors.ErrorMessage);
                }

                context.Result = new ViewResult
                {
                    ViewName = "Login",
                };
            }

            

        }
    }
}
