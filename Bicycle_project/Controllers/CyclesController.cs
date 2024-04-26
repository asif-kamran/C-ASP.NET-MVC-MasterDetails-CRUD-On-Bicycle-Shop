using Bicycle_project.Models;
using Bicycle_project.Models.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bicycle_project.Controllers
{
    [Authorize]
    public class CyclesController : Controller
    {
        private readonly CycleDbContext db= new CycleDbContext();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult ShoeDetails(int pg = 1)
        {
            var data = db.Cycles
                         .Include(x => x.Stocks)
                         .Include(x => x.CycleModel)
                         .Include(x => x.Company)
                         .OrderBy(x => x.CycleId)
                         .ToPagedList(pg, 5);
            return PartialView("_ShoeDetails", data);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateForm()
        {
            CycleInputModel model = new CycleInputModel();
            model.Stocks.Add(new StockInfo());
            ViewBag.CycleModels=db.CycleModels.ToList();
            ViewBag.Companys=db.Companys.ToList();
            return PartialView("_createForm", model);
        }
        [HttpPost]
        public ActionResult Create(CycleInputModel model,string act="")
        {
            if (act == "add")
            {
                model.Stocks.Add(new StockInfo());
                foreach (var i in ModelState.Values)
                {
                    i.Errors.Clear();
                    i.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Stocks.RemoveAt(index);
                foreach (var i in ModelState.Values)
                {
                    i.Errors.Clear();
                    i.Value = null;
                }
            }
            if(act== "insert")
            {
                if (ModelState.IsValid)
                {
                    var cycle = new Cycle
                    {
                        CompanyId = model.CompanyId,
                        ModelId = model.ModelId,
                        CycleName = model.CycleName,
                        EntryDate = model.EntryDate,
                        Status = model.Status,
                    };
                    //image
                    string extension = Path.GetExtension(model.Picture.FileName);
                    string filepath = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;
                    string savePath = Path.Combine(Server.MapPath("/Images/"), filepath);
                    model.Picture.SaveAs(savePath);
                    cycle.Picture = filepath;
                    db.Cycles.Add(cycle);
                    db.SaveChanges();
                    //StockInfo
                    foreach (var s in model.Stocks)
                    {
                        db.Database.ExecuteSqlCommand($"spInsertStock {(int)s.Category},{s.Amount},{(int)s.Qty},{cycle.CycleId}");
                    }
                    CycleInputModel newModel = new CycleInputModel()
                    {
                        CycleName = "",
                        EntryDate = DateTime.Today
                    };
                    newModel.Stocks.Add(new StockInfo());
                    ViewBag.CycleModels = db.CycleModels.ToList();
                    ViewBag.Companys = db.Companys.ToList();
                    foreach (var s in ModelState.Values)
                    {
                        s.Value = null;
                    }
                    return View("_createForm", newModel);
                }
            }
            ViewBag.CycleModels = db.CycleModels.ToList();
            ViewBag.Companys = db.Companys.ToList();
            return View("_createForm", model);
        }
        public ActionResult Delete(int? id)
        {
            var cycleInfo = db.Cycles.Find(id);
            if(cycleInfo != null)
            {
                var stockInfo = db.StockInfos.Where(x => x.CycleId == id);
                db.StockInfos.RemoveRange(stockInfo);
                db.Cycles.Remove(cycleInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult EditForm(int id)
        {
            var data=db.Cycles.FirstOrDefault(x=>x.CycleId == id);
            if(data==null) return new HttpNotFoundResult();
            db.Entry(data).Collection(x => x.Stocks).Load();
            CycleEditModel model = new CycleEditModel
            {
                CycleId= id,
                CompanyId=data.CompanyId,
                ModelId=data.ModelId,
                CycleName=data.CycleName,
                EntryDate=data.EntryDate,
                Status=data.Status,
                Stocks=data.Stocks.ToList(),
            };
            ViewBag.CycleModels = db.CycleModels.ToList();
            ViewBag.Companys = db.Companys.ToList();
            ViewBag.currentPic = data.Picture;
            return PartialView("_editForm",model);
        }
        [HttpPost]
        public ActionResult Edit(CycleEditModel model,string act = "")
        {
            if (act == "add")
            {
                model.Stocks.Add(new StockInfo());
                foreach (var i in ModelState.Values)
                {
                    i.Errors.Clear();
                    i.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Stocks.RemoveAt(index);
                foreach (var i in ModelState.Values)
                {
                    i.Errors.Clear();
                    i.Value = null;
                }
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var cycle = db.Cycles.FirstOrDefault(x => x.CycleId == model.CycleId);
                    if (cycle == null) { return new HttpNotFoundResult(); }
                    cycle.CycleName = model.CycleName;
                    cycle.EntryDate = model.EntryDate;
                    cycle.Status = model.Status;
                    cycle.CompanyId = model.CompanyId;
                    cycle.ModelId = model.ModelId;
                    if (model.Picture != null)
                    {
                        string extension = Path.GetExtension(model.Picture.FileName);
                        string filepath = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;
                        string savePath = Path.Combine(Server.MapPath("/Images/"), filepath);
                        model.Picture.SaveAs(savePath);
                        cycle.Picture = filepath;
                    }
                    else
                    {
                        //model.Picture=cycle.Picture;
                    }
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand($"exec spDeleteStock {cycle.CycleId}");
                    foreach (var s in model.Stocks)
                    {
                        db.Database.ExecuteSqlCommand($"exec spInsertStock {(int)s.Category},{s.Amount},{s.Qty},{cycle.CycleId}");
                    }
                }
                return PartialView("_success");
            }
            ViewBag.CycleModels = db.CycleModels.ToList();
            ViewBag.Companys = db.Companys.ToList();
            ViewBag.currentPic = db.Cycles.FirstOrDefault(x=>x.CycleId == model.CycleId)?.Picture;
            return View("_editForm",model);
        }
    }
}