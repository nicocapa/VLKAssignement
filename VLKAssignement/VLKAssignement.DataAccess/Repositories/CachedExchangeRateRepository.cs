using System;
using VLKAssignement.DataAccess.Repositories.Interfaces;
using System.Linq;
using VLKAssignement.DataAccess.Models;
using System.Collections.Generic;

namespace VLKAssignement.DataAccess.Repositories
{
    public class CachedExchangeRateRepository : RepositoryBase<Models.CachedExchangeRate>, ICachedExchangeRateRepository    
    {
        public CachedExchangeRateRepository(AppDbContext context) : base(context) { }

        public List<CachedExchangeRate> GetRate(DateTime date, string fromCurrency, string toCurrency)
        {
            return _context.CachedExchangeRates.Where(fx => fx.RateDate <= date && fx.RateDate >= date.AddDays(-1)  && (fx.CurrencyCodeFrom == fromCurrency || fx.CurrencyCodeFrom == toCurrency || fx.CurrencyCodeTo == toCurrency || fx.CurrencyCodeTo == fromCurrency)).ToList();
        }

        public void SaveRates(List<CachedExchangeRate> rates)
        {
            foreach (var item in rates)
            {
                _context.CachedExchangeRates.Add(item);
            }
            _context.SaveChanges();
        }
    }
}
