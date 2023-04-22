using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IRoleService
    {
        public Task<List<UserRole>> GetUserRolesByUserIdAsync(Int32 Id);

        public Task<List<UserRole>> GetUserRolesByUserNameAsync(String Email);

        public Task<bool> IsRoleExistsAsync(string name);

        public Task InitiateDefaultRolesAsync();

        public Task<UserRole> GetDefaultRoleAsync();


    }
}
