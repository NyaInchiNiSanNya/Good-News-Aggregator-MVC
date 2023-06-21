using IServices;
using Entities_Context.Entities.UserNews;
using IServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Services.Account
{

    public class UiThemeService:IUiThemeService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;


        public UiThemeService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }


        public async Task<Int32> GetIdThemeByStringAsync(String theme)
        {
            if (await IsThemeExistByNameAsync(theme))
            {
                return (await _unitOfWork.UserInterfaceTheme.FindBy(x=>x.Theme.Equals(theme))
                    .FirstOrDefaultAsync())!.Id;
            }

            return await GetIdDefaultThemeAsync();
        }



        public async Task InitiateThemeAsync()
        {
            Log.Information("Attempt to create themes");

            String[]? themesFromConfigFile = _configuration["Themes:all"]?.Split(" ");
           
            Boolean anyChanges = false;

            if (themesFromConfigFile is null)
            {
                throw new ArgumentException("No themes are defined in the configuration file");
            }

            foreach (var theme in themesFromConfigFile)
            {
                if (!await IsThemeExistByNameAsync(theme))
                {
                    await _unitOfWork.UserInterfaceTheme.AddAsync(new SiteTheme() { Theme = theme });

                    anyChanges = true;
                }
            }

            if (anyChanges)
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<Int32> GetIdDefaultThemeAsync()
        {
            String? defaultTheme = _configuration["Themes:default"];

            if (String.IsNullOrEmpty(defaultTheme))
            {
                throw new ArgumentException("No default theme is defined in the configuration file");
            }

            var theme = await _unitOfWork.UserInterfaceTheme
                .FindBy(x => x.Theme == defaultTheme)
                .FirstOrDefaultAsync();
            
            Int32 Id = theme?.Id ?? 0;

            if (Id == 0)
            {
                await InitiateThemeAsync();

                theme = await _unitOfWork.UserInterfaceTheme
                    .FindBy(x => x.Theme == defaultTheme)
                    .FirstOrDefaultAsync();
                
                if (theme is null || theme.Id==0)
                {
                    throw new InvalidOperationException("Cant get default theme id");
                }

                return theme?.Id ?? 0;
            }

            return Id;
        }

        public async Task<List<String>> GetAllThemesAsync()
        {
            List<SiteTheme> themeList = await _unitOfWork.UserInterfaceTheme.GetAsQueryable()
                .ToListAsync();

            List<String> allThemes = new List<string>();

            if (themeList is not null)
            {

                foreach (var theme in themeList)
                {
                  allThemes.Add(theme.Theme);  
                }
            }
            else
            {
                await InitiateThemeAsync();

                if (themeList != null)
                    foreach (var theme in themeList)
                    {
                        allThemes.Add(theme.Theme);
                    }
            }

            return allThemes;
        }

        public async Task<Boolean> IsThemeExistByNameAsync(String theme)
        {
            if (theme.IsNullOrEmpty())
            {
                throw new ArgumentNullException();
            }
            return await _unitOfWork.UserInterfaceTheme
                .FindBy(x=>x.Theme==theme)
                .FirstOrDefaultAsync() is not null;
        }

        public async Task<String> GetThemeNameByIdAsync(Int32 id)
        {
            if (id<=0)
            {
                throw new ArgumentException("Invalid Id");
            }

            var theme = (await _unitOfWork.UserInterfaceTheme.GetByIdAsync(id));

            return theme?.Theme?? (await _unitOfWork.UserInterfaceTheme.GetByIdAsync(
                await GetIdDefaultThemeAsync()))!.Theme;
        }


    }
}
