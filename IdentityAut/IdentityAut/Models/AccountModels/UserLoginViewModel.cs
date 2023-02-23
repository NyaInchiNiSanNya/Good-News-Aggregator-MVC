
using System;
using System.ComponentModel.DataAnnotations;

public class UserLoginViewModel
{
    [Required(ErrorMessage = "Введите почту")]
    [EmailAddress(ErrorMessage = "Неправильный логин и (или) пароль")]
    public String Email { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{6,16}$"
        , ErrorMessage = "Неправильный логин и (или) пароль")]
    public String Password { get; set; }
}