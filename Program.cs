using Microsoft.EntityFrameworkCore;
using Gigashop.Data;
using Gigashop.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Cấu hình dịch vụ OpenAI
var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];
builder.Services.AddSingleton(new OpenAIService(openAiApiKey));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "contact",
    pattern: "home/contact",
    defaults: new { controller = "Home", action = "Contact" });

app.MapControllerRoute(
    name: "shopDetail",
    pattern: "shop/detail/{id}",
    defaults: new { controller = "Shop", action = "Detail" });

app.MapControllerRoute(
    name: "chatbot",
    pattern: "chatbot",
    defaults: new { controller = "ChatBot", action = "Contact" });

// Đảm bảo cho phép Static Files nếu cần (nếu có file CSS, JS, v.v)
app.UseStaticFiles();

app.Run();
