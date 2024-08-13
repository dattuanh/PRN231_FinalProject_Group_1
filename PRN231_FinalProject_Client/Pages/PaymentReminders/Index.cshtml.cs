using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using PRN231_FinalProject_Client.Models;
using System.Text.Json;

namespace PRN231_FinalProject_Client.Pages.PaymentReminders
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        public List<PaymentReminder> PaymentRemindersList { get; set; } = default!;
        public List<PaymentReminder> PaymentRemindersDueIn24List { get; set; } = default!;
        [BindProperty]
        public string reminderSortOrder { get; set; } = "ReminderDate";
        [BindProperty]
        public string reminderSortOrderBy { get; set; } = "Asc";
        [BindProperty]
        public string reminderSearch { get; set; } = default!;
        public IndexModel()
        {
            client = new HttpClient();
        }
        // số lượng phần tử trong một trang, trang hiện tại, sort order
        public async Task GetData()
        {
            //var userId = HttpContext.Session.GetInt32("UserId");
            var userId = 2;
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7203/api/PaymentReminders/GetPaymentRemindesrByUserId/{userId}");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            PaymentRemindersList = JsonSerializer.Deserialize<List<PaymentReminder>>(strData, options);
            //PaymentRemindersDueIn24List
            var today = DateTime.Now;
            var endOfToday = today.AddHours(24);
            response = await client.GetAsync($"https://localhost:7203/api/PaymentReminders/GetPaymentRemindersByTimeAndUserId/{userId}?today={today.ToString("MM-dd-yyyy HH:mm:ss")}&endOfDate={endOfToday.ToString("MM-dd-yyyy HH:mm:ss")}");
            strData = await response.Content.ReadAsStringAsync();
            PaymentRemindersDueIn24List = JsonSerializer.Deserialize<List<PaymentReminder>>(strData, options);
        }
        public async Task OnpostAsync()
        {
            await GetData();
            if (!string.IsNullOrEmpty(reminderSearch))
            {

            }
            //PaymentRemindersList = PaymentRemindersList.Where(x => x.ReminderName.Contains(reminderSearch)).ToList();
        }
        public async Task OnGet()
        {
            ////var userId = HttpContext.Session.GetInt32("UserId");
            //var userId = 2;
            //// chưa làm được chức năng 
            ////if (userId == null)
            ////{
            ////    RedirectToPage("User/Login");
            ////    return;
            ////}
            ////PaymentRemindersList
            //HttpResponseMessage response = await client.GetAsync($"https://localhost:7203/api/PaymentReminders/GetPaymentRemindesrByUserId/{userId}");
            //string strData = await response.Content.ReadAsStringAsync();
            //var options = new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true,
            //};
            //PaymentRemindersList = JsonSerializer.Deserialize<List<PaymentReminder>>(strData, options);
            ////PaymentRemindersDueIn24List
            //var today = DateTime.Now;
            //var endOfToday = today.AddHours(24);
            //response = await client.GetAsync($"https://localhost:7203/api/PaymentReminders/GetPaymentRemindersByTimeAndUserId/{userId}?today={today.ToString("MM-dd-yyyy HH:mm:ss")}&endOfDate={endOfToday.ToString("MM-dd-yyyy HH:mm:ss")}");
            //strData = await response.Content.ReadAsStringAsync();
            //PaymentRemindersDueIn24List = JsonSerializer.Deserialize<List<PaymentReminder>>(strData, options);
            await GetData();
        }

        public IActionResult OnGetHtmlSnippet()
        {
            // Construct your HTML snippet here. This is just an example.
            string htmlContent = "<div><h1>Hello, this is a sample HTML snippet!</h1></div>";

            // Return the HTML content with the correct content type.
            return new ContentResult
            {
                Content = htmlContent,
                ContentType = "text/html",
                StatusCode = 200 // OK
            };
        }

    }
}
