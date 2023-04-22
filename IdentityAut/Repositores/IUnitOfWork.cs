using System.Data;
using AspNetSamples.Abstractions.Data.Repositories;
using Entities_Context.Entities.UserNews;
using IServices.Repositories;

namespace Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        public IArticleRepository Articles { get; }
        public ISourceRepository Source { get; }
        public IUsersRepository Users { get; }
        public IRoleRepository Roles { get; }
        public IUsersRolesRepository UsersRoles { get; }
        public IUserInterfaceThemeRepository UserInterfaceTheme { get; }
        public Task<int> SaveChangesAsync();
    }
}