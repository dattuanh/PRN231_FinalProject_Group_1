﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PRN231_FinalProject_Client.Models;
using System.Collections;

namespace PRN231_FinalProject_Client.Pages.Debts
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ReportApiUrl = "";

        public DetailsModel(ILogger<IndexModel> logger)
        {
            this.client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ReportApiUrl = "https://localhost:7203/api/Debts";
        }

        public DebtsLoan DebtsLoan { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{ReportApiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var debts = JsonConvert.DeserializeObject<DebtsLoan>(content);

                    if (debts == null)
                    {
                        return NotFound();
                    }

                    DebtsLoan = debts;
                }
                else
                {
                    return new ContentResult
                    {
                        Content = $"Failed to retrieve details for editing. Status code: {response.StatusCode}",
                        ContentType = "text/plain",
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    Content = $"An error occurred while fetching details for editing: {ex.Message}",
                    ContentType = "text/plain",
                    StatusCode = 500
                };
            }

            return Page();
        }
    }
}
