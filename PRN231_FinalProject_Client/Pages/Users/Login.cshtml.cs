using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN231_FinalProject_Client.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PRN231_FinalProject_Client.Pages.Users
{
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
    public class LoginModel : PageModel
    {
        HttpClient client;
        public LoginModel()
        {
            client = new HttpClient();
        }
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7203/api/Users/{Username}/{Password}");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var user = JsonSerializer.Deserialize<User>(strData, options);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                //HttpContext.Session.SetString("NewCurrency", "1");
                //var userId = HttpContext.Session.GetInt32("UserId");
                var today = DateTime.Now;
                var endOfToday = today.AddHours(24);
                response = await client.GetAsync($"https://localhost:7203/api/PaymentReminders/GetPaymentRemindersByTimeAndUserId/{user.UserId}?today={today}&endOfDate={endOfToday}");
                strData = await response.Content.ReadAsStringAsync();
                var reminders = JsonSerializer.Deserialize<List<PaymentReminder>>(strData, options);
                //var reminders = _context.PaymentReminders
                //    .Include(p => p.User)
                //    .Where(p => p.ReminderDate >= today && p.ReminderDate <= endOfToday && p.User.UserId == userId)
                //    .ToList();
                HttpContext.Session.SetInt32("RemindersCount", reminders.Count);
                //Console.WriteLine("count " + reminders.Count);

                return RedirectToPage("/Index");
            }
            else
            {
                // Authentication failed, add a model error and stay on the login page
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return Page();
            }
        }
    }
}
