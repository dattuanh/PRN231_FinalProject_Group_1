using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_Client.Models
{
    public partial class Budget
    {
        public int BudgetId { get; set; }
        public int? UserId { get; set; }
        public DateTime Month { get; set; }
        public decimal TotalBudget { get; set; }
        public string? Description { get; set; }

        public virtual User? User { get; set; }
    }
}
