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

namespace PRN231_FinalProject_Client.Pages.Investments
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "https://localhost:7203";

        private readonly ILogger<IndexModel> _logger;
        private readonly IMemoryCache _cache;
        private readonly string cachingKey = "ExpenseKey";

        public IndexModel(IMemoryCache cache, ILogger<IndexModel> logger)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _cache = cache;
            _logger = logger;
        }

        public IList<Investment> Investment { get;set; }

        public async Task OnGetAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await client.GetAsync(ApiUrl + "/api/Investments");
            string strData = await response.Content.ReadAsStringAsync();
            var investment = JsonSerializer.Deserialize<List<Investment>>(strData, options);

            Investment = investment;
        }
    }
}
