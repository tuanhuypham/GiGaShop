using Gigashop.Data;
using Gigashop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Gigashop.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AuthController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Dữ liệu nhập không hợp lệ.");
                return View(model);
            }

            // Mã hóa mật khẩu và kiểm tra người dùng
            var passwordHash = ComputeHash(model.Password);
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == passwordHash);

            if (user == null)
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác.");
                Console.WriteLine("Đăng nhập thất bại: Tài khoản hoặc mật khẩu không đúng.");
                return View(model);
            }

            // Lưu thông tin vào Session
            HttpContext.Session.SetString("UserId", user.UserID.ToString());
            HttpContext.Session.SetString("Username", user.Username);

            Console.WriteLine($"Đăng nhập thành công: {user.Username}");
            return RedirectToAction("Index", "Home");
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem email đã tồn tại chưa
                    if (_context.Users.Any(u => u.Email == model.Email))
                    {
                        ModelState.AddModelError("", "Email đã được sử dụng. Vui lòng chọn email khác.");
                        return View(model);
                    }

                    // Mã hóa mật khẩu
                    var passwordHash = ComputeHash(model.Password);

                    // Tạo đối tượng User mới
                    var user = new User
                    {
                        Username = model.Username,
                        PasswordHash = passwordHash,
                        FullName = model.FullName,
                        Email = model.Email,
                        Phone = model.Phone,
                        Address = model.Address,
                        Role = "User", // Gán role mặc định
                        CreatedAt = DateTime.Now
                    };

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Đăng ký thành công!";
                    Console.WriteLine($"Người dùng mới: {user.Username} đã được thêm vào cơ sở dữ liệu.");

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi thêm người dùng: {ex.Message}");
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
                }
            }

            return View(model);
        }

        // Hàm mã hóa mật khẩu
        private string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
