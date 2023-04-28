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

namespace Services.Account
{

    public class UiThemeService:IUiThemeService
    {
        private const String DefaultTheme = "default";
        private const String DarkTheme = "dark";

        private readonly IUnitOfWork _unitOfWork;


        public UiThemeService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            _unitOfWork = unitOfWork;

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

            Boolean AnyChanges = false;

            if (!await IsThemeExistByNameAsync(DefaultTheme))
            {
                await _unitOfWork.UserInterfaceTheme.AddAsync(new SiteTheme() { Theme = DefaultTheme });

                AnyChanges = true;
            }

            if (!await IsThemeExistByNameAsync(DarkTheme))
            {
                await _unitOfWork.UserInterfaceTheme.AddAsync(new SiteTheme() { Theme = DarkTheme });

                AnyChanges = true;
            }

            if (AnyChanges)
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<Int32> GetIdDefaultThemeAsync()
        {
            var theme = await _unitOfWork.UserInterfaceTheme
                .FindBy(x => x.Theme == DefaultTheme)
                .FirstOrDefaultAsync();
            
            Int32 Id = theme?.Id ?? 0;

            if (Id == 0)
            {
                await InitiateThemeAsync();

                theme = await _unitOfWork.UserInterfaceTheme
                    .FindBy(x => x.Theme == DefaultTheme)
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
            
            return theme?.Theme?? DefaultTheme;
        }


    }
}
