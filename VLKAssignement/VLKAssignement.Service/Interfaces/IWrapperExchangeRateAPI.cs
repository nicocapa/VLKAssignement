using System.Threading.Tasks;

namespace VLKAssignement.Service.Interfaces
{
    public interface IWrapperExchangeRateAPI
    {
        Task<ExchangeRateResult> GetLatestRates();
    }
}
