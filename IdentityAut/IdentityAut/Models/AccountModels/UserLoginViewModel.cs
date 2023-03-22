
using System.ComponentModel.DataAnnotations;
using Business_Logic.ValidationRules;
using Microsoft.AspNetCore.Mvc;

public class UserLoginViewModel:ValidatePatterns,ValidateErrors
{
    [Required(ErrorMessage =ValidateErrors.NoEmail )]
    [EmailAddress(ErrorMessage = ValidateErrors.BadEmail)]
    [Remote ("CheckUserLoginExist", "Account"
        , ErrorMessage = ValidateErrors.BadTry)]
    public String Email { get; set; }

    [Required(ErrorMessage = ValidateErrors.NoPassword)]
    [DataType(DataType.Password)]
    [RegularExpression(ValidatePatterns.PasswordPattern
        , ErrorMessage = ValidateErrors.BadPassword)]
    public String Password { get; set; }

}