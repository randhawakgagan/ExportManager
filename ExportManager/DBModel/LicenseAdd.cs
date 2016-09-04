using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExportManager.DBModel
{
   
    public class item_units
    {
        public string Itm_names;
        public int Itm_No_Units;
    }
        public class LicenseAdd
        {
            public IEnumerable<int> SelectedCountries { get; set; }

            public IEnumerable<SelectListItem> Counties { get; set; }
            public IEnumerable<int> SelectedItems { get; set; }
            public List<SelectListItem> Items { get; set; }
        //  public List<License_Item> Item_list { set; get; }
        public int No_Units { get; set; }
       // public List<int> no_of_itms;
        public List<item_units> Itms_db;
       // public List<int> Itm_No_Units;
         public int Id { get; set; }
        [Required]
           public string License_No { get; set; }

           [Required]
           [DataType(DataType.Date)]
           [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}", ApplyFormatInEditMode = true)]
            public System.DateTime Expiry_Date { get; set; }

           public string Notes { get; set; }
        }
    
}