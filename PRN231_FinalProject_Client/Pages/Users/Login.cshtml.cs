using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PRN231_FinalProject_Client.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
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
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("JWTToken");
            HttpContext.Session.Remove("RemindersCount");
            return RedirectToPage("/Users/Login");
        }
        //public void OnPostRegister()
        //{
        //    return;
        //    string a = "";
        //    Response.
        //    //Response.Redirect("/Users/Register");
        //}
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7203/api/Users/{Username}/{Password}");
            string strData = await response.Content.ReadAsStringAsync();
            var tmp = JObject.Parse(strData);
            var user = tmp["user"].ToObject<User>();
            var token = tmp["data"].ToString();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            //var user = JsonSerializer.Deserialize<User>(strData, options);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("JWTToken", token);
                //HttpContext.Session.SetString("NewCurrency", "1");
                //var userId = HttpContext.Session.GetInt32("UserId");
                var today = DateTime.Now;
                var endOfToday = today.AddHours(24);
                response = await client.GetAsync($"https://localhost:7203/api/PaymentReminders/GetPaymentRemindersByTimeAndUserId/{user.UserId}?today={today.ToString("MM-dd-yyyy HH:mm:ss")}&endOfDate={endOfToday.ToString("MM-dd-yyyy HH:mm:ss")}");
                strData = await response.Content.ReadAsStringAsync();
                var reminders = System.Text.Json.JsonSerializer.Deserialize<List<PaymentReminder>>(strData, options);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.GetAsync("https://localhost:7203/api/Users");
                strData = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetInt32("RemindersCount", reminders.Count);
                return RedirectToPage("/PaymentReminders/Index");
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
