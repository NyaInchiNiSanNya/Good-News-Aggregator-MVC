using Core.DTOs.Account;

namespace IServices.Services
{
    public interface IAuthService
    {
        public Task<Boolean> RegistrationAsync
            (UserRegistrationDto model);


        public Task<Boolean> LoginAsync
            (UserLoginDto? model);

        public Task<Boolean> IsUserExistAsync(String email);
    }
}
