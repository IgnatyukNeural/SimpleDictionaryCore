using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleDictionary.Models.DataModels
{
    public class Definition
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public string UserId { get; set; }
        public User Author { get; set; }
        public string AuthorUsername { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Hashtag> Hashtags { get; set; }

    }
}
