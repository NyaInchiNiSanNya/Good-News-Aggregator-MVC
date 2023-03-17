using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Entities_Context.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositores;

namespace Services.Account
{
    public sealed class IdentityService: IIdentityService
    {
    
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityService( UserManager<User> userManager
            , SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Boolean> isUserExist(string email)
        {
            if (await _userManager.FindByEmailAsync(email) is not null)
            {
                return true;
            }

            return false;
        }

        public async Task<Boolean> Registration(String Name, String email, String password)
        {
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
        
        
        public async Task IdLogout()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
