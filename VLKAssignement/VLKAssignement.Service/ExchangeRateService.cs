using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VLKAssignement.DataAccess.Repositories.Interfaces;
using VLKAssignement.DataAccess.Models;
using VLKAssignement.Service.Interfaces;
using System.Linq;

namespace VLKAssignement.Service
{
    public class ExchangeRateService:IExchangeRateService
    {
        private readonly IWrapperExchangeRateAPI _wrapperExchangeRateAPI;
        private readonly ICachedExchangeRateRepository _cachedExchangeRateRepository;

        public ExchangeRateService(IWrapperExchangeRateAPI wrapperExchangeRateAPI, ICachedExchangeRateRepository cachedExchangeRateRepository)
        {
            _wrapperExchangeRateAPI = wrapperExchangeRateAPI;
            _cachedExchangeRateRepository = cachedExchangeRateRepository;
        }

        /// <summary>
        /// Get rate to convert the amount to transfer to the account's currency
        /// </summary>
        /// <param name="rateDate">Date of the rate</param>
        /// <param name="fromCurrency">Currency of the destination</param>
        /// <param name="toCurrency">Currency of the user's account</param>
        /// <returns></returns>
        public async Task<CachedExchangeRate> GetExchangeRate(DateTime rateDate, string fromCurrency, string toCurrency)
        {
            ExchangeRateResult exchangeRateResult = new ExchangeRateResult(); 

            if (fromCurrency.Equals(toCurrency,StringComparison.InvariantCultureIgnoreCase))
            {
                return new CachedExchangeRate
                {
                    RateDate = DateTime.Today,
                    CurrencyCodeFrom = fromCurrency,
                    CurrencyCodeTo = toCurrency,
                    Rate = 1
                };
            }

            if (rateDate < DateTime.Today.AddDays(-2))
            {
                return null;
            }

            var existingRates = _cachedExchangeRateRepository.GetRate(rateDate, fromCurrency, toCurrency);

            if (existingRates.Count == 0)
            {
                if (DateTime.Today == rateDate)
                {
                    exchangeRateResult = await _wrapperExchangeRateAPI.GetLatestRates();
                    if (exchangeRateResult == null)
                    {
                        return await GetExchangeRate(rateDate.AddDays(-1), fromCurrency, toCurrency);
                    }
                    SaveRates(exchangeRateResult);
                }
                else
                {
                    return await GetExchangeRate(rateDate.AddDays(-1), fromCurrency, toCurrency);
                }
            }
            else
            {
                exchangeRateResult = ConvertToExchangeResult(existingRates);
            }
            return ExtractRate(fromCurrency, toCurrency, exchangeRateResult);
        }

        private ExchangeRateResult ConvertToExchangeResult(List<DataAccess.Models.CachedExchangeRate> existingRates)
        {
            var rates = new ExchangeRateResult();
            var firstExistingRate = existingRates.First();
            rates.Base = firstExistingRate.CurrencyCodeFrom;
            rates.Date = firstExistingRate.RateDate;
            rates.Rates = new Dictionary<string, decimal>();
            foreach (var rate in existingRates)
            {
                rates.Rates.Add(rate.CurrencyCodeTo, rate.Rate);
            }
            return rates;
        }

        private CachedExchangeRate ExtractRate(string fromCurrency, string toCurrency, ExchangeRateResult rates)
        {
            fromCurrency = fromCurrency.ToUpper();
            toCurrency = toCurrency.ToUpper();
            var result = new CachedExchangeRate
            {
                CurrencyCodeFrom = fromCurrency,
                CurrencyCodeTo = toCurrency,
                RateDate = rates.Date
            };
            if (toCurrency == rates.Base)
            {
                if (rates.Rates.ContainsKey(fromCurrency))
                {
                    result.Rate = rates.Rates[fromCurrency];
                    result.IsBaseCurrencySameAsTo = true;                    
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (rates.Rates.ContainsKey(fromCurrency) && rates.Rates.ContainsKey(toCurrency))
                {
                    var toRate = rates.Rates[toCurrency];
                    var fromRate = rates.Rates[fromCurrency];
                    
                    result.Rate = toRate / fromRate;                    
                }
                else if(rates.Rates.ContainsKey(toCurrency))
                {
                    result.Rate = rates.Rates[toCurrency];                    
                }
                else
                {
                    return null;
                }
            }
            return result;
        }

        private void SaveRates(ExchangeRateResult rates)
        {
            var cachedRates = new List<DataAccess.Models.CachedExchangeRate>();
            foreach (var rate in rates.Rates)
            {
                cachedRates.Add(new DataAccess.Models.CachedExchangeRate
                {
                    RateDate = rates.Date,
                    CurrencyCodeFrom = rates.Base,
                    CurrencyCodeTo = rate.Key,
                    Rate = rate.Value
                });
            }
            _cachedExchangeRateRepository.SaveRates(cachedRates);
        }
    }
}
