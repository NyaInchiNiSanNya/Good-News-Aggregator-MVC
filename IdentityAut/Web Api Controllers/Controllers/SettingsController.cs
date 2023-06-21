using Core.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using Web_Api_Controllers.ControllerFactory;
using Web_Api_Controllers.Filters.Errors;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.ResponseModels;

namespace Web_Api_Controllers.Controllers
{
    [ApiController]
    [CustomExceptionFilter]
    [Route("settings")]
    public class SettingsController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public SettingsController
            (IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new NullReferenceException(nameof(serviceFactory));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> SetNewInfoConfig(Int32 id, [FromBody] PutSettingsRequest request)
        {
            // нужно ли проверять на null?

            Boolean response = await _serviceFactory.CreateUserConfigService()
            .SetNewUserInfoAsync(
                _serviceFactory.CreateMapperService().Map<userInfoWithSettingsDTO>(request)
                , HttpContext.User.Identity?.Name);

            if (response)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }


        //[HttpGet]
        //public async Task<IActionResult> GetInfoConfig()
        //{
        //    userInfoWithSettingsDTO infoSettings =
        //            await _serviceFactory.CreateUserConfigService()
        //                .GetUserInformationAsync(HttpContext.User.Identity.Name);


        //    if (infoSettings is not null)
        //    {
        //        Ok();
        //    }

        //    return NotFound();
        //}

        //[HttpPost]
        //public async Task<IActionResult> GetUserByteArrayPicture([FromBody] String userPicture)
        //{
        //    if (HttpContext.User.Identity.Name is not null)
        //    {
        //        await _serviceFactory.CreateUserConfigService()
        //            .SetNewProfilePictureByNameAsync(userPicture, HttpContext.User.Identity.Name);
        //    }

        //    return Ok();
        //}
    }
}
