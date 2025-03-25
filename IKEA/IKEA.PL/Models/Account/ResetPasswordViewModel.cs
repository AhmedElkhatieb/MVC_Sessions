using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Account
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords Didnt Match")]
        public string ConfirmPassword { get; set; } = null!;

    }
}
