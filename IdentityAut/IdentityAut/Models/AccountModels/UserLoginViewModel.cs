
using System.ComponentModel.DataAnnotations;
using Business_Logic.ValidationRules;

public class UserLoginViewModel:ValidatePatterns,ValidateErrors
{
    [Required(ErrorMessage =ValidateErrors.NoEmail )]
    [EmailAddress(ErrorMessage = ValidateErrors.BadEmail)]
    public String Email { get; set; }

    [Required(ErrorMessage = ValidateErrors.NoPassword)]
    [DataType(DataType.Password)]
    [RegularExpression(ValidatePatterns.PasswordPattern
        , ErrorMessage = ValidateErrors.BadPassword)]
    public String Password { get; set; }

}