using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PRN231_FinalProject_Client.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PRN231_FinalProject_Client.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ReportApiUrl = "";
        public RegisterModel()
        {
            this.client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ReportApiUrl = "https://localhost:7203/api/Debts";
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || User == null)
            {
                return Page();
            }
            var json = JsonConvert.SerializeObject(User);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            ReportApiUrl = "https://localhost:7203/api/Users";
            var response = await client.PostAsync(ReportApiUrl, content);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Exist User!");
                return Page();
            }
            //return RedirectToPage("./Login");
        }
    }
}
