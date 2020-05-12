using System.Text.RegularExpressions;
using System.Linq;

namespace PalindromeApi
{
    public static class ExtensionMethods
    {
        public static bool IsLettersOnly(this string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z]+$");
        }

        public static bool IsPalindrome(this string input)
        {
            return input.SequenceEqual(input.Reverse());
        }
    }
}
