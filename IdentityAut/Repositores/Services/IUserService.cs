using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Account;
using Microsoft.Identity.Client;

namespace IServices
{
    public interface IUserService
    {
        public Task<List<UserDTO>> GetAllUsersWithRolesAsync();
    }
}
