using Core.DTOs.Account;

namespace IServices.Services
{
    public interface IUserService
    {
        public Task<List<UserDto>?> GetAllUsersWithRolesAsync();
    }
}
