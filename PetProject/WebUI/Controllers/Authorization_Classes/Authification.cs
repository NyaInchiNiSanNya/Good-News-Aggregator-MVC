using System.Text.RegularExpressions;
using Project.Abstractions;

namespace WebUI.Controllers.Classes
{
    public class Authification: IDisposable
    {
        private IUserRepository _IUserRepository;


        internal Authification(IUserRepository _IUserRepository)
        {
            this._IUserRepository = _IUserRepository;
        }

        public async Task<Boolean> User_Login(IUserRepository.UserLoginViewModel model)
        {
            if (await _IUserRepository.Authification_method(model))
            {
                return true;
            }
            return false;
        }


        public void Dispose() { }


    }
}
