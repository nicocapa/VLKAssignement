using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using VLKAssignement.Service.Interfaces;

namespace VLKAssignement.Service
{
    public class WrapperExchangeRateAPI : IWrapperExchangeRateAPI
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<WrapperExchangeRateAPI> _logger;

        public WrapperExchangeRateAPI(IHttpClientFactory clientFactory, ILogger<WrapperExchangeRateAPI> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
        public async Task<ExchangeRateResult> GetLatestRates()
        {
            ExchangeRateResult rateResponse = null;

            try
            {
                var client = _clientFactory.CreateClient("exchangerate");

                var response = await client.GetAsync("/latest");
                
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    rateResponse = JsonConvert.DeserializeObject<ExchangeRateResult>(responseStream);
                }   
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Error getting rates from the Exchange Rate API", ex);                
            }
            return rateResponse;
        }
    }
}
