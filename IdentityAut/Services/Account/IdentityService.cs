using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities_Context.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Repositores;

namespace Services.Account
{
    public sealed class IdentityService: IdentityRepository
    {
    
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityService( UserManager<User> userManager
            , SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public async Task<Boolean> Registration( String email, String password)
        {
            User user = new User
            {
                UserName = email,
                Email = email,
            };

            var result = await _userManager.CreateAsync(new User
            {
                UserName = email,
                Email = email,
            }, password);
            
            return result.Succeeded;
            
        }

        public async Task<Boolean> Login(String Email, String Password)
        {
            var result =
                await _signInManager.PasswordSignInAsync
                    (Email, Password, false, false);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(new User()
                {
                    UserName = Email,
                }, isPersistent: false);
            }
            return result.Succeeded;
        }
    }
}
