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
        [HttpGet("list")]
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
