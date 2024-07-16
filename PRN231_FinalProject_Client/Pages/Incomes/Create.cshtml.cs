using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN231_FinalProject_API.Models;

namespace PRN231_FinalProject_Client.Pages.Incomes
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
            Sources = new List<string> { "Salary", "Hourly wage", "Interest income", "Child support", "Others" };
            return Page();
        }

        [BindProperty]
        public Income Income { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || Income == null)
            {
                return Page();
            }
            //var currentUser = _context.Users.FirstOrDefault(u => u.Username == HttpContext.Session.GetString("Username"));
            var response = await client.GetAsync(ApiUrl + "/api/Users");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var currentUser = await JsonSerializer.DeserializeAsync<User>(await response.Content.ReadAsStreamAsync(), options);
            Income.UserId = currentUser.UserId;
            currentUser.Balance = currentUser.Balance + Income.Amount;
            var json = JsonSerializer.Serialize(currentUser);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            response = await client.PostAsync(ApiUrl + "/api/Incomes/", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            return Page();
        }
    }
}
