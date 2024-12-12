using Gigashop.Data;
using Gigashop.Models;
using Microsoft.AspNetCore.Mvc;
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
            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Dữ liệu nhập không hợp lệ.");
                return View(model);
            }

            // Tạo hash mật khẩu và kiểm tra người dùng
            var passwordHash = ComputeHash(model.Password);
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == passwordHash);

            if (user == null)
            {
                // Thêm lỗi vào ModelState
                ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác.");
                Console.WriteLine("Đăng nhập thất bại: Tài khoản hoặc mật khẩu không đúng.");
                return View(model);
            }

            // Lưu thông tin người dùng vào Session
            HttpContext.Session.SetString("UserId", user.UserID.ToString());
            HttpContext.Session.SetString("Username", user.Username);

            // Đăng nhập thành công
            Console.WriteLine($"Đăng nhập thành công: {user.Username}");
            return RedirectToAction("Index", "Home");
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: Register
        [HttpPost]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Nếu tất cả validation hợp lệ, tiếp tục xử lý (lưu vào database...)
                try
                {
                    var user = new User
                    {
                        Username = model.Username,
                        PasswordHash = model.Password, // Lưu mật khẩu mã hóa
                        FullName = model.FullName,
                        Email = model.Email,
                        Phone = model.Phone,
                        Address = model.Address
                    };

                    _context.Users.Add(user); // Thêm người dùng vào cơ sở dữ liệu
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Đăng ký thành công!";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.Message);
                }
            }

            // Nếu ModelState không hợp lệ, quay lại trang đăng ký với thông báo lỗi.
            return View(model);
        }


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
