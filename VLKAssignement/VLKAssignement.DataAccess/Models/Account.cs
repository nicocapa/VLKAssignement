using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VLKAssignement.DataAccess.Models
{
    [Table("Accounts")]
    public class Account
    {
        public Account()
        {
            Id = Guid.NewGuid();            
        }
        [Key]
        public Guid Id { get; private set; }

        [Required, MaxLength(34)]
        public string IBAN { get; set; }

        [Required, MaxLength(3)]
        public string CurrencyCode { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User Owner { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}