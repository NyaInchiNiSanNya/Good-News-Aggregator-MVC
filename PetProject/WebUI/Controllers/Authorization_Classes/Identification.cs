using System.Text.RegularExpressions;
using Project.Abstractions;

namespace WebUI.Controllers.Classes
{
    public class Identification : IDisposable
    {

        private IUserRepository _IUserRepository;


        internal Identification(IUserRepository _IUserRepository)
        {
            this._IUserRepository = _IUserRepository;
        }


        public async void Add_New_User(IUserRepository.UserRegistrationViewModel model)
        {
            await _IUserRepository.Add_New(model);
        }


        public async Task<Boolean> Check_Existence_User(IUserRepository.UserRegistrationViewModel model)
        {
            if (!await _IUserRepository.Existence_Check(model))
            {
                return false;
            }

            return true;
        }


        public void Dispose() { }
    }
}
