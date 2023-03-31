using AutoMapper;
using Entities_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Account
{
    public class UiThemeService
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
    }
}
