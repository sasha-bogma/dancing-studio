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
    public class StudentsController : Controller
    {
        private StudioContext db = new StudioContext();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.Include(x => x.Groups).OrderBy(x => x.Name).ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Include(x => x.Groups).SingleOrDefault(x => x.Id == id);
            ViewBag.Groups = db.Groups.OrderBy(x => x.Name).Include(x => x.Teacher).ToList();
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.Groups = db.Groups.OrderBy(x => x.Name).Include(x => x.Teacher).ToList();
            return View();
        }

        // POST: Students/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PhoneNumber,Birthday,Groups")] Student student, int[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();

                Student newStudent = db.Students.Find(student.Id);
                newStudent.Name = student.Name;
                newStudent.PhoneNumber = student.PhoneNumber;
                newStudent.Birthday = student.Birthday;

                newStudent.Groups.Clear();
                if (selectedGroups != null)
                {
                    foreach (var g in db.Groups.Where(gr => selectedGroups.Contains(gr.Id)))
                    {
                        newStudent.Groups.Add(g);
                    }
                }

                db.Entry(newStudent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Groups = db.Groups.OrderBy(x => x.Name).Include(x => x.Teacher).ToList();
            return View(student);
        }

        // POST: Students/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PhoneNumber,Birthday")] Student student, int[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                Student newStudent = db.Students.Find(student.Id);
                newStudent.Name = student.Name;
                newStudent.PhoneNumber = student.PhoneNumber;
                newStudent.Birthday = student.Birthday;

                newStudent.Groups.Clear();
                if (selectedGroups != null)
                {
                    foreach (var g in db.Groups.Where(gr => selectedGroups.Contains(gr.Id)))
                    {
                        newStudent.Groups.Add(g);
                    }
                }

                db.Entry(newStudent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Include(x => x.Groups).SingleOrDefault(x => x.Id == id);
            ViewBag.Groups = db.Groups.OrderBy(x => x.Name).Include(x => x.Teacher).ToList();
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
