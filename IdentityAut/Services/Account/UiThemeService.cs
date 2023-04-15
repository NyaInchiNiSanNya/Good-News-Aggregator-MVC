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

namespace Services.Account
{
    public class UiThemeService:IUiThemeService
    {
        private readonly UserArticleContext _userContext;


        public UiThemeService(UserArticleContext userContext
            , IMapper mapper)
        {
            if (userContext is null)
            {
                throw new ArgumentNullException(nameof(userContext));
            }

            _userContext = userContext;

        }


        public async Task<Int32> GetIdThemeByStringAsync(string Theme)
        {
            if (await IsThemeExistByNameAsync(Theme))
            {
                return await _userContext.Themes
                    .AsNoTracking()
                    .Where(x => x.Theme.Equals(Theme))
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
            }

            return await GetIdDefaultThemeAsync();
        }



        public async Task InitiateThemeAsync()
        {

            Boolean AnyChanges = false;

            if (!await IsThemeExistByNameAsync("default"))
            {
                await _userContext.Themes.AddAsync(new SiteTheme() { Theme = "default" });

                AnyChanges = true;
            }
            if (!await IsThemeExistByNameAsync("dark"))
            {
                await _userContext.Themes.AddAsync(new SiteTheme() { Theme = "dark" });

                AnyChanges = true;
            }

            if (AnyChanges)
            {
                await _userContext.SaveChangesAsync();
            }
        }

        public async Task<Int32> GetIdDefaultThemeAsync()
        {
            Int32 Id = await _userContext.Themes
                .AsNoTracking()
                .Where(x => x.Theme == "default")
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (Id == 0)
            {
                await InitiateThemeAsync();

                return await _userContext.Themes
                    .AsNoTracking()
                    .Where(x => x.Theme == "default")
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
            }

            return Id;
        }

        public async Task<List<String>> GetAllThemesAsync()
        {
            List<String> ThemeList = await _userContext.Themes
                .AsNoTracking()
                .Select(x => x.Theme)
                .ToListAsync();

            if (ThemeList is null)
            {
                await InitiateThemeAsync();

                return await _userContext.Themes
                    .AsNoTracking()
                    .Select(x => x.Theme)
                    .ToListAsync();
            }

            return ThemeList;
        }

        public async Task<Boolean> IsThemeExistByNameAsync(string Theme)
        {
            return await _userContext.Themes.AnyAsync(x => x.Theme==Theme);
        }

        public async Task<String> GetThemeNameByIdAsync(Int32 Id)
        {
            String Theme = await _userContext.Themes
                .AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => x.Theme)
                .FirstOrDefaultAsync();

            if (!Theme.IsNullOrEmpty())
            {
                return Theme;
            }

            return "default";
        }


    }
}
