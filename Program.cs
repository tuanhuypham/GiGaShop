﻿using Gigashop.Data;
using Gigashop.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn
    options.Cookie.HttpOnly = true;                // Chỉ sử dụng Session trong HTTP
    options.Cookie.IsEssential = true;             // Cookie cần thiết để hoạt động
});

// Thêm DbContext với SQL Server
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Cấu hình dịch vụ OpenAI
var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];
builder.Services.AddSingleton(new OpenAIService(openAiApiKey));

// Thêm các dịch vụ controller với views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình middleware cho lỗi và bảo mật trong môi trường không phải là Development
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Đảm bảo bảo mật
}

// Cấu hình Session, static files, và routing
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Cấu hình Authorization (nếu cần)
app.UseAuthorization();

// Định nghĩa các routes và controller cho ứng dụng
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "shopDetail",
    pattern: "shop/detail/{id}",
    defaults: new { controller = "Shop", action = "Detail" });

app.MapControllerRoute(
    name: "chatbot",
    pattern: "chatbot",
    defaults: new { controller = "ChatBot", action = "Contact" });

app.MapControllerRoute(
    name: "about",  
    pattern: "about",
    defaults: new { controller = "About", action = "About" });

// Đảm bảo cho phép Static Files nếu cần (nếu có file CSS, JS, v.v)
app.UseStaticFiles();

app.Run();
