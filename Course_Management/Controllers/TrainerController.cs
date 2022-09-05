using Course_Management.DataAccessLayer;
using Course_Management.Models;
using Course_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    public class TrainerController : Controller
    {
        private TrainingDB db = new TrainingDB();
        // GET: Trainer
        public ActionResult Index()
        {
            return View(db.Trainers.Include("Skills").ToList());
        }


        public ActionResult Create()
        {
            ViewBag.skills = new SelectList(db.Skills, "SkillId", "SkillTitle");
            return View();
        }
        [HttpPost]
        public ActionResult Create(TrainerViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                if (tvm.Photo!=null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(tvm.Photo.FileName);
                    string filepath = Path.Combine("Images", "Trainer",filename);
                    string file = Path.Combine(Server.MapPath("~/"+filepath));
                    tvm.Photo.SaveAs(file);
                    Trainer trainer = new Trainer
                    {
                        Name = tvm.Name,
                        FatherName = tvm.FatherName,
                        Email = tvm.Email,
                        Gender = tvm.Gender,
                        BirthDate = tvm.BirthDate,
                        NID = tvm.NID,
                        Phone = tvm.Phone,
                        Address = tvm.Address,
                        PhotoPath = filepath
                    };
                    db.Trainers.Add(trainer);
                    db.SaveChanges();
                    int id = trainer.TrainerId;
                    foreach (var skillid in tvm.SkillIds)
                    {
                        Skill skill = db.Skills.FirstOrDefault(x => x.SkillId == skillid);
                        db.Trainers.Include("Skills").FirstOrDefault(x => x.TrainerId == id).Skills.Add(skill);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(tvm);
        }




        public ActionResult Edit(int id)
        {
            ViewBag.skills = new SelectList(db.Skills, "SkillId", "SkillTitle");
            return View();
        }
        [HttpPost]
        public ActionResult Edit(TrainerViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                string filepath = tvm.PhotoPath;
                if (tvm.Photo != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(tvm.Photo.FileName);
                    filepath = Path.Combine("Images", "Trainer", filename);
                    string file = Path.Combine(Server.MapPath("~/" + filepath));
                    tvm.Photo.SaveAs(file);
                    Trainer trainer = new Trainer
                    {
                        TrainerId=tvm.TrainerId,
                        Name = tvm.Name,
                        FatherName = tvm.FatherName,
                        Email = tvm.Email,
                        Gender = tvm.Gender,
                        BirthDate = tvm.BirthDate,
                        NID = tvm.NID,
                        Phone = tvm.Phone,
                        Address = tvm.Address,
                        PhotoPath = filepath
                    };
                    db.Entry(trainer).State = EntityState.Modified;
                    db.SaveChanges();
                    int id = trainer.TrainerId;
                    foreach (var skillid in tvm.SkillIds)
                    {
                        Skill skill = db.Skills.FirstOrDefault(x => x.SkillId == skillid);
                        db.Trainers.Include("Skills").FirstOrDefault(x => x.TrainerId == id).Skills.Add(skill);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Trainer trainer = new Trainer
                    {
                        TrainerId = tvm.TrainerId,
                        Name = tvm.Name,
                        FatherName = tvm.FatherName,
                        Email = tvm.Email,
                        Gender = tvm.Gender,
                        BirthDate = tvm.BirthDate,
                        NID = tvm.NID,
                        Phone = tvm.Phone,
                        Address = tvm.Address,
                        PhotoPath = filepath
                    };
                    db.Entry(trainer).State = EntityState.Modified;
                    db.SaveChanges();
                    //int id = trainer.TrainerId;
                    //foreach (var skillid in tvm.SkillIds)
                    //{
                    //    Skill skill = db.Skills.FirstOrDefault(x => x.SkillId == skillid);
                    //    db.Trainers.Include("Skills").FirstOrDefault(x => x.TrainerId == id).Skills.Add(skill);
                    //}
                    //db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainers.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            db.Trainers.Remove(trainer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}