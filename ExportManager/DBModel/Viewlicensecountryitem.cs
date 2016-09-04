using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExportManager.DBModel
{
    public class Viewlicensecountryitem
    {
            public License allLicense { get; set; }
            public String allItems { get; set; }
            public String allCountry { get; set; }
          public IEnumerable<License> Licenses { get; set; }
          public IEnumerable<License> License_country { get; set; }
          public IEnumerable<License> License_item { get; set; }

    }
}