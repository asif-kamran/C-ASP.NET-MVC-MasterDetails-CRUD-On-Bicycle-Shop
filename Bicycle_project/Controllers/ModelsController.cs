using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bicycle_project.Models;

namespace Bicycle_project.Controllers
{
    public class ModelsController : Controller
    {
        private CycleDbContext db = new CycleDbContext();
        public ActionResult Index()
        {
            return View(db.CycleModels.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CycleModelId,ModelName")] CycleModel cycleModel)
        {
            if (ModelState.IsValid)
            {
                db.CycleModels.Add(cycleModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cycleModel);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CycleModel cycleModel = db.CycleModels.Find(id);
            if (cycleModel == null)
            {
                return HttpNotFound();
            }
            return View(cycleModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CycleModelId,ModelName")] CycleModel cycleModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cycleModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cycleModel);
        }
        public ActionResult Delete(int? id)
        {
            CycleModel cycleModel = db.CycleModels.Find(id);
            db.CycleModels.Remove(cycleModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
