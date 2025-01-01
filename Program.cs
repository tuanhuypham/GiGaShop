
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
// Thêm route cho admin
app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=AccountManagement}/{id?}");
app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Comment_Management}/{id?}");
app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Message_Management}/{id?}");



// Đảm bảo cho phép Static Files nếu cần (nếu có file CSS, JS, v.v)
app.UseStaticFiles();

// Chạy ứng dụng
app.Run();
