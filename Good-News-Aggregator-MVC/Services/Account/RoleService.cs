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
using IServices.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Services.Account
{
    public class RoleService :IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public RoleService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));


            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<List<UserRole>?> GetUserRolesByUserIdAsync(int id)
        {
            if (id<1)
            {
                throw new ArgumentException(nameof(id));
            }

            List<UsersRoles>? userRoles = (await _unitOfWork.UsersRoles
                .FindBy(userRole=>userRole.UserId==id,userRole=>userRole.Role)
                .ToListAsync());
            
            if (userRoles.Count!=0)
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

        public async Task<List<UserRole>?> GetUserRolesByUserNameAsync(String email)
        {
            if (email.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(email));
            }

            User? user = (await _unitOfWork.Users.FindBy(x => x.Email.Equals(email)).FirstOrDefaultAsync());

            if (user is not null)
            {
                return await GetUserRolesByUserIdAsync(user.Id);
            }

            Log.Warning("Cant get user roles:email {0}", email);
            
            return null;
        }

        public async Task InitiateDefaultRolesAsync()
        {
            Log.Information("Attempt to create roles");

            var rolesFromConfig = _configuration["Roles:all"]!;

            if (rolesFromConfig.IsNullOrEmpty())
            {
                throw new ArgumentException("No roles are defined in the configuration file");
            }

            var roles = rolesFromConfig.Split(" ");

            Boolean anyChanges = false;

            foreach (String role in roles)
            {
                if (!await IsRoleExistsAsync(role))
                {
                    await _unitOfWork.Roles.AddAsync(new UserRole() { Role = role });

                    anyChanges = true;
                }
            }
            if (anyChanges)
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<Boolean> IsRoleExistsAsync(String role)
        {
            if (role.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(role));
            }
            
            return await _unitOfWork.Roles.FindBy(x => x.Role == role).FirstOrDefaultAsync() is not null;
        }


        public async Task<UserRole> GetDefaultRoleAsync()
        {
            var defaultRoleFromConfigFile = _configuration["Roles:default"];
            
            if (String.IsNullOrEmpty(defaultRoleFromConfigFile))
            {
                throw new ArgumentException("No default role is defined in the configuration file");
            }
            
            UserRole? defaultRole = await _unitOfWork.Roles
                .FindBy(x=>x.Role.Equals(defaultRoleFromConfigFile))
                .FirstOrDefaultAsync();

            if (defaultRole == null) 
            {
                
                await InitiateDefaultRolesAsync();

                defaultRole = await _unitOfWork.Roles
                    .FindBy(x => x.Role.Equals(defaultRoleFromConfigFile))
                    .FirstOrDefaultAsync();

                if (defaultRole == null)
                {
                    throw new InvalidOperationException("Can't initiate user roles");
                }
            }

            return defaultRole;
        }
    }
}
