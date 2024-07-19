using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Expenses
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "https://localhost:7203";
        private readonly PRN221_ProjectContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly IMemoryCache _cache;
        private readonly string cachingKey = "ExpenseKey";

        public EditModel(IMemoryCache cache, ILogger<IndexModel> logger)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _cache = cache;
            _logger = logger;
        }


        [BindProperty]
        public Expense Expense { get; set; } = default!;

        public List<string> Sources { get; set; }

        public async Task OnGetAsync(int? id)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var response = await client.GetAsync(ApiUrl+"/api/Expenses/" +id);
            string strData = await response.Content.ReadAsStringAsync();
            var expense = System.Text.Json.JsonSerializer.Deserialize<Expense>(strData, options);
            Sources = new List<string> { "Food", "Transportation", "Debt", "Health Care", "Entertainment", "Rent", "Subscription", "Tax", "Maintainance" };

            Expense = expense;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Get the current user
                var userResponse = await client.GetAsync(ApiUrl + $"/api/Users/{HttpContext.Session.GetInt32("UserId")}");
                userResponse.EnsureSuccessStatusCode();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var currentUser = await System.Text.Json.JsonSerializer.DeserializeAsync<User>(await userResponse.Content.ReadAsStreamAsync(), options);

                // Get the original expense
                var expenseResponse = await client.GetAsync(ApiUrl + $"/api/Expenses/{Expense.ExpenseId}");
                expenseResponse.EnsureSuccessStatusCode();
                var originalExpense = await System.Text.Json.JsonSerializer.DeserializeAsync<Expense>(await expenseResponse.Content.ReadAsStreamAsync(), options);
                Expense temp = originalExpense;

                if (originalExpense == null)
                {
                    ModelState.AddModelError(string.Empty, "Original expense not found.");
                    return Page();
                }

                // Calculate the difference in amount
                decimal amountDifference = (decimal)(temp.Amount - Expense.Amount);

                // Update the user's balance
                currentUser.Balance += amountDifference;

                // Update the user
                var updateUserResponse = await client.PutAsJsonAsync(ApiUrl + $"/api/Users/{currentUser.UserId}", currentUser);
                updateUserResponse.EnsureSuccessStatusCode();

                // Update the expense
                Expense.UserId = currentUser.UserId;
                var updateIncomeResponse = await client.PutAsJsonAsync(ApiUrl + $"/api/Expenses/{Expense.ExpenseId}", Expense);

                if (!updateIncomeResponse.IsSuccessStatusCode)
                {
                    var errorContent = await updateIncomeResponse.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Failed to update expense. Status: {updateIncomeResponse.StatusCode}. Error: {errorContent}");
                    return Page();
                }

                return RedirectToPage("./Index");
            }
            catch (HttpRequestException e)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while communicating with the API: {e.Message}");
                return Page();
            }
            catch (System.Text.Json.JsonException e)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while processing the API response: {e.Message}");
                return Page();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {e.Message}");
                return Page();
            }
        }

    }
}
