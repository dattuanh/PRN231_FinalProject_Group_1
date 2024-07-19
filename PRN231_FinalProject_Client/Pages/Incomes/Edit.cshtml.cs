using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Incomes
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "https://localhost:7203";

        public EditModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [BindProperty]
        public Income Income { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = await client.GetAsync(ApiUrl + $"/api/Incomes/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var income = JsonSerializer.Deserialize<Income>(strData, options);
            if (income == null)
            {
                return NotFound();
            }
            Income = income;
            return Page();
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
                var currentUser = await JsonSerializer.DeserializeAsync<User>(await userResponse.Content.ReadAsStreamAsync(), options);

                // Get the original income
                var incomeResponse = await client.GetAsync(ApiUrl + $"/api/Incomes/{Income.IncomeId}");
                incomeResponse.EnsureSuccessStatusCode();
                var originalIncome = await JsonSerializer.DeserializeAsync<Income>(await incomeResponse.Content.ReadAsStreamAsync(), options);

                if (originalIncome == null)
                {
                    ModelState.AddModelError(string.Empty, "Original income not found.");
                    return Page();
                }

                // Calculate the difference in amount
                decimal amountDifference = Income.Amount - originalIncome.Amount;

                // Update the user's balance
                currentUser.Balance += amountDifference;

                // Update the user
                var updateUserResponse = await client.PutAsJsonAsync(ApiUrl + $"/api/Users/{currentUser.UserId}", currentUser);
                updateUserResponse.EnsureSuccessStatusCode();

                // Update the income
                Income.UserId = currentUser.UserId;
                var updateIncomeResponse = await client.PutAsJsonAsync(ApiUrl + $"/api/Incomes/{Income.IncomeId}", Income);

                if (!updateIncomeResponse.IsSuccessStatusCode)
                {
                    var errorContent = await updateIncomeResponse.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Failed to update income. Status: {updateIncomeResponse.StatusCode}. Error: {errorContent}");
                    return Page();
                }

                return RedirectToPage("./Index");
            }
            catch (HttpRequestException e)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while communicating with the API: {e.Message}");
                return Page();
            }
            catch (JsonException e)
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
