using System.Collections.Generic;

namespace SimpleDictionary.Services
{
    public interface IHashtagParser<T>
    {
        List<T> Parse(string text, string separator = ",");
    }
}
