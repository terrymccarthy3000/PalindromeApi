using Microsoft.EntityFrameworkCore;

namespace PalindromeApi.Models
{
    public class PalindromeContext : DbContext
    {
        public PalindromeContext(DbContextOptions<PalindromeContext> options) : base(options)
        {
        }

        public DbSet<Palindrome> Palindromes { get; set; }
    }
}
