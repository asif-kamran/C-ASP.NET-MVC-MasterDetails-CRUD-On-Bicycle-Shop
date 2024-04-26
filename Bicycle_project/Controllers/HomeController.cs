using Bicycle_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bicycle_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly CycleDbContext db= new CycleDbContext();
        public ActionResult Index()
        {
            var totalCycleCount=db.Cycles.Count();
            ViewBag.TotalCycleCount = totalCycleCount;
            var sumOfTotalCyclePrice=db.StockInfos.Sum(x=>x.Amount);
            ViewBag.sumOfTotalCyclePrice=sumOfTotalCyclePrice;
            var minCyclePrice=db.StockInfos.Min(x=>x.Amount);
            ViewBag.minCyclePrice=minCyclePrice;
            var maxCyclePrice = db.StockInfos.Max(x => x.Amount);
            ViewBag.maxCyclePrice = maxCyclePrice;
            var avgCyclePrice = db.StockInfos.Average(x => x.Amount);
            ViewBag.avgCyclePrice = avgCyclePrice;
            return View();
        }
    }
}