using VLKAssignement.Domain;

namespace VLKAssignement.Service.Interfaces
{
    public interface IValidator<TDomain> where TDomain : class, new()
    {
        ValidationResult Validate(TDomain objectToValidate);
    }
}