using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddResponseCaching();
            builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageApplicationModelConvention("/Pages/Users/Login", model =>
                {
                    model.Filters.Add(new ResponseCacheAttribute
                    {
                        Duration = 60,
                        Location = ResponseCacheLocation.Client,
                        NoStore = false
                    });
                });
                //options.Conventions.AddPageRoute("/Users/Login", "/user/login123");
                //options.Conventions.AddPageRoute("/Users/Login", "/user/login1234");
                //options.Conventions.AddPageRoute("/Users/Login", "/user/login1234");
            });

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("vi-VN");
            });
            app.UseResponseCaching();
            app.UseSession();
            app.MapRazorPages();
            app.Run();
        }
    }
}