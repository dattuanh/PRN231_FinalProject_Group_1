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
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "https://localhost:7203";
        private readonly PRN221_ProjectContext _context;
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

        public IList<Expense> Expenses { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }


        private DateTime? fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
            }
        }
        private DateTime? toDate = DateTime.Now;
        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate
        {
            get
            {
                return toDate;
            }
            set
            {
                toDate = value;
            }
        }

        public async Task OnGetAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            int? id = HttpContext.Session.GetInt32("UserId");
            var response = await client.GetAsync(ApiUrl + $"/api/Expenses/User/{id}");
            string strData = await response.Content.ReadAsStringAsync();
            var expense = JsonSerializer.Deserialize<List<Expense>>(strData, options);

            // Apply filters
            if (!string.IsNullOrEmpty(SearchString))
            {
                expense = expense.Where(i => i.Description.Contains(SearchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (FromDate.HasValue)
            {
                expense = expense.Where(i => i.ExpenseDate >= FromDate.Value).ToList();
            }
            if (ToDate.HasValue)
            {
                expense = expense.Where(i => i.ExpenseDate <= ToDate.Value).ToList();
            }

            
            Expenses = expense;

        }
    }
}
