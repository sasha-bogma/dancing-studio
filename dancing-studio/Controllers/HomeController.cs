using dancing_studio.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dancing_studio.Controllers
{
    public class HomeController : Controller
    {
        StudioContext db = new StudioContext();

        private static bool IsLikeToday(DateTime? d)
        {
            if (d == null) return false;
            DateTime today = new DateTime(((DateTime)d).Year, DateTime.Today.Month, DateTime.Today.Day);
            return d == today;
        }

        private static bool IsBiggerThenToday(DateTime? d)
        {
            if (d == null) return false;
            DateTime today = new DateTime( ((DateTime)d).Year, DateTime.Today.Month, DateTime.Today.Day);
            return d > today;
        }

        public ActionResult Index()
        {
            var a = db.Students.Where(x => x.Birthday.Value.Month == DateTime.Today.Month && x.Birthday.Value.Day == DateTime.Today.Day).OrderBy(x => x.Name).ToList();
            ViewBag.TodayBirthdayStudents = a;

            List<Student> b = new List<Student>();
            DateTime currentDate = DateTime.Today.AddDays(1);
            while (currentDate != DateTime.Today.AddDays(7))
            {
                var students = db.Students.Where(x => x.Birthday.Value.Month == currentDate.Month && x.Birthday.Value.Day == currentDate.Day).OrderBy(x => x.Name).ToList();
                b.AddRange(students);
                currentDate = currentDate.AddDays(1);
            }
            ViewBag.NearestBirthDays = b;

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

        public ActionResult Help()
        {

            return Redirect("~/Content/index.html");
        }
    }
}