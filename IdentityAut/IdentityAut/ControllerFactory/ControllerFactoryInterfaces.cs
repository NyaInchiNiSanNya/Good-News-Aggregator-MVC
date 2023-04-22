using AutoMapper;
using IServices;
using Repositores;
using UserConfigRepositores;

namespace MVC.ControllerFactory
{
    public interface IServiceFactory
    {
        IMapper createMapperService();
        IAuthService createIdentityService();
        IRoleService createRoleService();
        IUserService createAdminService();
        IConfiguration createConfigurationService();
        IArticleService createArticlesService();
        IUserInfoAndSettingsService createUserConfigService();
        IUiThemeService createThemeService();
    }
   
}
