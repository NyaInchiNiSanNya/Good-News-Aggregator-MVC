using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserConfigRepositores
{
    public interface GetSetUserConfigRepositore
    {
        public Task SetUserConfig(Dictionary<String, String> UserConfig);

        public Task<Dictionary<String, String>> GetUserConfig(String Email);
    }
}
