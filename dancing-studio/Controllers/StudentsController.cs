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
        private readonly StudioContext _dbContext = new StudioContext();

        // GET: Students
        public ActionResult Index()
        {
            return View(_dbContext.Students.Include(x => x.Groups).OrderBy(x => x.Name).ToList());
        }

        // GET: Students/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var student = _dbContext
                .Students
                .Include(x => x.Groups)
                .SingleOrDefault(x => x.Id == id);

            ViewBag.Groups = _dbContext
                .Groups
                .OrderBy(x => x.Name)
                .Include(x => x.Teacher)
                .ToList();

            if (student == null)
                return HttpNotFound();
            
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.Groups = _dbContext.Groups.OrderBy(x => x.Name).Include(x => x.Teacher).ToList();
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student, int[] selectedGroups)
        {
            if (!ModelState.IsValid) 
                return View(student);
            
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();

            var newStudent = _dbContext.Students.Find(student.Id) ?? new Student();
            newStudent.Name = student.Name;
            newStudent.PhoneNumber = student.PhoneNumber;
            newStudent.Birthday = student.Birthday;
            newStudent.Info = student.Info ?? "-";

            newStudent.Groups.Clear();
            if (selectedGroups != null)
            {
                var groupsToAdd = _dbContext
                    .Groups
                    .Where(gr => selectedGroups.Contains(gr.Id));
                foreach (var g in groupsToAdd)
                    newStudent.Groups.Add(g);
            }                

            _dbContext.Entry(newStudent).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var student = _dbContext.Students.Find(id);
            if (student == null)
                return HttpNotFound();
            
            ViewBag.Groups = _dbContext.Groups.OrderBy(x => x.Name).Include(x => x.Teacher).ToList();
            return View(student);
        }

        // POST: Students/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student, int[] selectedGroups)
        {
            if (!ModelState.IsValid) 
                return View(student);
            
            var newStudent = _dbContext.Students.Find(student.Id) ?? new Student();
            newStudent.Name = student.Name;
            newStudent.PhoneNumber = student.PhoneNumber;
            newStudent.Birthday = student.Birthday;
            newStudent.Info = student.Info == null ? "-" : student.Info;

            newStudent.Groups.Clear();
            if (selectedGroups != null)
                foreach (var g in _dbContext.Groups.Where(gr => selectedGroups.Contains(gr.Id)))
                    newStudent.Groups.Add(g);

            _dbContext.Entry(newStudent).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var student = _dbContext.Students.Include(x => x.Groups).SingleOrDefault(x => x.Id == id);
            ViewBag.Groups = _dbContext.Groups.OrderBy(x => x.Name).Include(x => x.Teacher).ToList();
            if (student == null)
                return HttpNotFound();
            
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var student = _dbContext.Students.Find(id);
            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _dbContext.Dispose();
            
            base.Dispose(disposing);
        }
    }
}
