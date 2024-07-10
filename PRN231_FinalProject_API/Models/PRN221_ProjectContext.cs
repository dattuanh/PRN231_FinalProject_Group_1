using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN231_FinalProject_API.Models
{
    public partial class PRN221_ProjectContext : DbContext
    {
        public PRN221_ProjectContext()
        {
        }

        public PRN221_ProjectContext(DbContextOptions<PRN221_ProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AutomatedTransaction> AutomatedTransactions { get; set; } = null!;
        public virtual DbSet<Budget> Budgets { get; set; } = null!;
        public virtual DbSet<Currency> Currencies { get; set; } = null!;
        public virtual DbSet<DebtsLoan> DebtsLoans { get; set; } = null!;
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; } = null!;
        public virtual DbSet<Expense> Expenses { get; set; } = null!;
        public virtual DbSet<FinancialProduct> FinancialProducts { get; set; } = null!;
        public virtual DbSet<Income> Incomes { get; set; } = null!;
        public virtual DbSet<Investment> Investments { get; set; } = null!;
        public virtual DbSet<PaymentReminder> PaymentReminders { get; set; } = null!;
        public virtual DbSet<ProductComparison> ProductComparisons { get; set; } = null!;
        public virtual DbSet<Security> Securities { get; set; } = null!;
        public virtual DbSet<TransactionLog> TransactionLogs { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DB"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AutomatedTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK__Automate__55433A4B8458CB5B");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AutomatedTransactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Automated__UserI__4F7CD00D");
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.Property(e => e.BudgetId).HasColumnName("BudgetID");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Month).HasColumnType("date");

                entity.Property(e => e.TotalBudget).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Budgets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Budgets__UserID__5070F446");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.CurrencyCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DebtsLoan>(entity =>
            {
                entity.HasKey(e => e.DebtLoanId)
                    .HasName("PK__DebtsLoa__27937CA54E86BB1A");

                entity.Property(e => e.DebtLoanId).HasColumnName("DebtLoanID");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DebtsLoans)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__DebtsLoan__UserI__5165187F");
            });

            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.HasKey(e => e.RateId)
                    .HasName("PK__Exchange__58A7CCBC6C54BE80");

                entity.Property(e => e.RateId).HasColumnName("RateID");

                entity.Property(e => e.FromCurrencyId).HasColumnName("FromCurrencyID");

                entity.Property(e => e.Rate).HasColumnType("decimal(15, 6)");

                entity.Property(e => e.ToCurrencyId).HasColumnName("ToCurrencyID");

                entity.HasOne(d => d.FromCurrency)
                    .WithMany(p => p.ExchangeRateFromCurrencies)
                    .HasForeignKey(d => d.FromCurrencyId)
                    .HasConstraintName("FK__ExchangeR__FromC__52593CB8");

                entity.HasOne(d => d.ToCurrency)
                    .WithMany(p => p.ExchangeRateToCurrencies)
                    .HasForeignKey(d => d.ToCurrencyId)
                    .HasConstraintName("FK__ExchangeR__ToCur__534D60F1");
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ExpenseDate).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Expenses__UserID__5441852A");
            });

            modelBuilder.Entity<FinancialProduct>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__Financia__B40CC6EDAB05DAE6");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Income>(entity =>
            {
                entity.ToTable("Income");

                entity.Property(e => e.IncomeId).HasColumnName("IncomeID");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.IncomeDate).HasColumnType("date");

                entity.Property(e => e.Source)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Incomes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Income__UserID__5535A963");
            });

            modelBuilder.Entity<Investment>(entity =>
            {
                entity.Property(e => e.InvestmentId).HasColumnName("InvestmentID");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.InvestmentDate).HasColumnType("date");

                entity.Property(e => e.InvestmentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Investments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Investmen__UserI__5629CD9C");
            });

            modelBuilder.Entity<PaymentReminder>(entity =>
            {
                entity.HasKey(e => e.ReminderId)
                    .HasName("PK__PaymentR__01A830A73A80C0F7");

                entity.Property(e => e.ReminderId).HasColumnName("ReminderID");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ReminderDate).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PaymentReminders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PaymentRe__UserI__571DF1D5");
            });

            modelBuilder.Entity<ProductComparison>(entity =>
            {
                entity.HasKey(e => e.ComparisonId)
                    .HasName("PK__ProductC__6E1F99B76F7081E8");

                entity.Property(e => e.ComparisonId).HasColumnName("ComparisonID");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductComparisons)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__ProductCo__Produ__5812160E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProductComparisons)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ProductCo__UserI__59063A47");
            });

            modelBuilder.Entity<Security>(entity =>
            {
                entity.ToTable("Security");

                entity.Property(e => e.SecurityId).HasColumnName("SecurityID");

                entity.Property(e => e.EncryptionKey)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Securities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Security__UserID__59FA5E80");
            });

            modelBuilder.Entity<TransactionLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__Transact__5E5499A8A90EAF76");

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TransactionLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Transacti__UserI__5AEE82B9");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
