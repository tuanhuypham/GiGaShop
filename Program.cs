﻿//using Gigashop.Data;
//using Gigashop.Services;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// Đăng ký Session
//builder.Services.AddSession(options =>
//{

//    options.Cookie.HttpOnly = true;                // Chỉ sử dụng Session trong HTTP
//    options.Cookie.IsEssential = true;             // Cookie cần thiết để hoạt động


//});
//void ConfigureServices(IServiceCollection services)
//{
//    services.AddControllersWithViews();

//    // Cấu hình session
//    services.AddSession(options =>
//    {

//        options.Cookie.HttpOnly = true;
//        options.Cookie.IsEssential = true;
//    });
//}
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        builder => builder.AllowAnyOrigin()
//                          .AllowAnyMethod()
//                          .AllowAnyHeader());
//});

//void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//{
//    if (env.IsDevelopment())
//    {
//        app.UseDeveloperExceptionPage();
//    }
//    else
//    {
//        app.UseExceptionHandler("/Home/Error");
//        app.UseHsts();
//    }
//    app.UseCors("AllowAll");
//    app.UseHttpsRedirection();
//    app.UseStaticFiles();

//    app.UseRouting();
//    app.UseAuthentication();
//    app.UseAuthorization();

//    // Kích hoạt session
//    app.UseSession();

//    app.UseEndpoints(endpoints =>
//    {
//        endpoints.MapControllerRoute(
//            name: "default",
//            pattern: "{controller=Home}/{action=Index}/{id?}");
//    });
//}

//// Thêm DbContext với SQL Server
//builder.Services.AddDbContext<ApplicationDBContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

//// Cấu hình dịch vụ OpenAI
//var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];
//builder.Services.AddSingleton(new OpenAIService(openAiApiKey));

//// Thêm các dịch vụ controller với views
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Cấu hình middleware cho lỗi và bảo mật trong môi trường không phải là Development
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();  // Đảm bảo bảo mật
//}

//// Cấu hình Session, static files, và routing
//app.UseSession();
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//builder.Services.AddSession();
//app.UseSession();

//// Cấu hình Authorization (nếu cần)
//app.UseAuthorization();

//// Định nghĩa các routes và controller cho ứng dụng
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "shopDetail",
//    pattern: "shop/detail/{id}",
//    defaults: new { controller = "Shop", action = "Detail" });

//app.MapControllerRoute(
//    name: "chatbot",
//    pattern: "chatbot",
//    defaults: new { controller = "ChatBot", action = "Contact" });

//app.MapControllerRoute(
//    name: "about",  
//    pattern: "about",
//    defaults: new { controller = "About", action = "About" });

//// Đảm bảo cho phép Static Files nếu cần (nếu có file CSS, JS, v.v)
//app.UseStaticFiles();

//app.Run();


using Gigashop.Data;
using Gigashop.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Thêm các dịch vụ controller với views
builder.Services.AddControllersWithViews();

// Cấu hình dịch vụ Session
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;  // Chỉ sử dụng Session trong HTTP
    options.Cookie.IsEssential = true;  // Cookie cần thiết để hoạt động
});

// Cấu hình DbContext cho SQL Server
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Cấu hình dịch vụ OpenAI
var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];
builder.Services.AddSingleton(new OpenAIService(openAiApiKey));

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();

// Xử lý các lỗi trong môi trường không phải Development
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Đảm bảo bảo mật
}

// Kích hoạt CORS và các middleware cần thiết
app.UseCors("AllowAll");  // Cho phép tất cả các nguồn truy cập
app.UseHttpsRedirection();
app.UseStaticFiles();  // Cho phép Static Files như CSS, JS, Images

// Xử lý Routing, Authentication, Authorization
app.UseRouting();
app.UseAuthentication();  // Cần nếu có sử dụng xác thực
app.UseAuthorization();   // Cần nếu có sử dụng phân quyền

// Kích hoạt Session sau khi đã có các middleware trên
app.UseSession();

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

// Chạy ứng dụng
app.Run();
