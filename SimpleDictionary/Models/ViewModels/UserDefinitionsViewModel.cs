using SimpleDictionary.Models.DataModels;
using System.Collections.Generic;

namespace SimpleDictionary.Models.ViewModels
{
    public class UserDefinitionsViewModel
    {
        public User Author { get; set; }
        public List<Definition> Definitions { get; set; }
    }
}
