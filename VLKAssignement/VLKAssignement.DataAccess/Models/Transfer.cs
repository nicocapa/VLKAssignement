using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VLKAssignement.DataAccess.Models
{
    [Table("Transfers")]
    public class Transfer
    {
        public Transfer()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
        }
        [Key]
        public Guid Id { get; private set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required, MaxLength(34)]
        public string DestinationAccountNumber { get; set; }

        [Required]
        public string DestinationCurrencyCode { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public Guid CartId { get; set; }

        [ForeignKey("CartId")]
        public virtual TransferCart Cart { get; set; }
    }
}
