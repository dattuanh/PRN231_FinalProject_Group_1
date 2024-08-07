﻿using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN231_FinalProject_Client.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public List<string> Sources { get; set; } =  new List<string> { "Salary", "Hourly wage", "Interest income", "Child support", "Others" };

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        
        private DateTime? fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { 
            get {return fromDate; }
            set
            {
                fromDate = value;
            }
            }
        private DateTime? toDate = DateTime.Now;
        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate {
            get
            {
                return toDate;
            }
            set
            {
                toDate = value;
            } 
        }

        [BindProperty(SupportsGet = true)]
        public string SelectedSource { get; set; }

        public async Task OnGetAsync(string source)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            int? id = HttpContext.Session.GetInt32("UserId");
            var response = await client.GetAsync(ApiUrl + $"/api/Incomes/User/{id}");
            string strData = await response.Content.ReadAsStringAsync();
            var incomes = JsonSerializer.Deserialize<List<Income>>(strData, options);

            // Apply filters
            if (!string.IsNullOrEmpty(SearchString))
            {
                incomes = incomes.Where(i => i.Description.Contains(SearchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (FromDate.HasValue)
            {
                incomes = incomes.Where(i => i.IncomeDate >= FromDate.Value).ToList();
            }

            if (ToDate.HasValue)
            {
                incomes = incomes.Where(i => i.IncomeDate <= ToDate.Value).ToList();
            }

            if (!string.IsNullOrEmpty(source))
               
            {
                SelectedSource = source;
                incomes = incomes.Where(i => i.Source == source).ToList();
            }

            Income = incomes;

           
        }
    }
}