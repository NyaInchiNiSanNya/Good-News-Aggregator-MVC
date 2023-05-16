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
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Services.Account
{
    public class RoleService :IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public RoleService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            _unitOfWork = unitOfWork;


            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            _configuration = configuration;
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
            Log.Information("Attempt to create roles");

            String[] RolesFromConfig = _configuration["Roles:all"].Split(" ");

            if (RolesFromConfig.Length==0)
            {
                throw new ArgumentException("No roles are defined in the configuration file");
            }
            
            Boolean AnyChanges = false;

            foreach (String role in RolesFromConfig)
            {
                if (!await IsRoleExistsAsync(role))
                {
                    await _unitOfWork.Roles.AddAsync(new UserRole() { Role = role });

                    AnyChanges = true;
                }
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
            String defaultRoleFromConfigFile = _configuration["Roles:all"];
            
            if (String.IsNullOrEmpty(defaultRoleFromConfigFile))
            {
                throw new ArgumentException("No default role is defined in the configuration file");
            }
            UserRole? defaultRole = await _unitOfWork.Roles
                .FindBy(x=>x.Role.Equals(defaultRoleFromConfigFile))
                .FirstOrDefaultAsync();

            if (defaultRole is null)
            {
                
                await InitiateDefaultRolesAsync();

                defaultRole = await _unitOfWork.Roles
                    .FindBy(x => x.Role.Equals(defaultRoleFromConfigFile))
                    .FirstOrDefaultAsync();
            }

            return defaultRole;
        }
    }
}
