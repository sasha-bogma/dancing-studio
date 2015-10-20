using dancing_studio.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dancing_studio.Controllers
{
    public class HomeController : Controller
    {
        StudioContext db = new StudioContext();

        public ActionResult Index()
        {
            db.Teachers.Add(new Teacher("Петрова Инна Сергеевна"));
            db.SaveChanges();
            return View();
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