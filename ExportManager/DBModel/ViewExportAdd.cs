using System;
using System.Collections.Generic;
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
        public int No_Of_Units { get; set; }
        public int lic_id;
        public int exp_id;
        public string msg;
        public IEnumerable<Export> exp_item { get; set; }
    }


}