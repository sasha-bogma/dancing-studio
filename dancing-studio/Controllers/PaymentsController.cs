using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using dancing_studio.DAL;

namespace dancing_studio.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly StudioContext _dbContext = new StudioContext();

        // GET: Payments
        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var student = _dbContext.Students.Find(id);
            if (student == null)
                return HttpNotFound();
            
            ViewBag.Student = student;
            var payments = _dbContext.Payments.Where(x => x.StudentId == id).Include(p => p.Student).OrderBy(p => p.Date);
            return View(payments.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var payment = _dbContext.Payments.Find(id);
            if (payment == null)
                return HttpNotFound();
            
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        
            var student = _dbContext.Students.Find(id);
            if (student == null)
                return HttpNotFound();
            
            
            double give = 0;
            if (_dbContext.Payments.Count(p => p.StudentId == id) != 0)
                give = _dbContext.Payments.Where(p => p.StudentId == id).Sum(p => p.Amount);

            double spend = 0;
            if (_dbContext.Lessons.Include(l => l.Presents).Count(l => l.Presents.FirstOrDefault(p => p.StudentId == id && p.Condition != Present.Presence.AbsenceValid) != null) != 0)
                spend = _dbContext.Lessons.Include(l => l.Presents).Where(l => l.Presents.FirstOrDefault(p => p.StudentId == id && p.Condition != Present.Presence.AbsenceValid) != null).Sum(l => l.Price);
            
            ViewBag.Bal = give - spend;
            ViewBag.Student = student;
            ViewBag.Presences = _dbContext.Presences.Where(x => x.StudentId == id).Include(p => p.Lesson).OrderBy(p => p.Lesson.DateTime);
            ViewBag.Payments = _dbContext.Payments.Where(x => x.StudentId == id).Include(p => p.Student).OrderBy(p => p.Date);
            ViewBag.StudentId = new SelectList(_dbContext.Students, "Id", "Name", _dbContext.Students.Find(id).Id);
            return View();
        }

        // POST: Payments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,Amount,Date")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Payments.Add(payment);
                _dbContext.SaveChanges();
                return RedirectToAction("Create", new { id = payment.StudentId });
            }

            ViewBag.Student = _dbContext.Students.Find(payment.StudentId);
            ViewBag.Presences = _dbContext.Presences.Where(x => x.StudentId == payment.StudentId).Include(p => p.Lesson).OrderBy(p => p.Lesson.DateTime);
            ViewBag.Payments = _dbContext.Payments.Where(x => x.StudentId == payment.StudentId).Include(p => p.Student).OrderBy(p => p.Date);
            ViewBag.StudentId = new SelectList(_dbContext.Students, "Id", "Name", payment.StudentId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var payment = _dbContext.Payments.Find(id);
            if (payment == null)
                return HttpNotFound();
            
            ViewBag.Student = _dbContext.Students.Find(payment.StudentId);
            ViewBag.StudentId = new SelectList(_dbContext.Students, "Id", "Name", payment.StudentId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,Amount,Date")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(payment).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Create", new {id = payment.StudentId});
            }
            ViewBag.Student = _dbContext.Students.Find(payment.StudentId);
            ViewBag.StudentId = new SelectList(_dbContext.Students, "Id", "Name", payment.StudentId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var payment = _dbContext.Payments.Include(x => x.Student).SingleOrDefault(x => x.Id == id);
            if (payment == null)
                return HttpNotFound();
            
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var payment = _dbContext.Payments.Find(id);
            var studId = payment.StudentId;
            _dbContext.Payments.Remove(payment);
            _dbContext.SaveChanges();
            return RedirectToAction("Create", new { id = studId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _dbContext.Dispose();

            base.Dispose(disposing);
        }
    }
}
