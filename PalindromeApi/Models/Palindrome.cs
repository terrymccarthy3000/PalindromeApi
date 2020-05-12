using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PalindromeApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Palindrome
    {
        public int Id { get; set; }

        public string Value { get; set; }

        [JsonProperty]
        public bool IsPalindrome { get; set; }

        [NotMapped, JsonProperty]
        public IEnumerable<CharacterCount> SortedCharacterCount { get; set; }

        [JsonProperty]
        public string TimeStamp { get; set; }


    }

    [JsonObject(MemberSerialization.OptIn)]
    public class CharacterCount 
    {
        public int Id { get; set; }
        [JsonProperty]
        public string character { get; set; }
        [JsonProperty]
        public int count { get; set; }
    }

    public class PalindromeFactory : IPalindromeFactory 
    {
        public Palindrome GetPalindrome(string input, DateTime createdDateTime, int id = 0)
        {
            if (!input.IsLettersOnly())
            {
                throw new ArgumentException("Input string contains non-alphabetic characters: " + input);
            }
            var lowerCaseInput = input.ToLower();
            var isPalindrome = lowerCaseInput.IsPalindrome();
            var characterCount = GetCharacterCounts(lowerCaseInput);

            var palindrome = new Palindrome
            {
                Value = lowerCaseInput,
                IsPalindrome = isPalindrome,
                SortedCharacterCount = characterCount,
                TimeStamp = createdDateTime.ToString("yyyy-MM-dd HH:mm") + " UTC"
            };

            if(id != 0)
            {
                palindrome.Id = id;
            }

            return palindrome;
        }

        public bool IsPalindrome(string input)
        {
            return input.SequenceEqual(input.Reverse());
        }

        public IEnumerable<CharacterCount> GetCharacterCounts(string input)
        {
            var sortedString = string.Concat(input.OrderBy(c => c));
            var groupedByLetter = sortedString.GroupBy(letter => letter);
            var characterCounts = groupedByLetter
                     .Select(alphabet => new CharacterCount
                     {
                         character = alphabet.Key.ToString(),
                         count = alphabet.Count()
                     });

            return characterCounts;
        }

    }
}

