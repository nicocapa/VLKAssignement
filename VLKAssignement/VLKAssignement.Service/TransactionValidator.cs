using VLKAssignement.Domain;
using VLKAssignement.DataAccess.Models;
using VLKAssignement.Service.Interfaces;

namespace VLKAssignement.Service
{
    public class TransactionValidator : IValidator<Transaction>
    {
        private readonly IAccountService _accountService;        

        public TransactionValidator(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public ValidationResult Validate(Transaction transaction)
        {
            var validationResult = new ValidationResult();
            ValidateBalance(transaction, validationResult);
            return validationResult;
        }

        private void ValidateBalance(Transaction transaction, ValidationResult validationResult)
        {
            var userAccount = _accountService.GetByUserId(transaction.UserId);
            var convertedAmount = ConvertedAmount(transaction);
            if (userAccount.Balance < convertedAmount)
            {
                validationResult.Messages.Add("You don't have enough capital to proceed with this transfer.");
            }
        }

        private decimal ConvertedAmount(Transaction transaction)
        {
            if (transaction.IsBaseCurrencySameAsTo)
            {
                return transaction.Amount / transaction.UsedRate;
            }
            else
            {
                return transaction.Amount * transaction.UsedRate;
            }
        }
    }
}
