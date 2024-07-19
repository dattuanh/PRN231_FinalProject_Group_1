using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Debts
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client;
        private string ReportApiUrl = "https://localhost:7203/api/Debts";

        public IndexModel(ILogger<IndexModel> logger)
        {
            this.client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public IList<DebtsLoan> DebtsLoan { get;set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string SearchType { get; set; }


        [HttpGet]
        public async Task OnGetAsync(string sortBy = "Amount", string sortOrder = "asc", string searchType = "")
        {
            SortBy = sortBy;
            SortOrder = sortOrder;
            SearchType = searchType;

            string apiUrl = $"{ReportApiUrl}?sortBy={sortBy}&sortOrder={sortOrder}&searchType={searchType}";
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            List<DebtsLoan> listDebts = JsonSerializer.Deserialize<List<DebtsLoan>>(strData, options);

            DebtsLoan = listDebts.ToList();
        }

    }
}
