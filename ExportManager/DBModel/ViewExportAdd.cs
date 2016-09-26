using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExportManager.DBModel
{
    public class ViewExportAdd
    {
        public List<SelectListItem> LicenseNos { set; get; }
        public List<SelectListItem> Items { set; get; }
        public License LicenseDetails {set; get;}
        //    public int? Selectedlicense { set; get; }
        public IEnumerable<int> Selectedlicense { set; get; }
        
        public IEnumerable<int> SelectedItems { set; get; }
        public IEnumerable<int> Selectedcountrylist { set; get; }
        public Export Export { set; get; }
        [Required(ErrorMessage = "Required")]
       
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int No_Of_Units { get; set; }
        public int lic_id { get; set; }
        public int exp_id { get; set; }
        public string msg { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}", ApplyFormatInEditMode = true)]
        public DateTime Expiry_Date { get; set; }
        public IEnumerable<Export> exp_item { get; set; }
    }


}