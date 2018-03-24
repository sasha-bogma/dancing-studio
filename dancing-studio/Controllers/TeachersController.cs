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
    public class TeachersController : Controller
    {
        private readonly StudioContext _dbContext = new StudioContext();

        // GET: Teachers
        public ActionResult Index()
        {
            return View(_dbContext.Teachers.ToList());
        }

        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var teacher = _dbContext.Teachers.Find(id);
            if (teacher == null)
                return HttpNotFound();
            
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            ViewBag.Groups = _dbContext.Groups.OrderBy(x => x.Name).Include(x => x.Teacher).ToList();
            return View();
        }

        // POST: Teachers/Create [Bind(Include = "Id,Name,PhoneNumber,Birthday")] 
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Teacher teacher, int[] selectedGroups)
        {
            if (!ModelState.IsValid) 
                return View(teacher);
            
            _dbContext.Teachers.Add(teacher);
            _dbContext.SaveChanges();

            var newTeacher = _dbContext.Teachers.Find(teacher.Id) ?? new Teacher();
            newTeacher.Name = teacher.Name;
            newTeacher.PhoneNumber = teacher.PhoneNumber;
            newTeacher.Birthday = teacher.Birthday;

            if (selectedGroups != null)
                foreach (var g in _dbContext.Groups.Where(gr => selectedGroups.Contains(gr.Id)))
                    newTeacher.Groups.Add(g);

            _dbContext.Entry(newTeacher).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var teacher = _dbContext.Teachers.Find(id);
            if (teacher == null)
                return HttpNotFound();
            
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PhoneNumber,Birthday")] Teacher teacher)
        {
            if (!ModelState.IsValid) 
                return View(teacher);
            
            _dbContext.Entry(teacher).State = EntityState.Modified;
            _dbContext.SaveChanges();
            
            return RedirectToAction("Index");
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var teacher = _dbContext.Teachers.Find(id);
            if (teacher == null)
                return HttpNotFound();
            
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var teacher = _dbContext.Teachers.Find(id);
            
            _dbContext.Teachers.Remove(teacher);
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
