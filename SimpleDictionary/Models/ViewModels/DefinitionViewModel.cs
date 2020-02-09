using System.ComponentModel.DataAnnotations;

namespace SimpleDictionary.Models.ViewModels
{
    public class DefinitionViewModel
    {
        [Required(ErrorMessage = "Can't be blank")]
        [Display(Name = "Word")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Can't be blank")]
        [Display(Name = "Type your definition here")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please, give an example of how it's used in a sentence")]
        [Display(Name = "Type an example")]
        [DataType(DataType.MultilineText)]
        public string Example { get; set; }

        [Display(Name = "Type a list of comma-separated hashtags, please")]
        public string Hashtags { get; set; }

    }
}
