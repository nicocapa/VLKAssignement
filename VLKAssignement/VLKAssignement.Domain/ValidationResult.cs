using System.Collections.Generic;

namespace VLKAssignement.Domain
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Messages = new List<string>();
        }
        public bool Succeded => Messages.Count == 0;

        public List<string> Messages { get; set; }
    }
}
