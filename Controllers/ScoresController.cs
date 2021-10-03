using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using School_Manager.Models;

namespace School_Manager.Controllers
{
    public class ScoresController : Controller
    {
        private managerSchoolEntities db = new managerSchoolEntities();

        // GET: Scores
        public ActionResult Index()
        {
            var scores = db.Scores.Include(s => s.ScoreType).Include(s => s.Student).Include(s => s.Subject);
            return View(scores.ToList());
        }

        // GET: Scores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Score score = db.Scores.Find(id);
            if (score == null)
            {
                return HttpNotFound();
            }
            return View(score);
        }

        // GET: Scores/Create
        public ActionResult Create()
        {
            ViewBag.ScoreTypeID = new SelectList(db.ScoreTypes, "ID", "Name");
            ViewBag.StudentID = new SelectList(db.Students, "ID", "Name");
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Name");
            return View();
        }

        // POST: Scores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ScoreNumber,StudentID,SubjectID,ScoreTypeID")] Score score)
        {
            if (ModelState.IsValid)
            {
                db.Scores.Add(score);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ScoreTypeID = new SelectList(db.ScoreTypes, "ID", "Name", score.ScoreTypeID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "Name", score.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Name", score.SubjectID);
            return View(score);
        }

        // GET: Scores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Score score = db.Scores.Find(id);
            if (score == null)
            {
                return HttpNotFound();
            }
            ViewBag.ScoreTypeID = new SelectList(db.ScoreTypes, "ID", "Name", score.ScoreTypeID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "Name", score.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Name", score.SubjectID);
            return View(score);
        }

        // POST: Scores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ScoreNumber,StudentID,SubjectID,ScoreTypeID")] Score score)
        {
            if (ModelState.IsValid)
            {
                db.Entry(score).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ScoreTypeID = new SelectList(db.ScoreTypes, "ID", "Name", score.ScoreTypeID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "Name", score.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Name", score.SubjectID);
            return View(score);
        }

        // GET: Scores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Score score = db.Scores.Find(id);
            if (score == null)
            {
                return HttpNotFound();
            }
            return View(score);
        }

        // POST: Scores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Score score = db.Scores.Find(id);
            db.Scores.Remove(score);
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
