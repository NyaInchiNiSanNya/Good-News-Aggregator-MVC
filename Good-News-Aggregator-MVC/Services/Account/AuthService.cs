using AutoMapper;
using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;
using IServices;
using IServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Services.Account
{
    public sealed class AuthService : IAuthService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IUiThemeService _uiTheme;

        private readonly IRoleService _role;

        public AuthService(IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IUiThemeService uiTheme, 
            IRoleService role)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _uiTheme = uiTheme ?? throw new ArgumentNullException(nameof(uiTheme));

            _role = role ?? throw new ArgumentNullException(nameof(role));
        }

        public async Task<Boolean> IsUserExistAsync(String email)
        {
            if (email.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await _unitOfWork.Users
                .FindBy(x => x.Email.Equals(email))
                .FirstOrDefaultAsync() is not null;
        }


        public async Task<Boolean> RegistrationAsync(UserRegistrationDto modelDto)
        {

            if(!await IsUserExistAsync(modelDto.Email)){

                User newUser = _mapper.Map<User>(modelDto);

                newUser.ThemeId =await _uiTheme.GetIdDefaultThemeAsync();
                
                newUser.Password = MakeHash(modelDto.Password);

                newUser.Created=DateTime.Now;

                PictureBase64EncoderDecoder encoder=new PictureBase64EncoderDecoder();
                newUser.ProfilePicture = await 
                    encoder.PictureEncoder(@"C:\\Users\\User\\Desktop\\ASP-Project\\ASProject\\IdentityAut\\IdentityAut\\wwwroot\\images\\defaultImage3.jpg");

                await _unitOfWork.Users.AddAsync(newUser);


                await _unitOfWork.SaveChangesAsync();

                var role = await _role.GetDefaultRoleAsync();


                if (role is null)
                {
                    await _unitOfWork.Users.Remove(
                        ((await _unitOfWork.Users
                            .FindBy(x => x.Email.Equals(modelDto.Email))
                            .FirstOrDefaultAsync())!).Id);


                    throw new ArgumentException("Registration failed.Can't create role");
                }

                var newUserRole=new UsersRoles()
                {
                    RoleId =role.Id,

                    UserId= ((await _unitOfWork.Users.FindBy(x=>x.Email.Equals(modelDto.Email)).FirstOrDefaultAsync())!).Id
                };

                await _unitOfWork.UsersRoles.AddAsync(newUserRole);
                
                await _unitOfWork.SaveChangesAsync();

                Log.Information("User {0} successfully registered.", modelDto.Email);

                return true;
            }

            return false;
        }

        #region PasswordHash
        static String MakeHash(String Password)
        {
            return BCrypt.Net.BCrypt.HashPassword(Password);
        }

        static Boolean CheckPassword(String Password,String PasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(Password,PasswordHash);
        }
        #endregion

        public async Task<Boolean> LoginAsync(UserLoginDto modelDto)
        {

            User? сheckUser = _unitOfWork.Users.FindBy(x => x.Email.Equals(modelDto.Email)).FirstOrDefault();

            if (сheckUser is not null
                && CheckPassword(modelDto.Password, сheckUser.Password))
            {
                Log.Information("User {0} successfully login.", modelDto.Email);

                return true;
            }

            return false;

        }

    }
}
