using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN231_FinalProject_Client.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PRN231_FinalProject_Client.Pages.Financial
{
    public class IndexModel : PageModel
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7203";

        public IndexModel()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public decimal TotalExpense { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal Balance { get; set; }
        public decimal Budget { get; set; }

        [BindProperty(SupportsGet = true)]

        public List<string> Categories { get; set; }
        public List<decimal> ExpenseAmounts { get; set; }
        public List<decimal> IncomeAmounts { get; set; }
        public List<decimal> ExpenseAmountsOverTime { get; set; }
        public List<string> Dates { get; set; }
        public string culture = "vi-VN";


        public async Task OnGetAsync()
        { 

            var response = await _httpClient.GetAsync(_apiUrl + $"/api/Users/{HttpContext.Session.GetInt32("UserId")}");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var currentUser = await JsonSerializer.DeserializeAsync<User>(await response.Content.ReadAsStreamAsync(), options);


            response = await _httpClient.GetAsync(_apiUrl + $"/api/Expenses/User/{currentUser.UserId}");
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var Expense = await JsonSerializer.DeserializeAsync<List<Expense>>(await response.Content.ReadAsStreamAsync(), options);
            TotalExpense = Expense.Where(e=>e.ExpenseDate.Month == DateTime.Now.Month).Sum(e => e.Amount);

            response = await _httpClient.GetAsync(_apiUrl + $"/api/Incomes/User/{currentUser.UserId}");
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var Income = await JsonSerializer.DeserializeAsync<List<Income>>(await response.Content.ReadAsStreamAsync(), options);
            TotalIncome = Income.Sum(i => i.Amount);

            Balance = currentUser.Balance;

            response = await _httpClient.GetAsync(_apiUrl + $"/api/Budgets/User/{currentUser.UserId}");
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var Budgets = await JsonSerializer.DeserializeAsync<List<Budget>>(await response.Content.ReadAsStreamAsync(), options);

            Budget = Budgets.Sum(b => b.TotalBudget);
            // Fetch expense categories and amounts
            Categories = new List<string> {"Housing", "Food", "Transportation", "Shopping", "Debt Payments", "Others" };
            ExpenseAmounts = new List<decimal>();
            foreach (var category in Categories)
            {
                var totalExpense = Expense
                    .Where(e => e.Category == category)
                    .Sum(e => e.Amount);
                ExpenseAmounts.Add(totalExpense);
            }

            var startDate = DateTime.Today.AddMonths(-6); // Assuming you want data for the last 6 months
            var endDate = DateTime.Today;
            Dates = new List<string>();
            IncomeAmounts = new List<decimal>();
            ExpenseAmountsOverTime = new List<decimal>();
            for (var date = startDate; date <= endDate; date = date.AddMonths(1))
            {
                Dates.Add(date.ToString("MMM yyyy"));

                var totalIncome = Income
                    .Where(i => i.IncomeDate.Month == date.Month && i.IncomeDate.Year == date.Year)
                    .Sum(i => i.Amount);
                IncomeAmounts.Add(totalIncome);

                var totalExpense = Expense
                    .Where(e => e.ExpenseDate.Month == date.Month && e.ExpenseDate.Year == date.Year)
                    .Sum(e => e.Amount);
                ExpenseAmountsOverTime.Add(totalExpense);
            }
        }
        //if (HttpContext.Session.GetString("NewCurrency") != "1")
        //{

        //    var newCurrency = HttpContext.Session.GetString("NewCurrency");

        //    if (!string.IsNullOrEmpty(newCurrency))
        //    {
        //        if (int.TryParse(newCurrency, out int newCurrencyId))
        //        {
        //            switch (newCurrencyId)
        //            {
        //                case 1:
        //                    culture = "vi-VN";
        //                    break;
        //                case 2:
        //                    culture = "en-US";
        //                    break;
        //                case 3:
        //                    culture = "fr-FR";
        //                    break;

        //            }
        //            var eRate = _context.ExchangeRates
        //                .FirstOrDefault(e => e.ToCurrencyId == newCurrencyId);

        //            if (eRate != null)
        //            {

        //                TotalExpense = TotalExpense * eRate.Rate;
        //                TotalIncome = TotalIncome * eRate.Rate;
        //                Balance = Balance * eRate.Rate;
        //                Budget = Budget * eRate.Rate;
        //                for (int i = 0; i < ExpenseAmounts.Count; i++)
        //                {
        //                    ExpenseAmounts[i] = ExpenseAmounts[i] * eRate.Rate;
        //                }
        //                for (int i = 0; i < IncomeAmounts.Count; i++)
        //                {
        //                    IncomeAmounts[i] *= eRate.Rate;
        //                }
        //                for (int i = 0; i < ExpenseAmountsOverTime.Count; i++)
        //                {
        //                    ExpenseAmountsOverTime[i] *= eRate.Rate;
        //                }
        //            }
        //        }
        //    }
        //}


    }
}
