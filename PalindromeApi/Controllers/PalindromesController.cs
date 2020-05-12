using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PalindromeApi.Models;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace PalindromeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalindromesController : ControllerBase
    {
        private readonly PalindromeContext _context;
        private readonly IPalindromeFactory _palindromeFactory;

        public PalindromesController(PalindromeContext context, IPalindromeFactory palindromeFactory)
        {
            _context = context;
            _palindromeFactory = palindromeFactory;
        }

        // GET: api/Palindromes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Palindrome>>> GetPalindromes()
        {
            return await _context.Palindromes.ToListAsync();
        }

        // GET: api/Palindromes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Palindrome>> GetPalindrome(int id)
        {
            var palindrome = await _context.Palindromes.FindAsync(id);

            if (palindrome == null)
            {
                return NotFound();
            }

            return palindrome;
        }

        // PUT: api/Palindromes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPalindrome(int id, string input)
        {
            var dateTime = DateTime.UtcNow;
            var palindrome = _palindromeFactory.GetPalindrome(input, dateTime, id);

            _context.Entry(palindrome).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PalindromeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Palindromes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ContentResult> PostPalindrome(string input)
        {
            var dateTime = DateTime.UtcNow;
            var palindrome = _palindromeFactory.GetPalindrome(input, dateTime);
            var jsonResult = JsonConvert.SerializeObject(palindrome);
            _context.Palindromes.Add(palindrome);
            await _context.SaveChangesAsync();

            return Content(jsonResult, "application/json");
        }

        // DELETE: api/Palindromes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Palindrome>> DeletePalindrome(int id)
        {
            var palindrome = await _context.Palindromes.FindAsync(id);
            if (palindrome == null)
            {
                return NotFound();
            }

            _context.Palindromes.Remove(palindrome);
            await _context.SaveChangesAsync();

            return palindrome;
        }

        private bool PalindromeExists(int id)
        {
            return _context.Palindromes.Any(e => e.Id == id);
        }
    }
}
