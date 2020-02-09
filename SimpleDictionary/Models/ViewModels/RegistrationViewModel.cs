using System.ComponentModel.DataAnnotations;

namespace SimpleDictionary.Models.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Repeat your password, please")]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat the password")]
        public string PasswordConfirm { get; set; }

    }
}
