using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_API.Models
{
    public partial class Currency
    {
        public Currency()
        {
            ExchangeRateFromCurrencies = new HashSet<ExchangeRate>();
            ExchangeRateToCurrencies = new HashSet<ExchangeRate>();
        }

        public int CurrencyId { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencyName { get; set; }

        public virtual ICollection<ExchangeRate> ExchangeRateFromCurrencies { get; set; }
        public virtual ICollection<ExchangeRate> ExchangeRateToCurrencies { get; set; }
    }
}
