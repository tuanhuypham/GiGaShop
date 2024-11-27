using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Gigashop.Models;

namespace Gigashop.Controllers
{
    public class HomeController : Controller
    {
        private readonly GigashopEntities1 db;

        public HomeController()
        {
            db = new GigashopEntities1();
        }
        public ActionResult Index()
        {
            var products = db.Products;

            return View(products); // Truyền danh sách sản phẩm đến view
           
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}