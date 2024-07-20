using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN231_FinalProject_API.Migrations
{
    public partial class initialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyCode = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    CurrencyName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyID);
                });

            migrationBuilder.CreateTable(
                name: "FinancialProducts",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Financia__B40CC6EDAB05DAE6", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Balance = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    RateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromCurrencyID = table.Column<int>(type: "int", nullable: true),
                    ToCurrencyID = table.Column<int>(type: "int", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(15,6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exchange__58A7CCBC6C54BE80", x => x.RateID);
                    table.ForeignKey(
                        name: "FK__ExchangeR__FromC__52593CB8",
                        column: x => x.FromCurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyID");
                    table.ForeignKey(
                        name: "FK__ExchangeR__ToCur__534D60F1",
                        column: x => x.ToCurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyID");
                });

            migrationBuilder.CreateTable(
                name: "AutomatedTransactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Automate__55433A4B8458CB5B", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK__Automated__UserI__4F7CD00D",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    BudgetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<DateTime>(type: "date", nullable: true),
                    TotalBudget = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.BudgetID);
                    table.ForeignKey(
                        name: "FK__Budgets__UserID__5070F446",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "DebtsLoans",
                columns: table => new
                {
                    DebtLoanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DebtsLoa__27937CA54E86BB1A", x => x.DebtLoanID);
                    table.ForeignKey(
                        name: "FK__DebtsLoan__UserI__5165187F",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ExpenseDate = table.Column<DateTime>(type: "date", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    Category = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseID);
                    table.ForeignKey(
                        name: "FK__Expenses__UserID__5441852A",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Income",
                columns: table => new
                {
                    IncomeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    IncomeDate = table.Column<DateTime>(type: "date", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    Source = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Income", x => x.IncomeID);
                    table.ForeignKey(
                        name: "FK__Income__UserID__5535A963",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    InvestmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    InvestmentType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    InvestmentDate = table.Column<DateTime>(type: "date", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.InvestmentID);
                    table.ForeignKey(
                        name: "FK__Investmen__UserI__5629CD9C",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PaymentReminders",
                columns: table => new
                {
                    ReminderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ReminderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentR__01A830A73A80C0F7", x => x.ReminderID);
                    table.ForeignKey(
                        name: "FK__PaymentRe__UserI__571DF1D5",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "ProductComparisons",
                columns: table => new
                {
                    ComparisonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductC__6E1F99B76F7081E8", x => x.ComparisonID);
                    table.ForeignKey(
                        name: "FK__ProductCo__Produ__5812160E",
                        column: x => x.ProductID,
                        principalTable: "FinancialProducts",
                        principalColumn: "ProductID");
                    table.ForeignKey(
                        name: "FK__ProductCo__UserI__59063A47",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Security",
                columns: table => new
                {
                    SecurityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    EncryptionKey = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Security", x => x.SecurityID);
                    table.ForeignKey(
                        name: "FK__Security__UserID__59FA5E80",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "TransactionLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transact__5E5499A8A90EAF76", x => x.LogID);
                    table.ForeignKey(
                        name: "FK__Transacti__UserI__5AEE82B9",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutomatedTransactions_UserID",
                table: "AutomatedTransactions",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserID",
                table: "Budgets",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DebtsLoans_UserID",
                table: "DebtsLoans",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_FromCurrencyID",
                table: "ExchangeRates",
                column: "FromCurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_ToCurrencyID",
                table: "ExchangeRates",
                column: "ToCurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserID",
                table: "Expenses",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Income_UserID",
                table: "Income",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_UserID",
                table: "Investments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReminders_UserID",
                table: "PaymentReminders",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComparisons_ProductID",
                table: "ProductComparisons",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComparisons_UserID",
                table: "ProductComparisons",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Security_UserID",
                table: "Security",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_UserID",
                table: "TransactionLogs",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutomatedTransactions");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "DebtsLoans");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Income");

            migrationBuilder.DropTable(
                name: "Investments");

            migrationBuilder.DropTable(
                name: "PaymentReminders");

            migrationBuilder.DropTable(
                name: "ProductComparisons");

            migrationBuilder.DropTable(
                name: "Security");

            migrationBuilder.DropTable(
                name: "TransactionLogs");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "FinancialProducts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
