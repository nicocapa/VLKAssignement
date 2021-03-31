using System;
using System.Threading.Tasks;
using VLKAssignement.DataAccess.Models;

namespace VLKAssignement.Service.Interfaces
{
    public interface IExchangeRateService
    {
        Task<CachedExchangeRate> GetExchangeRate(DateTime rateDate, string fromCurrency, string toCurrency);
    }
}