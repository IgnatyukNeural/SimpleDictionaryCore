using System.ComponentModel.DataAnnotations;

namespace SimpleDictionary.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
