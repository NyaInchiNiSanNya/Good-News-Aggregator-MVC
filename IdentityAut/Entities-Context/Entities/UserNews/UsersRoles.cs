using AspNetSamples.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_Context.Entities.UserNews
{
    public class UsersRoles : IBaseEntity
    {
        public Int32 Id { get; set; }

        public Int32 UserId { get; set; }
        public User User { get; set; }

        public Int32 RoleId { get; set; }
        public UserRole Role { get; set; }

    }
}
