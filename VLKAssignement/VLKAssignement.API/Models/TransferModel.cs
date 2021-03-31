using System;
using System.ComponentModel.DataAnnotations;

namespace VLKAssignement.API.Models
{
    public class TransferModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please specify the user who wants to do the transfer")]
        public string UserId { get; set; }        

        [Required(ErrorMessage = "Please specify the account number (IBAN) that you want to use as destination")          
         ,RegularExpression(@"^[A-Z]{2}\d{2}[A-Z]{4}\d{10}$", ErrorMessage = "The format of the destination account is incorrect, do not use spaces and use capital letters")
         ]
        public string DestinationAccountNumber { get; set; }

        [Required(ErrorMessage = "Please specify the currency code for the destination account"), 
         MaxLength(3, ErrorMessage = "The currency code must have 3 characters and follow the ISO 4217 format"),
         MinLength(3, ErrorMessage = "The currency code must have 3 characters and follow the ISO 4217 format")]
        public string DestinationCurrencyCode { get; set; }

        [Required(ErrorMessage = "Please specify the amount that you want to transfer"), DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public string Status { get; set; }


        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
