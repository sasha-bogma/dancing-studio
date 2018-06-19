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
        private readonly StudioContext _dbContext = new StudioContext();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.TodayBirthdayStudents = _dbContext
                .Students
                .Where(x => x.Birthday.Value.Month == DateTime.Today.Month 
                            && x.Birthday.Value.Day == DateTime.Today.Day)
                .OrderBy(x => x.Name)
                .ToList();
            
            var fromDate = DateTime.Today.AddDays(1);
            var toDate = fromDate.AddDays(7);
            
            ViewBag.NearestBirthDays = _dbContext
                .Students
                .Where(x => IsDayBetween(fromDate.Month,
                                        fromDate.Day,
                                        toDate.Month,
                                        toDate.Day,
                                        x.Birthday.Value))
                .OrderBy(x => x.Birthday)
                .ToList();

            return View();
        }

        private bool IsDayBetween(int fromMonth, 
                                int fromDay, 
                                int toMonth, 
                                int toDay, 
                                DateTime toCheck)
        {
            return toCheck.Month >= fromMonth && toCheck.Day >= fromDay 
                && toCheck.Month <= toMonth && toCheck.Day <= toDay;
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