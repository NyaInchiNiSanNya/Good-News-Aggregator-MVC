using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Entities_Context.Entities.UserNews
{
    public class UserRole : IBaseEntity
    {
        public Int32 Id { get; set; }
        public String Role { get; set; }

        public List<UsersRoles> User { get; set; }

    }
}
