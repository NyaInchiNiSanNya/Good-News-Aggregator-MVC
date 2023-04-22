using Core.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositores
{
    public interface IAuthService
    {
        public Task<Boolean> RegistrationAsync
            (UserRegistrationDTO model);


        public Task<Boolean> LoginAsync
            (UserLoginDTO model);

        public Task IdLogoutAsync();

        public Task<Boolean> isUserExistAsync(String Email);
    }
}
