using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExportManager.DBModel
{
    public class Export_view
    {

        // public IEnumerable<Export> exports { get; set; }
        public IPagedList<Export> exports { get; set; }
        // public IPagedList<Export> exp_country { get; set; }
        //    public IPagedList<Export> exp_item { get; set; }
        public Export export_val { get; set; }
        public License lic_details { get; set; }
        public IEnumerable<Export> exp_country { get; set; }
        public IEnumerable<Export> exp_item { get; set; }
        //public string search_str { get; set; }

    }
}