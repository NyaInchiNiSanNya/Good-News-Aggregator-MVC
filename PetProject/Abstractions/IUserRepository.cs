using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.Identity.Client;

namespace Project.Abstractions
{

    public interface IUserRepository
    {

        public class UserRegistrationViewModel
        {

            [Required(ErrorMessage = "Введите имя")]
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Некорректное имя")]
            public String Name { get; set; }
            
            [Required(ErrorMessage = "Введите почту")]
            [EmailAddress(ErrorMessage = "Некорректный электронный адрес")]
            public String Email { get; set; }
            
            [Required(ErrorMessage = "Введите пароль")]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{6,16}$"
                , ErrorMessage = "Ваш пароль должен содержать символы верхнего и нижнего регистров" +
                                 ", а так же цифры и спец. символ.")]
            public String Password { get; set; }

            [Required(ErrorMessage = "Подтвердите пароль")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
            public String ConfirmPassword { get; set; }

        }

        public class UserLoginViewModel
        {
            [Required(ErrorMessage = "Введите почту")]
            [EmailAddress(ErrorMessage = "Неверный электронный адрес или пароль.")]
            public String Email { get; set; }
            
            [Required(ErrorMessage = "Введите пароль")]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{6,16}$"
                , ErrorMessage = "Неверный электронный адрес или пароль.")]
            public String Password { get; set; }
        }


        public Task Add_New(UserRegistrationViewModel model);
        public Task<Boolean> Existence_Check(UserRegistrationViewModel User);
        public Task<Boolean> Authification_method(UserLoginViewModel User);

    }
}
