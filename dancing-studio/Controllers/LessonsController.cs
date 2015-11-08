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
        private StudioContext db = new StudioContext();

        public ActionResult ChooseGroup()
        {
            List<Group> groups = new List<Group>(db.Groups.Include(l => l.Teacher).OrderBy(l => l.Name));
            return View(groups);
        }

        // GET: Lessons
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.groupId = id;
            ViewBag.grName = db.Groups.Find(id).Name + " - " + db.Groups.Include(x => x.Teacher).SingleOrDefault(x => x.Id ==id).Teacher.Name;

            ViewBag.Dates = db.Lessons.Where(x => x.GroupId == id).OrderBy(x => x.DateTime).Select(x => x.DateTime ).ToList();
            ViewBag.Students = db.Groups.Find(id).Students.OrderBy(x => x.Name).Select(x => new { x.Id, x.Name }).ToList();
            List<Present> pres = db.Presences.Include(x => x.Student).Include(x => x.Lesson).Where(x => x.Lesson.GroupId == id).OrderBy(x => x.Lesson.DateTime).ThenBy(x => x.Student.Name).ToList();
            ViewBag.Presences = pres;

            var lessons = db.Lessons.Include(l => l.Group).Include(l => l.Teacher).Where(l => l.GroupId == id).OrderBy(l => l.DateTime);
            return View(lessons.ToList());
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Include(x => x.Teacher).Include(x => x.Group).SingleOrDefault(x => x.Id == id);
            ViewBag.gr = lesson.GroupId;
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // GET: Lessons/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Gr = id;
            ViewBag.GrName = db.Groups.Find(id).Name;
            ViewBag.GroupId = new SelectList(db.Groups.Where(x => x.Id == id), "Id", "Name", db.Groups.Find(id).Id);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", db.Groups.Find(id).TeacherId);
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
                db.Lessons.Add(lesson);
                db.SaveChanges();

                foreach (Student s in db.Groups.Include(x => x.Students).SingleOrDefault(x => x.Id == lesson.GroupId).Students)
                {
                    db.Presences.Add(new Present { StudentId = s.Id, LessonId = lesson.Id, Condition = Present.Presence.Present});
                    db.SaveChanges();
                }
                return RedirectToAction("Index", new {id = lesson.GroupId });
            }

            ViewBag.Gr = lesson.GroupId;
            ViewBag.GrName = db.Groups.Find(lesson.GroupId).Name;

            ViewBag.GroupId = new SelectList(db.Groups.Where(x => x.Id == lesson.GroupId), "Id", "Name");
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", lesson.TeacherId);
            return View(lesson);
        }


        // отсутствующие
        public ActionResult EditPresents(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            List<Present> presents = db.Presences.Where(x => x.LessonId == id).Include(x => x.Student).Include(x => x.Lesson).OrderBy(x => x.Student.Name).ToList();

            ViewBag.gr = lesson.GroupId;

            return View(presents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPresents( List<Present> presents)
        {
            if (ModelState.IsValid)
            {
                foreach (Present p in presents)
                {
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", new { id = db.Lessons.Find(presents[0].LessonId).GroupId});
            }

            return View(presents);

        }
        

        // GET: Lessons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Include(x => x.Group).SingleOrDefault(x => x.Id == id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.gr = lesson.GroupId;
            ViewBag.grName = lesson.Group.Name;
            ViewBag.GroupId = new SelectList(db.Groups.Where(x => x.Id == lesson.GroupId), "Id", "Name");
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", lesson.TeacherId);
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
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = lesson.GroupId });
            }

            ViewBag.gr = lesson.GroupId;
            ViewBag.grName = lesson.Group.Name;

            ViewBag.GroupId = new SelectList(db.Groups.Where(x => x.Id == lesson.GroupId), "Id", "Name");
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", lesson.TeacherId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Include(x => x.Teacher).Include(x => x.Group).SingleOrDefault(x => x.Id == id);
            ViewBag.gr = lesson.GroupId;
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson lesson = db.Lessons.Find(id);
            int gr = lesson.GroupId;

            foreach (Present p in db.Presences.Where(x => x.LessonId == id).ToList())
            {
                db.Presences.Remove(p);
                db.SaveChanges();
            }

            db.Lessons.Remove(lesson);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = gr });
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
