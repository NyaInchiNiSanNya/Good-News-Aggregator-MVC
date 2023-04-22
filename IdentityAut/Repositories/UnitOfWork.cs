using Abstract;
using AspNetSamples.Abstractions.Data.Repositories;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices.Repositories;

namespace AspNetSamples.Repositories;

public class UnitOfWork : IUnitOfWork
{

    private readonly UserArticleContext _dbContext;

    private readonly IArticleRepository _articlesRepository;
    private readonly ISourceRepository _sourceRepository;
    private readonly IUsersRepository _userRepository;
    private readonly IUsersRolesRepository _usersRolesRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserInterfaceThemeRepository _userInterfaceThemeRepository;

    public UnitOfWork(UserArticleContext dbContext,
        IArticleRepository articleRepository,
        ISourceRepository sourceRepository,
        IUsersRepository userRepository,
        IRoleRepository roleRepository,
        IUsersRolesRepository usersRolesRepository,
        IUserInterfaceThemeRepository interfaceThemeRepository)
    {
        _dbContext = dbContext;
        _articlesRepository = articleRepository;
        _sourceRepository = sourceRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _usersRolesRepository = usersRolesRepository;
        _userInterfaceThemeRepository = interfaceThemeRepository;
    }

    public IArticleRepository Articles => _articlesRepository;
    public ISourceRepository Source => _sourceRepository;
    public IUsersRepository Users => _userRepository;
    public IRoleRepository Roles => _roleRepository;
    public IUsersRolesRepository UsersRoles => _usersRolesRepository;
    public IUserInterfaceThemeRepository UserInterfaceTheme => _userInterfaceThemeRepository;
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}