using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231_FinalProject_API.Models
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
        [JsonIgnore]
        public virtual ICollection<AutomatedTransaction> AutomatedTransactions { get; set; }
        [JsonIgnore]
        public virtual ICollection<Budget> Budgets { get; set; }
        [JsonIgnore]
        public virtual ICollection<DebtsLoan> DebtsLoans { get; set; }
        [JsonIgnore]
        public virtual ICollection<Expense> Expenses { get; set; }
        [JsonIgnore]
        public virtual ICollection<Income> Incomes { get; set; }
        [JsonIgnore]
        public virtual ICollection<Investment> Investments { get; set; }
        [JsonIgnore]
        public virtual ICollection<PaymentReminder> PaymentReminders { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductComparison> ProductComparisons { get; set; }
        [JsonIgnore]
        public virtual ICollection<Security> Securities { get; set; }
        [JsonIgnore]
        public virtual ICollection<TransactionLog> TransactionLogs { get; set; }
    }
}
