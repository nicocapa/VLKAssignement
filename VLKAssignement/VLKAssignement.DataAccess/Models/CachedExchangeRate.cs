using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VLKAssignement.DataAccess.Models
{
    [Table("CachedExchangeRates"), Index(nameof(CurrencyCodeFrom),nameof(CurrencyCodeTo), nameof(RateDate),IsUnique =true, Name ="IX_From_To_RateDate")]
    public class CachedExchangeRate
    {
        public CachedExchangeRate()
        {
            Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; private set; }

        [Required, MaxLength(3)]
        public string CurrencyCodeFrom { get; set; }

        [Required, MaxLength(3)]
        public string CurrencyCodeTo { get; set; }

        [Required]
        public decimal Rate { get; set; }

        [Required]
        public DateTime RateDate { get; set; }

        [NotMapped]
        public bool IsBaseCurrencySameAsTo { get; set; }
    }
}
