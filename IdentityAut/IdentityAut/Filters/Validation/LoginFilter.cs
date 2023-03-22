using Business_Logic.Controllers.HelperClasses;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Repositores;

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
                throw new NullReferenceException();

            }
            _IdentityService = identityService;


            if (mapper is null)
            {
                throw new NullReferenceException();

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
