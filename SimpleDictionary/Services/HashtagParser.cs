using Microsoft.Extensions.Logging;
using SimpleDictionary.Models.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleDictionary.Services
{
    public class HashtagParser : IHashtagParser<Hashtag>
    { 
        private string RemoveWhitespaces(string text)
        {
            return Regex.Replace(text, @"\s+", "");
        }

        /// <summary>
        /// Returns a list of hashtags from a string
        /// </summary>
        /// <param name="text">A string with hashtags</param>
        /// <param name="separator">Symbol to be used as separator</param>
        /// <returns>List</returns>
        public List<Hashtag> Parse(string text, string separator = ",")
        {
            var regex = RemoveWhitespaces(text);
            string[] separatedArray = regex.Split(separator);

            var list = separatedArray.Where(x => x != "" && x.StartsWith("#"));

            var hashtagList = new List<Hashtag>();

            foreach(var hashtag in list)
            {
                hashtagList.Add(new Hashtag { Name = hashtag });
            }

            return hashtagList;
        }
    }
}
