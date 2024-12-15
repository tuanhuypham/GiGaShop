using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gigashop.Views
{
    public class HomeController : Controller
    {
        // GET: HomeController
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

        public IActionResult Details()
        {
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
