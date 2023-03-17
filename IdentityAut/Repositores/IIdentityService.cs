using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositores
{
    public interface IIdentityService
    {
        public Task<Boolean> Registration
            (String Name, String Email, String password);
        

        public Task<Boolean> Login
            (String Email, String password);

        public Task IdLogout();

        public Task<Boolean> isUserExist(String email);
    }
}
