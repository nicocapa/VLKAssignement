using System;

namespace VLKAssignement.Domain
{
    public class SignResult
    {
        public SignResult()
        {
            ValidationResult = new ValidationResult();
        }

        public Guid TransactionId { get; set; }

        public ValidationResult ValidationResult { get; set; }
    }
}
