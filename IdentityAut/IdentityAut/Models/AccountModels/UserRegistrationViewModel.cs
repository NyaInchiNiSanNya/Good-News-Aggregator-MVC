using System;
using System.ComponentModel.DataAnnotations;

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