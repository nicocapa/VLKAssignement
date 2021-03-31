using System;
using System.Collections.Generic;

namespace VLKAssignement.DataAccess.Repositories.Interfaces
{
    public interface ICachedExchangeRateRepository : IRepositoryBase<Models.CachedExchangeRate>    
    {
        List<Models.CachedExchangeRate> GetRate(DateTime date, string fromCurrency, string toCurrency);
        void SaveRates(List<Models.CachedExchangeRate> rates);
    }
}