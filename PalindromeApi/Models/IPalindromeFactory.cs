using System;
using System.Collections.Generic;

namespace PalindromeApi.Models
{
    public interface IPalindromeFactory
    {
        Palindrome GetPalindrome(string input, DateTime createdDateTime, int id = 0);
        bool IsPalindrome(string input);
        IEnumerable<CharacterCount> GetCharacterCounts(string input);
    }
}