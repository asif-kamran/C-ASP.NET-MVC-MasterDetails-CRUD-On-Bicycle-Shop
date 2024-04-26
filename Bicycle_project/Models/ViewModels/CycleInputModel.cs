using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bicycle_project.Models.ViewModels
{
    public class CycleInputModel
    {
        public int CycleId { get; set; }
        [Required, StringLength(50), Display(Name = "Cycle Name")]
        public string CycleName { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Entry Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EntryDate { get; set; }
        public bool Status { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        [Display(Name = "Model")]
        public int ModelId { get; set; }
        public virtual List<StockInfo> Stocks { get; set; } = new List<StockInfo>();
    }
}