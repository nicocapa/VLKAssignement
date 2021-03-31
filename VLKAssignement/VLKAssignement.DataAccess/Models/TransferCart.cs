using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VLKAssignement.DataAccess.Models
{
    [Table("TransfersCart")]
    public class TransferCart
    {
        public TransferCart()
        {
            Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; private set; }

        public virtual IList<Transfer> Transfers { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
