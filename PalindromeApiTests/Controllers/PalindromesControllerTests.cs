using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PalindromeApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;

namespace PalindromeApi.Controllers.Tests
{
    [TestClass()]
    public class PalindromesControllerTests : IDisposable
    {
        PalindromeFactory palindromeFactory;
        PalindromeContext context;
        PalindromesController controller;

        public PalindromesControllerTests()
        {
            var options = new DbContextOptionsBuilder<PalindromeContext>()
          .UseInMemoryDatabase(databaseName: "Palindromes")
          .Options;
            palindromeFactory = new PalindromeFactory();
            context = new PalindromeContext(options);
            controller = new PalindromesController(context, palindromeFactory);
        }

        public void Dispose()
        {
            controller = null;
            context = null;
            palindromeFactory = null;
        }

        [TestMethod()]
        public void PostPalindrome_PostiveTest()
        {
            
            string input = "aabbcccbbaa";
            var expectedPalindrome = palindromeFactory.GetPalindrome(input, DateTime.UtcNow);
            var response = controller.PostPalindrome(input);

            Assert.IsNotNull(response, "Null response.");
            Assert.IsNotNull(response.Result, "Response.Result is null.");

            var palindromeJson = response.Result.Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(palindromeJson), "Response.Result.Content is empty. ");

            var actualPalindrome = JsonConvert.DeserializeObject<Palindrome>(palindromeJson);
            Assert.IsTrue(actualPalindrome.IsPalindrome, "IsPalindrome had unexpected value: " + actualPalindrome.IsPalindrome);
            Assert.AreEqual(expectedPalindrome.IsPalindrome, actualPalindrome.IsPalindrome, $"Actual IsPalindrome value: {actualPalindrome.IsPalindrome}. Expected value: {expectedPalindrome.IsPalindrome}");


            var expectedMinusActual = expectedPalindrome.SortedCharacterCount.Except(actualPalindrome.SortedCharacterCount, new CharacterCountEqualityComparer());
            if (expectedMinusActual.Any())
            {
                StringBuilder sb = new StringBuilder();
                foreach(var cc in expectedMinusActual)
                {
                    sb.AppendLine($"{cc.character}:{cc.count}");
                }

                Assert.Fail("Actual SortedCharacterCount was missing expected items:" + Environment.NewLine + sb.ToString());
            }
            

            var actualMinusExpected = actualPalindrome.SortedCharacterCount.Except(expectedPalindrome.SortedCharacterCount, new CharacterCountEqualityComparer());
            if(actualMinusExpected.Any())
            {
                StringBuilder sb = new StringBuilder();
                foreach (var cc in actualMinusExpected)
                {
                    sb.AppendLine($"{cc.character}:{cc.count}");
                }

                Assert.Fail("Actual SortedCharacterCount contained unexpected items: " + Environment.NewLine + sb.ToString());
            }
        }

        [TestMethod()]
        public void PostPalindrome_NegativeTest()
        {

            string input = "abcdeFGhiJKlMNOpqrstuvwxyz";
            var expectedPalindrome = palindromeFactory.GetPalindrome(input, DateTime.UtcNow);
            var response = controller.PostPalindrome(input);

            Assert.IsNotNull(response, "Null response.");
            Assert.IsNotNull(response.Result, "Response.Result is null.");

            var palindromeJson = response.Result.Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(palindromeJson), "Response.Result.Content is empty. ");

            var actualPalindrome = JsonConvert.DeserializeObject<Palindrome>(palindromeJson);
            Assert.AreEqual(expectedPalindrome.IsPalindrome, actualPalindrome.IsPalindrome, $"Actual IsPalindrome value: {actualPalindrome.IsPalindrome}. Expected value: {expectedPalindrome.IsPalindrome}");


            var expectedMinusActual = expectedPalindrome.SortedCharacterCount.Except(actualPalindrome.SortedCharacterCount, new CharacterCountEqualityComparer());
            if (expectedMinusActual.Any())
            {
                StringBuilder sb = new StringBuilder();
                foreach (var cc in expectedMinusActual)
                {
                    sb.AppendLine($"{cc.character}:{cc.count}");
                }

                Assert.Fail("Actual SortedCharacterCount was missing expected items:" + Environment.NewLine + sb.ToString());
            }


            var actualMinusExpected = actualPalindrome.SortedCharacterCount.Except(expectedPalindrome.SortedCharacterCount, new CharacterCountEqualityComparer());
            if (actualMinusExpected.Any())
            {
                StringBuilder sb = new StringBuilder();
                foreach (var cc in actualMinusExpected)
                {
                    sb.AppendLine($"{cc.character}:{cc.count}");
                }

                Assert.Fail("Actual SortedCharacterCount contained unexpected items: " + Environment.NewLine + sb.ToString());
            }
        }
    }

    class CharacterCountEqualityComparer : IEqualityComparer<CharacterCount>
    {
        public bool Equals(CharacterCount cc1, CharacterCount cc2)
        {
            if (cc2 == null && cc1 == null)
                return true;
            else if (cc1 == null || cc2 == null)
                return false;
            return cc1.character.Equals(cc2.character) && cc1.count.Equals(cc2.count);
        }

        public int GetHashCode(CharacterCount cc)
        {
            return cc.character.GetHashCode() ^ cc.count;
        }
    }
}