﻿using Gigashop.Data;
using Gigashop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

            // Kiểm tra trạng thái người dùng
            if (user.Status != "Active")
            {
                ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa hoặc chưa được kích hoạt.");
                Console.WriteLine($"Đăng nhập thất bại: Tài khoản {user.Username} có trạng thái {user.Status}.");
                return View(model);
            }

            // Lưu thông tin vào Session
            HttpContext.Session.SetString("UserId", user.UserID.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role); // Lưu vai trò vào session

            // Lưu thông tin vào Cookie (vĩnh viễn)
            Response.Cookies.Append("UserId", user.UserID.ToString(), new CookieOptions { Expires = DateTime.Now.AddYears(1), HttpOnly = true });
            Response.Cookies.Append("Username", user.Username, new CookieOptions { Expires = DateTime.Now.AddYears(1), HttpOnly = true });
            Response.Cookies.Append("Role", user.Role, new CookieOptions { Expires = DateTime.Now.AddYears(1), HttpOnly = true });

            Console.WriteLine($"Đăng nhập thành công: {user.Username} với vai trò {user.Role}");

            // Kiểm tra vai trò của người dùng và chuyển hướng
            if (user.Role == "admin")
            {
                return RedirectToAction("MainPage_Admin", "Admin"); // Chuyển hướng đến trang admin
            }
            else if (user.Role == "User")
            {
                return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang user
            }
            else
            {
                // Nếu người dùng không có vai trò hợp lệ
                ModelState.AddModelError("", "Vai trò người dùng không hợp lệ.");
                Console.WriteLine("Đăng nhập thất bại: Vai trò người dùng không hợp lệ.");
                return View(model);
            }
        }


        public IActionResult Logout()
        {
            // Xóa session
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Username");

            // Xóa cookie
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("Username");

            Console.WriteLine("Đăng xuất thành công.");
            return RedirectToAction("Login", "Auth");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                username = Request.Cookies["Username"];
            }

            ViewBag.Username = username;
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

                    // Ghi log hoạt động vào bảng AdminActivities
                    var activity = new AdminActivity
                    {
                        Action = "Đăng ký tài khoản",
                        DataType = "Người dùng",
                        AdminUsername = model.Username, // Ghi tên người dùng đã đăng ký
                        CreatedAt = DateTime.Now
                    };

                    _context.AdminActivities.Add(activity);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Đăng ký thành công!";
                    Console.WriteLine($"Người dùng mới: {user.Username} đã được thêm vào cơ sở dữ liệu.");


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
