using Gigashop.Data;
using Gigashop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace Gigashop.Views
{
    public class HomeController : Controller
    {
        // GET: HomeController
        private readonly ApplicationDBContext _context;


        public HomeController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [HttpGet("list")]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Lấy thông tin từ Session nếu có
            var username = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserID");

            // Kiểm tra nếu thông tin session không tồn tại thì lấy từ cookie
            if (string.IsNullOrEmpty(username) && userId == null)
            {
                username = Request.Cookies["Username"];
                userId = int.TryParse(Request.Cookies["UserID"], out var id) ? (int?)id : null;
            }

            // Lưu thông tin vào ViewBag
            ViewBag.Username = username;
            ViewBag.UserID = userId;
        }


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult accessories()
        {
            return View();
        }
        public ActionResult listLaptop()
        {
            return View();
        }
        public ActionResult AccessoryPC()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult listPC()
        {
            return View();
        }
        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }


        public IActionResult Details()
        {

            return View();
        }

        public async Task<IActionResult> ShopingCart()
        {
            // Kiểm tra nếu cookie chứa UserID
            if (!Request.Cookies.TryGetValue("UserID", out var userIdStr) || string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập nếu UserID không tồn tại
            }

            // Chuyển UserID từ chuỗi sang số nguyên
            if (!int.TryParse(userIdStr, out var userId))
            {
                return BadRequest("Invalid UserID in cookie.");
            }

            // Lấy danh sách sản phẩm trong giỏ hàng của người dùng
            var cartItems = await _context.Carts
                .Where(c => c.UserID == userId)
                .Include(c => c.Product) // Bao gồm thông tin sản phẩm liên kết
                .ToListAsync();

            return View(cartItems); // Truyền danh sách vào View
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            var cartItem = await _context.Carts.FindAsync(cartId);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ShopingCart));
        }


        //public IActionResult ShopingCart()
        //{
        //    return View();
        //}

        public IActionResult CheckOut()
        {
            return View();
        }

        public IActionResult Service()
        {
            return View();
        }

        public IActionResult ServiceList()
        {
            return View();
        }


        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        public IActionResult MyAccount()
        {
            // Lấy thông tin từ Session nếu có
            var username = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetString("UserId");

            // Nếu thông tin không có trong Session, kiểm tra Cookie
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
            {
                username = Request.Cookies["Username"];
                userId = Request.Cookies["UserId"];
            }

            // Nếu không có thông tin người dùng, chuyển hướng về trang đăng nhập
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu dựa trên UserID
            var user = _context.Users.FirstOrDefault(u => u.UserID.ToString() == userId);

            // Kiểm tra nếu không tìm thấy người dùng trong cơ sở dữ liệu
            if (user == null)
            {
                return RedirectToAction("Index", "Home"); // Nếu không tìm thấy, chuyển về trang chủ
            }

            // Trả về View với thông tin người dùng
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MyAccount(User user)
        {

            try
            {
                // Kiểm tra UserID hợp lệ
                if (user.UserID <= 0)
                {
                    TempData["Error"] = "UserID không hợp lệ.";
                    return RedirectToAction("MyAccount");
                }

                // Lấy thông tin người dùng từ cơ sở dữ liệu
                var existingUser = _context.Users.SingleOrDefault(u => u.UserID == user.UserID);
                if (existingUser == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                    return RedirectToAction("MyAccount");
                }

                // Cập nhật thông tin
                existingUser.FullName = user.FullName;
                existingUser.Phone = user.Phone;
                existingUser.Address = user.Address;
                existingUser.Email = user.Email;

                // Xử lý mật khẩu mới (nếu có)
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    if (user.PasswordHash != Request.Form["ConfirmPassword"])
                    {
                        TempData["Error"] = "Mật khẩu và xác nhận mật khẩu không khớp.";
                        return View(user);
                    }

                    // Hash mật khẩu mới
                    existingUser.PasswordHash = HashPassword(user.PasswordHash);
                }

                // Lưu thay đổi
                _context.SaveChanges();

                // Cập nhật thông tin Session
                HttpContext.Session.SetInt32("UserID", existingUser.UserID);
                HttpContext.Session.SetString("Username", existingUser.Username);

                TempData["Message"] = "Thông tin đã được cập nhật thành công.";
                return RedirectToAction("MyAccount");
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết hơn nếu cần
                TempData["Error"] = "Đã xảy ra lỗi khi cập nhật: " + ex.Message;
                return View(user);
            }
        }




        // Mã hóa mật khẩu bằng SHA256
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/cart/add")]
        public IActionResult AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                // Lấy thông tin từ Session nếu có
                var username = HttpContext.Session.GetString("Username");
                var userId = HttpContext.Session.GetString("UserId");

                // Nếu thông tin không có trong Session, kiểm tra Cookie
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
                {
                    username = Request.Cookies["Username"];
                    userId = Request.Cookies["UserId"];
                }

                // Nếu không có thông tin người dùng, trả về lỗi
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
                {
                    return Json(new { message = "Người dùng chưa đăng nhập." });
                }

                // Chuyển đổi UserId từ string sang int
                int parsedUserId;
                if (!int.TryParse(userId, out parsedUserId))
                {
                    return Json(new { message = "Thông tin người dùng không hợp lệ." });
                }

                // Tìm sản phẩm theo ProductID
                var product = _context.Products.FirstOrDefault(p => p.ProductID == request.ProductID);
                if (product == null)
                    return Json(new { message = "Sản phẩm không tồn tại." });

                // Kiểm tra giỏ hàng hiện tại
                var cart = _context.Carts.FirstOrDefault(c => c.UserID == parsedUserId && c.ProductID == request.ProductID);
                if (cart == null)
                {
                    // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới
                    cart = new Cart
                    {
                        UserID = parsedUserId,
                        ProductID = request.ProductID,
                        Quantity = request.Quantity,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.Carts.Add(cart);
                }
                else
                {
                    // Nếu đã có trong giỏ hàng, cập nhật số lượng
                    cart.Quantity += request.Quantity;
                    cart.UpdatedAt = DateTime.Now;
                }

                // Lưu thay đổi vào DB
                _context.SaveChanges();

                return Json(new { message = "Đã thêm sản phẩm vào giỏ hàng thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { message = "Đã xảy ra lỗi khi thêm sản phẩm vào giỏ hàng." });
            }
        }


        public class AddToCartRequest
        {
            public int ProductID { get; set; } // ID sản phẩm
            public int Quantity { get; set; } // Số lượng
        }

    }
}