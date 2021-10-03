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
    public class UserStatusController : Controller
    {
        private managerSchoolAccount db = new managerSchoolAccount();

        // GET: UserStatus
        public ActionResult Index()
        {
            return View(db.UserStatus.ToList());
        }

        // GET: UserStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStatu userStatu = db.UserStatus.Find(id);
            if (userStatu == null)
            {
                return HttpNotFound();
            }
            return View(userStatu);
        }

        // GET: UserStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] UserStatu userStatu)
        {
            if (ModelState.IsValid)
            {
                db.UserStatus.Add(userStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userStatu);
        }

        // GET: UserStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStatu userStatu = db.UserStatus.Find(id);
            if (userStatu == null)
            {
                return HttpNotFound();
            }
            return View(userStatu);
        }

        // POST: UserStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] UserStatu userStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userStatu);
        }

        // GET: UserStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStatu userStatu = db.UserStatus.Find(id);
            if (userStatu == null)
            {
                return HttpNotFound();
            }
            return View(userStatu);
        }

        // POST: UserStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserStatu userStatu = db.UserStatus.Find(id);
            db.UserStatus.Remove(userStatu);
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
