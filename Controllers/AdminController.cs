using Gigashop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gigashop.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AdminController(ApplicationDBContext context)
        {
            _context = context;
        }
        public IActionResult MainPage_Admin()
        {
            // Kiểm tra quyền Admin
            if (HttpContext.Session.GetString("Role") != "admin")
            {
                return RedirectToAction("Login", "Auth");
            }
            // Lấy dữ liệu thống kê
            var totalUsers = _context.Users.Count();
            var totalOrders = _context.Checkouts.Count();
            var totalProducts = _context.Products.Count();
            var totalRevenue = _context.Checkouts.Sum(c => c.TotalAmount);

            // Truyền dữ liệu qua ViewBag
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalRevenue = totalRevenue;

            // Lấy hoạt động gần đây (nếu cần)
            ViewBag.AdminActivities = _context.AdminActivities // Đảm bảo có bảng AdminActivities
                .OrderByDescending(a => a.CreatedAt)
                .Take(10)
                .ToList();

            return View();
        }
        public IActionResult AccountManagement()
        {
            var users = _context.Users.ToList();
            ViewBag.Users = users;
            return View();
        }
        // Block người dùng
        public IActionResult BlockUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user != null)
            {
                user.Status = "Blocked";
                _context.SaveChanges();

                // Lưu hoạt động
                var activity = new AdminActivity
                {
                    Action = "Chặn tài khoản",
                    DataType = "Admin",
                    AdminUsername = user.Username, // Ghi tên người dùng đã đăng ký
                    CreatedAt = DateTime.Now
                };

                _context.AdminActivities.Add(activity);
                _context.SaveChanges();
            }
            return RedirectToAction("AccountManagement");
        }
        // Unblock người dùng
        public IActionResult UnblockUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user != null)
            {
                user.Status = "Active";
                _context.SaveChanges();

                // Lưu hoạt động
                var activity = new AdminActivity
                {
                    Action = "Gỡ chặn tài khoản",
                    DataType = "Admin",
                    AdminUsername = user.Username, // Ghi tên người dùng đã đăng ký
                    CreatedAt = DateTime.Now
                };

                _context.AdminActivities.Add(activity);
                _context.SaveChanges();
            }
            return RedirectToAction("AccountManagement");
        }

        // Delete người dùng
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();

                // Lưu hoạt động
                var activity = new AdminActivity
                {
                    Action = "Xóa tài khoản",
                    DataType = "Admin",
                    AdminUsername = user.Username, // Ghi tên người dùng đã đăng ký
                    CreatedAt = DateTime.Now
                };

                _context.AdminActivities.Add(activity);
                _context.SaveChanges();
            }
            return RedirectToAction("AccountManagement");
        }

        // GET: Comment Management - Hiển thị tất cả review
        public async Task<IActionResult> Comment_Management()
        {
            // Include User and Product data for display
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .ToListAsync();

            ViewBag.Reviews = reviews;
            return View("Comment_Management");
        }

        // DELETE: Delete a review by ID
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id)
        {
            // Tìm đánh giá theo ID
            var review = await _context.Reviews
                .Include(r => r.User) // Nếu cần lấy thông tin người dùng liên quan
                .Include(r => r.Product) // Nếu cần lấy thông tin sản phẩm liên quan
                .FirstOrDefaultAsync(r => r.ReviewID == id);
            if(review != null)
            {
                // Xóa đánh giá
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            // Lưu hoạt động (xóa đánh giá)
            var activity = new AdminActivity
            {
                Action = "Xóa đánh giá",
                DataType = "Admin",
                AdminUsername = review.Comment, // Tên quản trị viên (nếu có)
                CreatedAt = DateTime.Now
            };

            _context.AdminActivities.Add(activity);
            await _context.SaveChangesAsync();
            }    
            return View("Comment_Management");
        }
        // Hiển thị danh sách tin nhắn
        public async Task<IActionResult> Message_Management()
        {
            // Include User data for display
            var messages = await _context.ContactMessages
                .Include(m => m.User) // Nếu bạn muốn hiển thị thông tin người dùng liên quan
                .OrderByDescending(m => m.CreatedAt) // Sắp xếp theo thời gian gửi mới nhất
                .ToListAsync();

            ViewBag.Messages = messages; // Truyền dữ liệu vào ViewBag
            return View("Message_Management");
        }

        // Xóa tin nhắn
        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            // Tìm tin nhắn theo ID
            var message = await _context.ContactMessages
                .Include(m => m.User) // Nếu cần lấy thông tin người dùng liên quan
                .FirstOrDefaultAsync(m => m.MessageID == id);
            if(message != null)
            {
                // Xóa tin nhắn
                _context.ContactMessages.Remove(message);
                await _context.SaveChangesAsync();

                // Lưu hoạt động (xóa tin nhắn)
                var activity = new AdminActivity
                {
                    Action = "Xóa tin nhắn",
                    DataType = "Tin nhắn",
                    AdminUsername = message.Message, // Tên quản trị viên (nếu có)
                    CreatedAt = DateTime.Now
                };

                _context.AdminActivities.Add(activity);
                await _context.SaveChangesAsync();
            }    

            return View("Message_Management");
        }
    }
}
