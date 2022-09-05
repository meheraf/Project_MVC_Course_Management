using Course_Management.DataAccessLayer;
using Course_Management.Models;
using Course_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    public class CourseController : Controller
    {
        TrainingDB db = new TrainingDB();
        // GET: Course
        public ActionResult Index()
        {
            
            IEnumerable<Course> course = db.Courses.Include("Category").Include("Batches").ToList();
            return View(course);
        }

        public ActionResult Details(int id)
        {
            Course c = db.Courses.Include("Category").FirstOrDefault(x => x.CourseId == id);
            return View(c);
        }


        public ActionResult Create()
        {
            ViewBag.categories = new SelectList(db.Categories, "CategoryId", "CategoryTitle");
            return View();
        }

        [HttpPost]
        public ActionResult Create(CourseViewModel cvm)
        {

            if (ModelState.IsValid)
            {
                if (cvm.Photo!=null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(cvm.Photo.FileName);
                    string filepath = Path.Combine("Images", "CourseImages", filename);
                    cvm.Photo.SaveAs(Server.MapPath("~/" + filepath));
                    Course course = new Course
                    {
                        CourseTitle = cvm.CourseTitle,
                        Overview = cvm.Overview,
                        CategoryId = cvm.CategoryId,
                        Duration = cvm.Duration,
                        Quizes = cvm.Quizes,
                        Assesments = cvm.Assesments,
                        Cost = cvm.Cost,
                        Thumbnail = filepath
                    };
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View();
        }
    }
}