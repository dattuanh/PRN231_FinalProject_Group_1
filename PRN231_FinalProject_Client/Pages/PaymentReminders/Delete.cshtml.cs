using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PRN231_FinalProject_Client.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PRN231_FinalProject_Client.Pages.PaymentReminders
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ReportApiUrl = "";

        public DeleteModel(ILogger<IndexModel> logger)
        {
            this.client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ReportApiUrl = "https://localhost:7203/api/PaymentReminders/GetPaymentReminder";
        }

        [BindProperty]
        public PaymentReminder PaymentReminder { get; set; }

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
                    var paymentReminder = JsonConvert.DeserializeObject<PaymentReminder>(content); // Assuming Staff is your model class

                    if (paymentReminder == null)
                    {
                        return NotFound();
                    }

                    PaymentReminder = paymentReminder;
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

            var paymentReminderId = PaymentReminder.ReminderId;

            if (paymentReminderId == null)
            {
                return NotFound();
            }

            //decimal num = (decimal)PaymentReminder.InterestRate;

            try
            {
                ReportApiUrl = "https://localhost:7203/api/PaymentReminders/DeletePaymentReminder";
                var json = JsonConvert.SerializeObject(PaymentReminder);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.DeleteAsync($"{ReportApiUrl}/{paymentReminderId}");

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
