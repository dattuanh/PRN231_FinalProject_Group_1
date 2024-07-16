using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_Client.Models
{
    public partial class User
    {
        public User()
        {
            AutomatedTransactions = new HashSet<AutomatedTransaction>();
            Budgets = new HashSet<Budget>();
            DebtsLoans = new HashSet<DebtsLoan>();
            Expenses = new HashSet<Expense>();
            Incomes = new HashSet<Income>();
            Investments = new HashSet<Investment>();
            PaymentReminders = new HashSet<PaymentReminder>();
            ProductComparisons = new HashSet<ProductComparison>();
            Securities = new HashSet<Security>();
            TransactionLogs = new HashSet<TransactionLog>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public decimal? Balance { get; set; }

        public virtual ICollection<AutomatedTransaction> AutomatedTransactions { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<DebtsLoan> DebtsLoans { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Income> Incomes { get; set; }
        public virtual ICollection<Investment> Investments { get; set; }
        public virtual ICollection<PaymentReminder> PaymentReminders { get; set; }
        public virtual ICollection<ProductComparison> ProductComparisons { get; set; }
        public virtual ICollection<Security> Securities { get; set; }
        public virtual ICollection<TransactionLog> TransactionLogs { get; set; }
    }
}
