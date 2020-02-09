using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleDictionary.Models.DataModels
{
    public class User : IdentityUser
    {
        public List<Definition> Definitions { get; set; }
        public List<LikedDefinition> LikedDefinitions { get; set; }
        public List<DislikedDefinition> DislikedDefinitions { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }
    }
}