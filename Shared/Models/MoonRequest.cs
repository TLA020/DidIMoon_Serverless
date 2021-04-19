using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class MoonRequest
    {
        [Required]
        public CryptoCurrency Coin { get; set; }

        [Range(1, 100000, ErrorMessage = "Amount invalid (1-100000).")]
        public int Amount { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
