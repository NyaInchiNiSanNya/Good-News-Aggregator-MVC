using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using Business_Logic.ValidationRules;

public class UserRegistrationViewModel: 
    ValidatePatterns
    ,ValidateErrors
{

    [Required(ErrorMessage = ValidateErrors.BadName)]
    [RegularExpression(ValidatePatterns.NamePattern
        , ErrorMessage =ValidateErrors.BadName)]
    public String Name { get; set; }

    [Required(ErrorMessage = ValidateErrors.NoEmail)]
    [EmailAddress(ErrorMessage = ValidateErrors.BadEmail)]
    [Remote( "CheckUserExist", "Account",
        ErrorMessage = ValidateErrors.EmailExist)]
    public String Email { get; set; }

    [Required(ErrorMessage = ValidateErrors.NoPassword)]
    [DataType(DataType.Password)]
    [RegularExpression(ValidatePatterns.PasswordPattern
        , ErrorMessage = ValidateErrors.BadPassword)]
    public String Password { get; set; }

    [Required(ErrorMessage = ValidateErrors.BadConfirm)]
    [DataType(DataType.Password)]
    [Compare("Password"
        , ErrorMessage = ValidateErrors.BadConfirm)]
    public String ConfirmPassword { get; set; }

}