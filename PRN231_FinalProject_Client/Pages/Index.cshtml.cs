using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Memory;

using PRN231_FinalProject_Client.Models;
using System.Diagnostics;
using System.Net.Http;

namespace PRN231_FinalProject_Client.Pages
{
 
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IndexModel> _logger;
        
        private readonly IConfiguration _configuration;
        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
         
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public decimal TotalExpense { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal Balance { get; set; }
        public decimal Budget { get; set; }
       
        [BindProperty(SupportsGet = true)]

        public List<Expense> RecentExpenses { get; set; }
        public List<PaymentReminder> PaymentReminders { get; set; }
        public List<string> Categories { get; set; }
        public List<decimal> ExpenseAmounts { get; set; }
        public List<decimal> IncomeAmounts { get; set; }
        public List<decimal> ExpenseAmountsOverTime { get; set; }
        public List<string> Dates { get; set; }
        public string culture = "vi-VN";
        public async Task OnGetAsync()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    // Handle case where userId is not set
                    return;
                }
                var apiUrl = "https://localhost:7203";
                User user = new User();
                
              
                var userUrl = $"{apiUrl}/api/Users/{userId}";
                var incomeUrl = $"{apiUrl}/api/Incomes/total?id={userId}";
                var expenseUrl = $"{apiUrl}/api/Expenses/total?id={userId}";
                var paymentUrl = $"{apiUrl}/api/PaymentReminders/GetFutureRemindersForUser/future/{userId}";
                var recentExpensesUrl = $"{apiUrl}/api/Expenses/recent/{userId}";

                using var userResponse = await _httpClient.GetAsync(userUrl, HttpCompletionOption.ResponseHeadersRead);
                using var incomeResponse = await _httpClient.GetAsync(incomeUrl, HttpCompletionOption.ResponseHeadersRead);
                using var expenseResponse = await _httpClient.GetAsync(expenseUrl, HttpCompletionOption.ResponseHeadersRead);
                using var paymentResponse = await _httpClient.GetAsync(paymentUrl, HttpCompletionOption.ResponseHeadersRead);
                using var recentExpensesResponse = await _httpClient.GetAsync(recentExpensesUrl, HttpCompletionOption.ResponseHeadersRead);

                userResponse.EnsureSuccessStatusCode();
                incomeResponse.EnsureSuccessStatusCode();
                expenseResponse.EnsureSuccessStatusCode();
                paymentResponse.EnsureSuccessStatusCode();
                recentExpensesResponse.EnsureSuccessStatusCode();
                
                user= await userResponse.Content.ReadFromJsonAsync<User>();
                Balance = user.Balance;
                TotalIncome = await incomeResponse.Content.ReadFromJsonAsync<decimal>();
                TotalExpense = await expenseResponse.Content.ReadFromJsonAsync<decimal>();
                PaymentReminders = await paymentResponse.Content.ReadFromJsonAsync<List<PaymentReminder>>();
                RecentExpenses = await recentExpensesResponse.Content.ReadFromJsonAsync<List<Expense>>();
            }
            catch (HttpRequestException e)
            {
                // Log the error
                Console.WriteLine($"HTTP request error: {e.Message}");
                // Set default values or handle the error as appropriate
                TotalIncome = TotalExpense = 0;
                PaymentReminders = new List<PaymentReminder>();
                RecentExpenses = new List<Expense>();
            }
            catch (IOException e)
            {
                // Log the error
                Console.WriteLine($"IO error: {e.Message}");
                // Set default values or handle the error as appropriate
                TotalIncome = TotalExpense = 0;
                PaymentReminders = new List<PaymentReminder>();
                RecentExpenses = new List<Expense>();
            }
            catch (Exception e)
            {
                // Log any other unexpected errors
                Console.WriteLine($"Unexpected error: {e.Message}");
                // Set default values or handle the error as appropriate
                TotalIncome = TotalExpense = 0;
                PaymentReminders = new List<PaymentReminder>();
                RecentExpenses = new List<Expense>();
            }
        }


    }
    }
