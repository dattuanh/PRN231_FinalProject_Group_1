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
        public IndexModel()
        {
            client = new HttpClient();
        }
        [BindProperty(SupportsGet = true)]
        public string SortByReminders { get; set; } = "reminderdate";
        [BindProperty(SupportsGet = true)]
        public string SortOrderReminders { get; set; } = "asc";
        [BindProperty(SupportsGet = true)]
        public string SearchTypeReminders { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SortByRemindersIn24h { get; set; } = "reminderdate";
        [BindProperty(SupportsGet = true)]
        public string SortOrderRemindersIn24h { get; set; } = "asc";
        [BindProperty(SupportsGet = true)]
        public string SearchTypeRemindersIn24h { get; set; } = string.Empty;


        //string sortBy = "ReminderDate", string sortOrder = "asc", string searchType = ""
        // số lượng phần tử trong một trang, trang hiện tại, sort order
        public async Task OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            // chưa làm được chức năng 
            //if (userId == null)
            //{
            //    RedirectToPage("User/Login");
            //    return;
            //}
            //PaymentRemindersList
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7203/api/PaymentReminders/GetPaymentRemindesrByUserId/{userId}");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            PaymentRemindersList = JsonSerializer.Deserialize<List<PaymentReminder>>(strData, options);
            if (!string.IsNullOrEmpty(SearchTypeReminders))
            {
                PaymentRemindersList = PaymentRemindersList.Where(p => p.ReminderDate.ToString().ToLower().Trim().Contains(SearchTypeReminders.ToLower().Trim()) || p.Description.ToLower().Trim().Contains(SearchTypeReminders.ToLower().Trim())).ToList();
            }
            PaymentRemindersList = (SortByReminders.ToLower().Trim(), SortOrderReminders.ToLower().Trim()) switch
            {
                ("reminderdate", "asc") => PaymentRemindersList.OrderBy(x => x.ReminderDate).ToList(),
                ("reminderdate", "desc") => PaymentRemindersList.OrderByDescending(x => x.ReminderDate).ToList(),
                _ => PaymentRemindersList
            };
            //PaymentRemindersDueIn24List
            var today = DateTime.Now;
            var endOfToday = today.AddHours(24);
            response = await client.GetAsync($"https://localhost:7203/api/PaymentReminders/GetPaymentRemindersByTimeAndUserId/{userId}?today={today.ToString("MM-dd-yyyy HH:mm:ss")}&endOfDate={endOfToday.ToString("MM-dd-yyyy HH:mm:ss")}");
            strData = await response.Content.ReadAsStringAsync();
            PaymentRemindersDueIn24List = JsonSerializer.Deserialize<List<PaymentReminder>>(strData, options);
            HttpContext.Session.SetInt32("RemindersCount", PaymentRemindersDueIn24List.Count);
            if (!string.IsNullOrEmpty(SearchTypeRemindersIn24h))
            {
                PaymentRemindersDueIn24List = PaymentRemindersDueIn24List.Where(p => p.ReminderDate.ToString().ToLower().Trim().Contains(SearchTypeRemindersIn24h.ToLower().Trim()) || p.Description.ToLower().Trim().Contains(SearchTypeRemindersIn24h.ToLower().Trim())).ToList();
            }
            PaymentRemindersDueIn24List = (SortByRemindersIn24h.ToLower().Trim(), SortOrderRemindersIn24h.ToLower().Trim()) switch
            {
                ("reminderdate", "asc") => PaymentRemindersDueIn24List.OrderBy(x => x.ReminderDate).ToList(),
                ("reminderdate", "desc") => PaymentRemindersDueIn24List.OrderByDescending(x => x.ReminderDate).ToList(),
                _ => PaymentRemindersDueIn24List
            };
        }
        public async Task OnpostAsync()
        {

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
