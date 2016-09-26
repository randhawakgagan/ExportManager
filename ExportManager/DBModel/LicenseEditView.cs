using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExportManager.DBModel
{
    public class LicenseEditView
    {

        public License Lic_details { get; set; }
        public IEnumerable<License> country { get; set; }
        public IEnumerable<License> item { get; set; }
        public string msg { get; set; }
        public IEnumerable<int> SelectedCountries { get; set; }
        public IEnumerable<SelectListItem> Counties { get; set; }
        public IEnumerable<int> SelectedItems { get; set; }
        public List<SelectListItem> Items { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int No_Of_Units { get; set; }
        public int Lic_id { get; set; }
    }
}