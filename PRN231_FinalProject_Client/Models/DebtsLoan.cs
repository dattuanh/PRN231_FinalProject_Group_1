using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_Client.Models
{
    public partial class DebtsLoan
    {
        public int DebtLoanId { get; set; }
        public int? UserId { get; set; }
        public string? Type { get; set; }
        public decimal? Amount { get; set; }
        public decimal? InterestRate { get; set; }
        public string? Description { get; set; }

        public virtual User? User { get; set; }
    }
}
