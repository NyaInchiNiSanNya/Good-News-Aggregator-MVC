using Core.DTOs;
using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IUiThemeService
    {
        public Task<String> GetThemeNameByIdAsync(Int32 Id);

        public Task InitiateThemeAsync();

        public Task<Int32> GetIdThemeByStringAsync(string Theme);

        public Task<Int32> GetIdDefaultThemeAsync();

        public Task<Boolean> IsThemeExistByNameAsync(string theme);
        
        public Task<List<String>> GetAllThemesAsync();
    }
}
