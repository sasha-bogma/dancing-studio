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
    public class LessonsController : Controller
    {
        private readonly StudioContext _dbContext = new StudioContext();

        public ActionResult ChooseGroup()
        {
            var groups = new List<Group>(_dbContext.Groups.Include(l => l.Teacher).OrderBy(l => l.Name));
            return View(groups);
        }

        // GET: Lessons
        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            ViewBag.groupId = id;
            ViewBag.grName = _dbContext.Groups.Find(id).Name + " - " + _dbContext.Groups.Include(x => x.Teacher).SingleOrDefault(x => x.Id ==id).Teacher.Name;

            ViewBag.Dates = _dbContext.Lessons.Where(x => x.GroupId == id).OrderBy(x => x.DateTime).Select(x => x.DateTime ).ToList();
            ViewBag.Students = _dbContext.Groups.Find(id).Students.OrderBy(x => x.Name).Select(x => new { x.Id, x.Name }).ToList();
            var pres = _dbContext.Presences.Include(x => x.Student).Include(x => x.Lesson).Where(x => x.Lesson.GroupId == id).OrderBy(x => x.Lesson.DateTime).ThenBy(x => x.Student.Name).ToList();
            ViewBag.Presences = pres;

            var lessons = _dbContext.Lessons.Include(l => l.Group).Include(l => l.Teacher).Where(l => l.GroupId == id).OrderBy(l => l.DateTime);
            return View(lessons.ToList());
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var lesson = _dbContext.Lessons.Include(x => x.Teacher).Include(x => x.Group).SingleOrDefault(x => x.Id == id);
            ViewBag.gr = lesson.GroupId;
            
            return View(lesson);
        }

        // GET: Lessons/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            ViewBag.Gr = id;
            ViewBag.GrName = _dbContext.Groups.Find(id).Name;
            ViewBag.GroupId = new SelectList(_dbContext.Groups.Where(x => x.Id == id), "Id", "Name", _dbContext.Groups.Find(id).Id);
            ViewBag.TeacherId = new SelectList(_dbContext.Teachers, "Id", "Name", _dbContext.Groups.Find(id).TeacherId);
            
            return View();
        }

        // POST: Lessons/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GroupId,TeacherId,DateTime,Price")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Lessons.Add(lesson);
                _dbContext.SaveChanges();

                foreach (var s in _dbContext.Groups.Include(x => x.Students).SingleOrDefault(x => x.Id == lesson.GroupId).Students)
                {
                    _dbContext.Presences.Add(new Present { StudentId = s.Id, LessonId = lesson.Id, Condition = Present.Presence.Present});
                    _dbContext.SaveChanges();
                }
                return RedirectToAction("Index", new {id = lesson.GroupId });
            }

            ViewBag.Gr = lesson.GroupId;
            ViewBag.GrName = _dbContext.Groups.Find(lesson.GroupId).Name;

            ViewBag.GroupId = new SelectList(_dbContext.Groups.Where(x => x.Id == lesson.GroupId), "Id", "Name");
            ViewBag.TeacherId = new SelectList(_dbContext.Teachers, "Id", "Name", lesson.TeacherId);
            
            return View(lesson);
        }


        // отсутствующие
        public ActionResult EditPresents(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var lesson = _dbContext.Lessons.Find(id);
            if (lesson == null)
                return HttpNotFound();
            
            var presents = _dbContext.Presences.Where(x => x.LessonId == id).Include(x => x.Student).Include(x => x.Lesson).OrderBy(x => x.Student.Name).ToList();

            ViewBag.gr = lesson.GroupId;

            return View(presents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPresents( List<Present> presents)
        {
            if (!ModelState.IsValid) 
                return View(presents);
            
            foreach (var p in presents)
            {
                _dbContext.Entry(p).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index", new { id = _dbContext.Lessons.Find(presents[0].LessonId).GroupId});

        }
        

        // GET: Lessons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var lesson = _dbContext.Lessons.Include(x => x.Group).SingleOrDefault(x => x.Id == id);
            if (lesson == null)
                return HttpNotFound();
            
            ViewBag.gr = lesson.GroupId;
            ViewBag.grName = lesson.Group.Name;
            ViewBag.GroupId = new SelectList(_dbContext.Groups.Where(x => x.Id == lesson.GroupId), "Id", "Name");
            ViewBag.TeacherId = new SelectList(_dbContext.Teachers, "Id", "Name", lesson.TeacherId);
            
            return View(lesson);
        }
        

        // POST: Lessons/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GroupId,TeacherId,DateTime,Price")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(lesson).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index", new { id = lesson.GroupId });
            }

            ViewBag.gr = lesson.GroupId;
            ViewBag.grName = lesson.Group.Name;

            ViewBag.GroupId = new SelectList(_dbContext.Groups.Where(x => x.Id == lesson.GroupId), "Id", "Name");
            ViewBag.TeacherId = new SelectList(_dbContext.Teachers, "Id", "Name", lesson.TeacherId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var lesson = _dbContext.Lessons.Include(x => x.Teacher).Include(x => x.Group).SingleOrDefault(x => x.Id == id);
            ViewBag.gr = lesson.GroupId;

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var lesson = _dbContext.Lessons.Find(id);
            var gr = lesson.GroupId;

            foreach (var p in _dbContext.Presences.Where(x => x.LessonId == id).ToList())
            {
                _dbContext.Presences.Remove(p);
                _dbContext.SaveChanges();
            }

            _dbContext.Lessons.Remove(lesson);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", new { id = gr });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _dbContext.Dispose();
            
            base.Dispose(disposing);
        }
    }
}
