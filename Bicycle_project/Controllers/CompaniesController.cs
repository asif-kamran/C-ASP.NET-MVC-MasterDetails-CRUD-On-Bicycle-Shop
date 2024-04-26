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
    public class CompaniesController : Controller
    {
        private CycleDbContext db = new CycleDbContext();
        public ActionResult Index()
        {
            return View(db.Companys.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companys.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }
        public ActionResult Edit(int? id)
        {
            Company company = db.Companys.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,CompanyName")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }
        public ActionResult Delete(int? id)
        {
            Company company = db.Companys.Find(id);
            db.Companys.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
