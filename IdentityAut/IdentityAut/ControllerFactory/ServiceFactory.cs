using AutoMapper;
using IServices;
using Repositores;
using UserConfigRepositores;

namespace MVC.ControllerFactory
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        IUserService IServiceFactory.createAdminService()
        {
            return _serviceProvider.GetService<IUserService>() 
                   ?? throw new NullReferenceException(nameof(IUserService));
        }

        IArticleService IServiceFactory.createArticlesService()
        {
            return _serviceProvider.GetService<IArticleService>()
                   ?? throw new NullReferenceException(nameof(IArticleService));
        }

        IConfiguration IServiceFactory.createConfigurationService()
        {
            return _serviceProvider.GetService<IConfiguration>()
                   ?? throw new NullReferenceException(nameof(IConfiguration));
        }

        IAuthService IServiceFactory.createIdentityService()
        {
            return _serviceProvider.GetService<IAuthService>()
                   ?? throw new NullReferenceException(nameof(IAuthService));
        }

        IMapper IServiceFactory.createMapperService()
        {
            return _serviceProvider.GetService<IMapper>()
                   ?? throw new NullReferenceException(nameof(IMapper));
        }

        IRoleService IServiceFactory.createRoleService()
        {
            return _serviceProvider.GetService<IRoleService>()
                   ?? throw new NullReferenceException(nameof(IRoleService));
        }

        IUiThemeService IServiceFactory.createThemeService()
        {
            return _serviceProvider.GetService<IUiThemeService>()
                   ?? throw new NullReferenceException(nameof(IUiThemeService));
        }

        IUserInfoAndSettingsService IServiceFactory.createUserConfigService()
        {
            return _serviceProvider.GetService<IUserInfoAndSettingsService>()
                   ?? throw new NullReferenceException(nameof(IUserInfoAndSettingsService));
        }
    }
}
