using Microsoft.EntityFrameworkCore;
using Project.Abstractions;
using Project.Domain.DBContext;

namespace Project.DBExchange 
{
    public class Authorization_Service : IUserRepository
    {
        private CharacterContext _dbcontext;

        
        
        //contex дб подставлен автоматически так как в program реализован и DI и DB context
        public Authorization_Service(CharacterContext _dbcontext)
        {
            this._dbcontext= _dbcontext;

        }



        public async Task<Boolean> Existence_Check(IUserRepository.UserRegistrationViewModel Email)
        {
            if (await _dbcontext.Users.Where(x => x.Email == Email.Email).FirstOrDefaultAsync() is not null)
            { 
                return false;
            }
            
            
            return true;
        }


        public async Task Add_New(IUserRepository.UserRegistrationViewModel model)
        {

            Domain.Entities.User.User New_User = new Domain.Entities.User.User() 
                    { Name = model.Name, Password = HashPassword.Get_Hash(model.Password), Email = model.Email };
                
            await _dbcontext.Users.AddAsync(New_User);

            //await не работает
            _dbcontext.SaveChanges();     
                
                
            
        }

        public async Task<Boolean> Authification_method(IUserRepository.UserLoginViewModel user)
        {
            var User=_dbcontext.Users.Where(x => x.Email == user.Email).FirstOrDefault();
            
            if (User is not null && HashPassword.Verify_Password(user.Password, User.Password))
            {
                return true;
            }

            return false;
            
        }
    }
}
