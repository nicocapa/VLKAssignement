using System;
using System.Collections.Generic;

namespace VLKAssignement.Service
{
    public class ExchangeRateResult
    {
        public Dictionary<string, decimal> Rates { get; set; }

        public string Base { get; set; }

        public DateTime Date { get; set; }
    }
}
