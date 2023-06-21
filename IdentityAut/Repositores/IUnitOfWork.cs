using IServices.Repositories;

namespace IServices
{
    public interface IUnitOfWork : IDisposable
    {
        
        public IArticleTagRepository ArticlesTags { get; }
        public ICommentRepository Comment { get; }
        public IArticleRepository Articles { get; }
        public ISourceRepository Source { get; }
        public IUsersRepository Users { get; }
        public IRoleRepository Roles { get; }
        public IUsersRolesRepository UsersRoles { get; }
        public IUserInterfaceThemeRepository UserInterfaceTheme { get; }
        public ITagRepository Tag { get; }
        public Task<int> SaveChangesAsync();
    }
}