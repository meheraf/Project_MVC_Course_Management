using Course_Management.DataAccessLayer;
using Course_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    public class EnrollController : Controller
    {
        TrainingDB db = new TrainingDB();

        // GET: Enroll
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Create(int bId,int tId)
        {
            var trainee = db.Trainees.FirstOrDefault(x => x.TraineeId == tId);
            var batch = db.Batches.FirstOrDefault(x => x.BatchId == bId);
            db.Enrollments.Add(new Enrollment { BatchId = bId, TraineeId = tId });
            db.SaveChanges();
            return RedirectToAction("Index","Course");
        }
    }
}