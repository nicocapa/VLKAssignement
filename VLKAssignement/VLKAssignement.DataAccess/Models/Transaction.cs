using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VLKAssignement.DataAccess.Models
{
    public class Transaction
    {
        public Transaction()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
        }
        [Key]
        public Guid Id { get; private set; }

        [Required, MaxLength(34)]
        public string DestinationAccountNumber { get; set; }

        [Required]
        public string DestinationCurrencyCode { get; set; }

        [Required]
        public decimal Amount { get; set; }        

        public decimal UsedRate { get; set; }

        public bool IsBaseCurrencySameAsTo { get; set; }

        public DateTime UsedRateDate { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
