using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Debts
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ReportApiUrl = "";

        public CreateModel(ILogger<IndexModel> logger)
        {
            this.client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ReportApiUrl = "https://localhost:7203/api/Debts";
        }

        public IActionResult OnGet()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        [BindProperty]
        public DebtsLoan DebtsLoan { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            DebtsLoan.UserId = userId;
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(DebtsLoan);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(ReportApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
