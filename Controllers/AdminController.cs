using Microsoft.AspNetCore.Mvc;

namespace Gigashop.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            // Kiểm tra quyền Admin
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            Console.WriteLine("Truy cập Dashboard của Admin.");
            return View();
        }
    }
}
