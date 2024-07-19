using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Investments
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
        public IActionResult OnGet()
        {
        
            return Page();
        }

        [BindProperty]
        public Investment Investment { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Investment == null)
            {
                return Page();
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Investment.UserId = HttpContext.Session.GetInt32("UserId");
            //currentUser.Balance = currentUser.Balance - Investment.Amount;
            var json = JsonSerializer.Serialize(Investment);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ApiUrl + "/api/Investments/", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            return Page();
        }
    }
}
