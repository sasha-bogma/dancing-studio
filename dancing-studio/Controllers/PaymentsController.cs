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
        private StudioContext db = new StudioContext();

        // GET: Payments
        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var student = db.Students.Find(id);
            if (student == null)
                return HttpNotFound();
            
            ViewBag.Student = student;
            var payments = db.Payments.Where(x => x.StudentId == id).Include(p => p.Student).OrderBy(p => p.Date);
            return View(payments.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var payment = db.Payments.Find(id);
            if (payment == null)
                return HttpNotFound();
            
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        
            var student = db.Students.Find(id);
            if (student == null)
                return HttpNotFound();
            
            
            double give = 0;
            if (db.Payments.Count(p => p.StudentId == id) != 0)
                give = db.Payments.Where(p => p.StudentId == id).Sum(p => p.Amount);

            double spend = 0;
            if (db.Lessons.Include(l => l.Presents).Count(l => l.Presents.FirstOrDefault(p => p.StudentId == id && p.Condition != Present.Presence.AbsenceValid) != null) != 0)
                spend = db.Lessons.Include(l => l.Presents).Where(l => l.Presents.FirstOrDefault(p => p.StudentId == id && p.Condition != Present.Presence.AbsenceValid) != null).Sum(l => l.Price);
            
            ViewBag.Bal = give - spend;
            ViewBag.Student = student;
            ViewBag.Presences = db.Presences.Where(x => x.StudentId == id).Include(p => p.Lesson).OrderBy(p => p.Lesson.DateTime);
            ViewBag.Payments = db.Payments.Where(x => x.StudentId == id).Include(p => p.Student).OrderBy(p => p.Date);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", db.Students.Find(id).Id);
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
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("Create", new { id = payment.StudentId });
            }

            ViewBag.Student = db.Students.Find(payment.StudentId);
            ViewBag.Presences = db.Presences.Where(x => x.StudentId == payment.StudentId).Include(p => p.Lesson).OrderBy(p => p.Lesson.DateTime);
            ViewBag.Payments = db.Payments.Where(x => x.StudentId == payment.StudentId).Include(p => p.Student).OrderBy(p => p.Date);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", payment.StudentId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var payment = db.Payments.Find(id);
            if (payment == null)
                return HttpNotFound();
            
            ViewBag.Student = db.Students.Find(payment.StudentId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", payment.StudentId);
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
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", new {id = payment.StudentId});
            }
            ViewBag.Student = db.Students.Find(payment.StudentId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", payment.StudentId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var payment = db.Payments.Include(x => x.Student).SingleOrDefault(x => x.Id == id);
            if (payment == null)
                return HttpNotFound();
            
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var payment = db.Payments.Find(id);
            var studId = payment?.StudentId;
            db.Payments.Remove(payment);
            db.SaveChanges();
            return RedirectToAction("Create", new { id = studId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
