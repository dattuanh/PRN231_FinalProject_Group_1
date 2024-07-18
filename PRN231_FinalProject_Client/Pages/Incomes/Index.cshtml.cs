using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Incomes
{
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "https://localhost:7203";
        public IndexModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public IList<Income> Income { get; set; } = default!;

        public async Task OnGetAsync()
        {
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            
            int? id  = HttpContext.Session.GetInt32("UserId");
            var response = await client.GetAsync(ApiUrl + $"/api/Incomes/User/{id}");
            string strData = await response.Content.ReadAsStringAsync();
            var income = JsonSerializer.Deserialize<List<Income>>(strData, options);

            Income = income;
        }
    }
}
