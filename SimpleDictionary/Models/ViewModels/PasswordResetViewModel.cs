using System.ComponentModel.DataAnnotations;

namespace SimpleDictionary.Models.ViewModels
{
    public class PasswordResetViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Enter your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your new password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public string Token { get; set; }
    }
}
