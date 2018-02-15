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
    public class GroupsController : Controller
    {
        private readonly StudioContext _dbContext = new StudioContext();

        // GET: Groups
        public ActionResult Index()
        {
            var groups = _dbContext.Groups.Include(g => g.Teacher);
            return View(groups.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var group = _dbContext.Groups.Include(g => g.Teacher).SingleOrDefault(x => x.Id == id);
            if (group == null)
                return HttpNotFound();
            
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            ViewBag.TeacherId = new SelectList(_dbContext.Teachers, "Id", "Name");
            return View();
        }

        // POST: Groups/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TeacherId,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Groups.Add(group);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeacherId = new SelectList(_dbContext.Teachers, "Id", "Name", group.TeacherId);
            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var group = _dbContext.Groups.Find(id);
            if (group == null)
                return HttpNotFound();
            
            ViewBag.TeacherId = new SelectList(_dbContext.Teachers, "Id", "Name", group.TeacherId);
            return View(group);
        }

        // POST: Groups/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TeacherId,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(group).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeacherId = new SelectList(_dbContext.Teachers, "Id", "Name", group.TeacherId);
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var group = _dbContext.Groups.Include(x => x.Teacher).SingleOrDefault(x => x.Id == id);
            if (group == null)
                return HttpNotFound();
            
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var group = _dbContext.Groups.Find(id);
            _dbContext.Groups.Remove(group);
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
