using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using PagedList;
using System.Web;
using System.Web.Mvc;
using School_Manager.Service;
using School_Manager.Models;

namespace School_Manager.Controllers
{
    public class StudentsController : Controller
    {
        private managerSchoolEntities db = new managerSchoolEntities();

        // GET: Students
        
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
           
            ViewBag.CurrentSort = sortOrder;

            //Sort 
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            //Paging 
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //Searching Name or class of Student
            var students = db.Students.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString)
                                       || s.Class.Name.Contains(searchString));
            }

            //sort name, class, age of Student
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.Name);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.Age);
                    break;
                default:
                    students = students.OrderBy(s => s.Age);
                    break;
                case "class_desc":
                    students = students.OrderByDescending(s => s.Class.Name);
                    break;
            }

            //pageSize: so luong Student trong 1 page
            //PageNumber: so luong page
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));

        }


        // GET: Students/Details/5
        
        public ActionResult Details(int? id)
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

            ScoreService service = new ScoreService(student, db);
            // service.CalculatorScore();
            StudentSubjectScore studentSubjectScore = service.CalculatorScore();

            return View(studentSubjectScore);
        }
        
        [HttpPost]
        [Authorize(Roles = "AdminManager")]
        public ActionResult UpdateScore(List<Score> listScoreUpdate, int studentId, int subjectId)
        {          
            try {

                Student student = db.Students.Find(studentId);
                if (student == null)
                {
                    return HttpNotFound();
                }
                ScoreService service = new ScoreService(student, db);
                // service.CalculatorScore();
                StudentSubjectScore studentSubjectScore = service.CalculatorScore();
                for (int i = 0; i < listScoreUpdate.Count; i++)
                {
                    ScoreUpdateService scoreUpdateService = new ScoreUpdateService(db);
                    scoreUpdateService.UpdateScore(listScoreUpdate[i].ID, listScoreUpdate[i].ScoreNumber);
                    Console.WriteLine(listScoreUpdate[i]);
                }
                // lay ra studentSubjectScore -> scoreCalculators -> finalScore
                double finalScore = studentSubjectScore.scoreCalculators.FirstOrDefault<ScoreCalculator>(x => x.subject.ID == subjectId).finalScore;
                return Json(new {status = "Success", finalScore = finalScore });
            }
            catch (Exception e) {
                return Json("Fail: " + e.ToString());
            }

        }

        // GET: Students/Create
        [Authorize(Roles = "AdminManager,Teacher")]
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.Classes, "ID", "Name");
            return View();
        }

        // POST: Students/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Age,ClassID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.Classes, "ID", "Name", student.ClassID);
            return View(student);
        }

        // GET: Students/Edit/5

        //[Authorize(Roles = "1")]
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
            ViewBag.ClassID = new SelectList(db.Classes, "ID", "Name", student.ClassID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Age,ClassID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ID", "Name", student.ClassID);
            return View(student);
        }

        // GET: Students/Delete/5
     
        public ActionResult Delete(int? id)
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
