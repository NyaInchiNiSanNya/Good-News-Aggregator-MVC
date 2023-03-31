using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.Account
{
    public class RoleService:IRoleService
    {
        private readonly UserArticleContext _userContext;


        public RoleService(UserArticleContext userContext
            , IMapper mapper)
        {
            if (userContext is null)
            {
                throw new ArgumentNullException(nameof(userContext));
            }

            _userContext = userContext;

        }

        public async Task<List<UserRole>> GetUserRolesByUserId(int Id)
        {
            User user = await _userContext.Users
                .AsNoTracking()
                .Where(x => x.Id == Id)
                .Include(x => x.Role)
                .ThenInclude(x => x.Role)
                .SingleOrDefaultAsync();

            return user.Role.Select(r=>r.Role).ToList();
        }

        public async Task<List<UserRole>> GetUserRolesByUserName(String Email)
        {
            User user = await _userContext.Users
                .AsNoTracking()
                .Where(x => x.Email.Equals(Email))
                .Include(x => x.Role)
                .ThenInclude(x => x.Role)
                .SingleOrDefaultAsync();

            return user.Role.Select(r => r.Role).ToList();
        }

        public async Task InitiateDefaultRolesAsync()
        {

            Boolean AnyChanges = false;

            if (! await IsRoleExistsAsync("User"))
            {
                await _userContext.Roles.AddAsync(new UserRole(){Role="User"});

                AnyChanges=true;
            }
            if (!await IsRoleExistsAsync("Admin"))
            {
                await _userContext.Roles.AddAsync(new UserRole() { Role = "Admin" });

                AnyChanges=true;
            }
            if (!await IsRoleExistsAsync("SuperAdmin"))
            {
                await _userContext.Roles.AddAsync(new UserRole() { Role = "SuperAdmin" });
               
                AnyChanges=true;
            }

            if (AnyChanges)
            {
                await _userContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IsRoleExistsAsync(string name)
        {
            return await _userContext.Users.AnyAsync(x => x.Name == name);
        }

        public async Task<UserRole> GetDefaultRole()
        {
            return await _userContext.Roles
                .AsNoTracking()
                .Where(x=>x.Role=="User")
                .FirstOrDefaultAsync();
        }
    }
}
