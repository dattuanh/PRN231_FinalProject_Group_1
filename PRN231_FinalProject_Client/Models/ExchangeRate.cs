using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_Client.Models
{
    public partial class ExchangeRate
    {
        public int RateId { get; set; }
        public int? FromCurrencyId { get; set; }
        public int? ToCurrencyId { get; set; }
        public decimal? Rate { get; set; }

        public virtual Currency? FromCurrency { get; set; }
        public virtual Currency? ToCurrency { get; set; }
    }
}
