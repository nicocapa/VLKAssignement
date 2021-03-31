using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLKAssignement.API.Models
{
    public class TransactionModel
    {        
        public Guid Id { get; private set; }
     
        public string DestinationAccountNumber { get; set; }

        public string DestinationCurrencyCode { get; set; }

        public decimal Amount { get; set; }

        public decimal UsedRate { get; set; }

        public DateTime UsedRateDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid UserId { get; set; }
    }
}
