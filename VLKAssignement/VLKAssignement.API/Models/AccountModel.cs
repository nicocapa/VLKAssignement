using System;
using System.ComponentModel.DataAnnotations;

namespace VLKAssignement.API.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; }

        [Required]
        public string IBAN { get; set; }

        [Required]
        public string CurrencyCode { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}