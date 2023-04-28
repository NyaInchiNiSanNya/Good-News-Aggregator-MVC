using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using AutoMapper;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.Account
{
    public class RoleService :IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;


        public RoleService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            _unitOfWork = unitOfWork;

        }

        public async Task<List<UserRole>> GetUserRolesByUserIdAsync(int Id)
        {
            List<UsersRoles>? userRoles = (await _unitOfWork.UsersRoles
                .FindBy(userRole=>userRole.UserId==Id,userRole=>userRole.Role)
                .ToListAsync());
            
            if (userRoles is not null)
            {
                List<UserRole> Roles=new List<UserRole>();
                
                foreach (var role in userRoles)
                {
                    Roles.Add(role.Role);
                }
                return Roles;
            }
            
            return null;
        }

        public async Task<List<UserRole>> GetUserRolesByUserNameAsync(String Email)
        {
            User? user = (await _unitOfWork.Users.FindBy(x => x.Email.Equals(Email)).FirstOrDefaultAsync());

            if (user is not null)
            {
                return await GetUserRolesByUserIdAsync(user.Id);
            }

            return null;
        }

        public async Task InitiateDefaultRolesAsync()
        {

            Boolean AnyChanges = false;

            if (! await IsRoleExistsAsync("User"))
            {
                await _unitOfWork.Roles.AddAsync(new UserRole(){Role="User"});

                AnyChanges=true;
            }

            if (!await IsRoleExistsAsync("Admin"))
            {
                await _unitOfWork.Roles.AddAsync(new UserRole() { Role = "Admin" });

                AnyChanges=true;
            }

            if (!await IsRoleExistsAsync("SuperAdmin"))
            {
                await _unitOfWork.Roles.AddAsync(new UserRole() { Role = "SuperAdmin" });
               
                AnyChanges=true;
            }

            if (AnyChanges)
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> IsRoleExistsAsync(String role)
        {
            return await _unitOfWork.Roles.FindBy(x => x.Role == role).FirstOrDefaultAsync() is not null;
        }


        public async Task<UserRole> GetDefaultRoleAsync()
        {
            UserRole? defaultRole = await _unitOfWork.Roles
                .FindBy(x=>x.Role.Equals("User"))
                .FirstOrDefaultAsync();

            if (defaultRole is null)
            {
                await InitiateDefaultRolesAsync();

                defaultRole = await _unitOfWork.Roles
                    .FindBy(x => x.Role.Equals("User"))
                    .FirstOrDefaultAsync();
            }

            return defaultRole;
        }
    }
}
