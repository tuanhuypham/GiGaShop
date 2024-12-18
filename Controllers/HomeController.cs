using Gigashop.Data;
using Gigashop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        public async Task<IActionResult> AddToCart(CartRequest request)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Bạn phải đăng nhập để thêm sản phẩm vào giỏ hàng.");
            }

            // Tìm sản phẩm theo ProductID
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng của người dùng chưa
            var orderDetail = await _context.OrderDetails
                .Where(od => od.UserID == userId && od.ProductID == request.ProductId)
                .FirstOrDefaultAsync();

            if (orderDetail != null)
            {
                // Nếu sản phẩm đã có trong giỏ hàng, cộng thêm số lượng
                orderDetail.Quantity += request.Quantity;
                orderDetail.Total = orderDetail.Quantity * product.Price;
                _context.OrderDetails.Update(orderDetail);
            }
            else
            {
                // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới vào OrderDetail
                orderDetail = new OrderDetail
                {
                    UserID = userId,
                    ProductID = request.ProductId,
                    Quantity = request.Quantity,
                    UnitPrice = product.Price,
                    Total = request.Quantity * product.Price
                };

                _context.OrderDetails.Add(orderDetail);
            }

            await _context.SaveChangesAsync();

            return Ok("Sản phẩm đã được thêm vào giỏ hàng.");
        }




        private int GetOrCreateOrder(int userId)
        {
            // Kiểm tra xem người dùng có đơn hàng chưa
            var order = _context.Orders.FirstOrDefault(o => o.UserID == userId && o.Status == "Pending");
            if (order == null)
            {
                order = new Order
                {
                    UserID = userId,
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            return order.OrderID;
        }


        public class AddToCartModel
        {
            public int ProductID { get; set; }
            public int Quantity { get; set; }
        }

        public class CartRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetCartItems()
        {
            var sessionUser = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(sessionUser))
            {
                return Unauthorized("Bạn phải đăng nhập để xem giỏ hàng.");
            }

            int userId = int.Parse(sessionUser);
            var cartItems = await _context.OrderDetails
                                          .Where(o => o.OrderID == 0) // Giả định OrderID = 0 là giỏ hàng chưa thanh toán
                                          .Include(o => o.Product)
                                          .ToListAsync();

            return Ok(cartItems);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Kiểm tra session hoặc cookie
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                username = Request.Cookies["Username"];
            }

            // Lưu thông tin vào ViewBag
            ViewBag.Username = username;
        }
        public ActionResult Index()
        {
            return View();
        }public ActionResult accessories()
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

        public IActionResult Details(int id)
        {
            //var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
            //if (product == null)
            //{
            //    return NotFound();
            //}
            return View();
        }

        public IActionResult ShopingCart()
        {
            return View();
        }

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

    }
}
