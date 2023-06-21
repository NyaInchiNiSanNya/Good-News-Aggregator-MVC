using AutoMapper;
using IServices.Services;

namespace Web_Api_Controllers.ControllerFactory
{
    public interface IServiceFactory
    {
        IMapper CreateMapperService();
        IAuthService CreateIdentityService();
        IRoleService CreateRoleService();
        IUserService CreateAdminService();
        IConfiguration CreateConfigurationService();
        IArticleService CreateArticlesService();
        ISettingsService CreateUserConfigService();
        IUiThemeService CreateThemeService();
    }
   
}
