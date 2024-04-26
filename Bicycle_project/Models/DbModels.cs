using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design.Serialization;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bicycle_project.Models
{
    public enum Category
    {
        RoadBikes=1,
        MountainBikes,
        AdventureBikes,
        FatBikes,
        KidBikes,
        WomenBikes
    }
    public class Company
    {
        public int CompanyId { get; set; }
        [Required,StringLength(50),Display(Name ="Company Name")]
        public string CompanyName { get; set;}
        public virtual ICollection<Cycle> Cycles { get; set; } = new List<Cycle>();
    }
    public class CycleModel
    {
        public int CycleModelId { get; set; }
        [Required, StringLength(50), Display(Name = "Model Name")]
        public string ModelName { get; set; }
        public virtual ICollection<Cycle> Cycles { get; set; }=new List<Cycle>();
    }
    public class Cycle
    {
        public int CycleId { get; set; }
        [Required, StringLength(50), Display(Name = "Cycle Name")]
        public string CycleName { get; set; }
        [Required,Column(TypeName ="date"),Display(Name ="Entry Date"),DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime EntryDate { get; set; }
        public bool Status { get; set; }
        public string Picture { get; set; }
        [Display(Name = "Model Name")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        [Display(Name = "Model Name")]
        [ForeignKey("CycleModel")]
        public int ModelId { get; set; }
        public virtual Company Company { get; set; }
        public virtual CycleModel CycleModel { get; set; }
        public virtual ICollection<StockInfo> Stocks { get; set; }=new List<StockInfo>();
    }
    public class StockInfo
    {
        public int StockInfoId { get; set; }
        public Category Category { get; set; }
        public decimal Amount { get; set; }
        public int Qty { get; set; }
        [ForeignKey("Cycle")]
        public int CycleId { get; set; }
        public virtual Cycle Cycle { get; set; }
    }
    public class CycleDbContext : DbContext
    {
        public DbSet<Company> Companys { get; set; }
        public DbSet<CycleModel> CycleModels { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<StockInfo> StockInfos { get; set; }
    }
}