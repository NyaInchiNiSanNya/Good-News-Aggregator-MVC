﻿using Entities_Context.Entities.UserNews;

namespace IServices.Services
{
    public interface IRoleService
    {
        public Task<List<UserRole>?> GetUserRolesByUserIdAsync(Int32 id);

        public Task<List<UserRole>?> GetUserRolesByUserNameAsync(String email);

        public Task<Boolean> IsRoleExistsAsync(String name);

        public Task InitiateDefaultRolesAsync();

        public Task<UserRole> GetDefaultRoleAsync();


    }
}
