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

            var response = await client.PutAsJsonAsync(ApiUrl + $"/api/Incomes/{Income.IncomeId}", Income);
            if (!response.IsSuccessStatusCode)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }

        
    }
}
