using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositores
{
    public interface IdentityRepository
    {
        public Task<Boolean> Registration
            (String Email, String password);
        
        public Task<Boolean> Login
            (String Email, String password);
    }
}
