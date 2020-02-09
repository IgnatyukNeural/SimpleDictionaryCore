using System.ComponentModel.DataAnnotations;

namespace SimpleDictionary.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Remember { get; set; } = false;
    }
}
