using Core.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositores
{
    public interface IIdentityService
    {
        public Task Registration
            (UserRegistrationDTO model);


        public Task<Boolean> Login
            (UserLoginDTO model);

        public Task IdLogout();

        public Task<Boolean> isUserExist(String Email);
    }
}
