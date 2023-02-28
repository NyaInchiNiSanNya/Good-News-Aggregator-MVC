using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities_Context.Entities.UserNews;
using Microsoft.EntityFrameworkCore;
using Project.Domain.DBContext;

namespace Services.Account
{
    public sealed class UserConfigService:UserConfigRepositores.GetSetUserConfigRepositore
    {
        private readonly UserNewsContext ConfigContext;
        
        public UserConfigService(UserNewsContext UserConfig)
        {
            ConfigContext = UserConfig;
        }

        public async Task SetUserConfig(Dictionary<String,String> UserConfigDict)
        {
            await ConfigContext.Users.AddAsync(new UserConfig()
            {
                Name = UserConfigDict["Name"],
                Email = UserConfigDict["Email"],
                Config = UserConfigDict["Config"]

            });

            await ConfigContext.SaveChangesAsync();

            
        }

        public async Task<Dictionary<String,String>> GetUserConfig(String Email)
        {
            UserConfig User = await ConfigContext.Users.Where(x => Email == Email).AsNoTracking().FirstOrDefaultAsync();

            return new Dictionary<String, String>
            {
                { "Name", User.Name },
                { "Config", User.Config }
            };
        }
    }
}
