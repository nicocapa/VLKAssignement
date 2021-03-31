using VLKAssignement.DataAccess.Models;
using VLKAssignement.Domain;
using VLKAssignement.Service.Interfaces;

namespace VLKAssignement.Service
{
    public class TransferValidator : IValidator<Transfer>
    {
        private ValidationResult result = new ValidationResult();

        public ValidationResult Validate(Transfer transfer)
        {
            if(transfer.Status != Status.Pending)
            {
                result.Messages.Add($"You cannot sign this transfer because its status is: {transfer.Status} and it is not {Status.Pending}");
            }
            return result;
        }
    }
}
