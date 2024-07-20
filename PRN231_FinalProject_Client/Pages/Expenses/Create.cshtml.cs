using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Expenses
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "https://localhost:7203";

        public CreateModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);


        }
        public List<string> Sources { get; set; }
        public IActionResult OnGet()
        {
            //if (HttpContext.Session.GetString("Username") == null)
            //{

            //    return RedirectToPage("/Index");
            //}
            Sources = new List<string> { "Food", "Transportation", "Debt", "Health Care", "Entertainment", "Rent", "Subscription", "Tax", "Maintainance"};
            return Page();
        }

        [BindProperty]
        public Expense Expense { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Expense == null)
            {
                return Page();
            }
            //var currentUser = _context.Users.FirstOrDefault(u => u.Username == HttpContext.Session.GetString("Username"));
            var response = await client.GetAsync(ApiUrl + $"/api/Users/{HttpContext.Session.GetInt32("UserId")}");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var currentUser = await JsonSerializer.DeserializeAsync<User>(await response.Content.ReadAsStreamAsync(), options);
            Expense.UserId = currentUser.UserId;
            currentUser.Balance = currentUser.Balance - Expense.Amount;
            response = await client.PutAsync(ApiUrl + $"/api/Users/{currentUser.UserId}", new StringContent(JsonSerializer.Serialize(currentUser), System.Text.Encoding.UTF8, "application/json"));
            var json = JsonSerializer.Serialize(Expense);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            response = await client.PostAsync(ApiUrl + "/api/Expenses/", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            return Page();
        }
    }
}
