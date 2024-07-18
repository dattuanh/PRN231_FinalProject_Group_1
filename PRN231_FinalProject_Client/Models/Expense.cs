using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_Client.Models
{
    public partial class Expense
    {
        public int ExpenseId { get; set; }
        public int? UserId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal Amount { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }

        public virtual User? User { get; set; }
    }
}
