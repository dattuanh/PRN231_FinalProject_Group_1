using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Expenses
{
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "https://localhost:7203";
        private readonly PRN221_ProjectContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly IMemoryCache _cache;
        private readonly string cachingKey = "ExpenseKey";
        public DeleteModel(IMemoryCache cache, ILogger<IndexModel> logger)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _cache = cache;
            _logger = logger;
        }

        [BindProperty]
        public Expense Expense { get; set; } = default!;

        public async Task OnGetAsync(int? id)
        {
            var response = await client.GetAsync(ApiUrl + "/api/Users/1");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var currentUser = await JsonSerializer.DeserializeAsync<User>(await response.Content.ReadAsStreamAsync(), options);

            response = await client.GetAsync(ApiUrl + "/api/Expenses/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var expense = JsonSerializer.Deserialize<Expense>(strData, options);

            Expense = expense;
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                String tempUrl = ApiUrl + "/api/Expenses";
                var response = await client.DeleteAsync($"{tempUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    Console.WriteLine(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return RedirectToPage("./Index");
        }
    }
}
