using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_Client.Models
{
    public partial class Investment
    {
        public int InvestmentId { get; set; }
        public int? UserId { get; set; }
        public string? InvestmentType { get; set; }
        public DateTime? InvestmentDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }

        public virtual User? User { get; set; }
    }
}
