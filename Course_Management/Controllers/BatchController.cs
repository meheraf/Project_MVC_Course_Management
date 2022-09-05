using Course_Management.DataAccessLayer;
using Course_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    public class BatchController : Controller
    {
        TrainingDB db = new TrainingDB();
        // GET: Batch
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Create()
        {
            ViewBag.trainers =new SelectList(db.Trainers, "TrainerId", "Name");
            ViewBag.courses = new SelectList(db.Courses, "CourseId", "CourseTitle");

            return View();
        }


        [HttpPost]
        public ActionResult Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                db.Batches.Add(batch);
                db.SaveChanges();
            }
            ViewBag.trainers = new SelectList(db.Trainers, "TrainerId", "Name");
            ViewBag.courses = new SelectList(db.Courses, "CourseId", "CourseTitle");
            return View(batch);
        }
    }
}