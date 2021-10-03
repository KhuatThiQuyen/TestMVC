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
    public class ScoreTypesController : Controller
    {
        private managerSchoolEntities db = new managerSchoolEntities();

        // GET: ScoreTypes
        public ActionResult Index()
        {
            return View(db.ScoreTypes.ToList());
        }

        // GET: ScoreTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreType scoreType = db.ScoreTypes.Find(id);
            if (scoreType == null)
            {
                return HttpNotFound();
            }
            return View(scoreType);
        }

        // GET: ScoreTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScoreTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] ScoreType scoreType)
        {
            if (ModelState.IsValid)
            {
                db.ScoreTypes.Add(scoreType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoreType);
        }

        // GET: ScoreTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreType scoreType = db.ScoreTypes.Find(id);
            if (scoreType == null)
            {
                return HttpNotFound();
            }
            return View(scoreType);
        }

        // POST: ScoreTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] ScoreType scoreType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scoreType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoreType);
        }

        // GET: ScoreTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreType scoreType = db.ScoreTypes.Find(id);
            if (scoreType == null)
            {
                return HttpNotFound();
            }
            return View(scoreType);
        }

        // POST: ScoreTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoreType scoreType = db.ScoreTypes.Find(id);
            db.ScoreTypes.Remove(scoreType);
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
