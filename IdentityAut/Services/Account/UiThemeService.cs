using AutoMapper;
using Entities_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IServices;
using Entities_Context.Entities.UserNews;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Abstract;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Services.Account
{

    public class UiThemeService:IUiThemeService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;


        public UiThemeService(IUnitOfWork unitOfWork, IConfiguration configuration)
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


        public async Task<Int32> GetIdThemeByStringAsync(string Theme)
        {
            if (await IsThemeExistByNameAsync(Theme))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return (await _unitOfWork.UserInterfaceTheme.FindBy(x=>x.Theme.Equals(Theme))
                    .FirstOrDefaultAsync()).Id;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            return await GetIdDefaultThemeAsync();
        }



        public async Task InitiateThemeAsync()
        {
            Log.Information("Attempt to create themes");

            String[] themesFromConfigFile = _configuration["Themes:all"].Split(" ");
           
            Boolean AnyChanges = false;

            if (themesFromConfigFile.Length==0)
            {
                throw new ArgumentException("No themes are defined in the configuration file");
            }

            foreach (var theme in themesFromConfigFile)
            {
                if (!await IsThemeExistByNameAsync(theme))
                {
                    await _unitOfWork.UserInterfaceTheme.AddAsync(new SiteTheme() { Theme = theme });

                    AnyChanges = true;
                }
            }

            if (AnyChanges)
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<Int32> GetIdDefaultThemeAsync()
        {
            String defaultTheme = _configuration["Themes:default"];

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

                foreach (var theme in themeList)
                {
                    allThemes.Add(theme.Theme);
                }
            }

            return allThemes;
        }

        public async Task<Boolean> IsThemeExistByNameAsync(String theme)
        {
            return await _unitOfWork.UserInterfaceTheme
                .FindBy(x=>x.Theme==theme)
                .FirstOrDefaultAsync() is not null;
        }

        public async Task<String> GetThemeNameByIdAsync(Int32 Id)
        {
            var theme = (await _unitOfWork.UserInterfaceTheme.GetByIdAsync(Id));

            return theme?.Theme?? (await _unitOfWork.UserInterfaceTheme.GetByIdAsync(
                await GetIdDefaultThemeAsync())).Theme;
        }


    }
}
