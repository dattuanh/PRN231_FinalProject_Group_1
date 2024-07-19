using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Debts
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ReportApiUrl = "";

        public EditModel(ILogger<IndexModel> logger)
        {
            this.client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ReportApiUrl = "https://localhost:7203/api/Debts";
        }

        [BindProperty]
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
                    var debst = JsonConvert.DeserializeObject<DebtsLoan>(content); // Assuming Staff is your model class

                    if (debst == null)
                    {
                        return NotFound();
                    }

                    DebtsLoan = debst;
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
               return Page();
            }

            var debtsID = DebtsLoan.DebtLoanId;

            if (debtsID == null)
            {
                return NotFound();
            }

            decimal num = (decimal)DebtsLoan.InterestRate;

            try
            {
                var json = JsonConvert.SerializeObject(DebtsLoan);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PatchAsync($"{ReportApiUrl}/{debtsID}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    return new ContentResult
                    {
                        Content = $"Failed to update staff. Status code: {response.StatusCode}",
                        ContentType = "text/plain",
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    Content = $"An error occurred while deleting staff: {ex.Message}",
                    ContentType = "text/plain",
                    StatusCode = 500
                };
            }
        }
    }
}
