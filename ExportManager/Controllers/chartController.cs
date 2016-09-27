using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using ExportManager.DBModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExportManager.Controllers
{
    public class ChartController : Controller
    {
        private LicenseManagerEntities db = new LicenseManagerEntities();
        // GET: chart
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var lic_no = from lic in db.Licenses where lic.UserId == userId select new { licenseNo = lic.License_No, licenseId = lic.Id };

            var lic_ids = (from lic in db.Licenses where lic.UserId == userId select lic.Id).ToList() ;
            var exp_count = from lic_exp in db.Exports
                            where lic_ids.Contains(lic_exp.License_Id.Value)
                            group lic_exp by lic_exp.License_Id into g
                            // orderby g.Count(x =>x.Id )
                            select
            new
            {
                lic_code = g.Key,
              //  item_value = g.First().,
                total = g.Count() 
            };
            //
            List<object> dataList = new List<object>();
            int i = 0;

            List<Series> allSeries = new List<Series>();
            var lic_arry = lic_no.ToArray();

            Dictionary<int, int> licenseId_ExportCount_Dictionary = exp_count.ToDictionary(o => o.lic_code.Value, o => o.total);
            foreach (var item in lic_arry)
            {
                int count = 0;
                if (licenseId_ExportCount_Dictionary.ContainsKey(item.licenseId))
                {
                    count = licenseId_ExportCount_Dictionary[item.licenseId];
                }
                dataList.Add(count);
                 
            }

            var query = db.lic_exp_val(userId).ToList();
        //    var lic_val = query.Select(o=>o.l_val).;
            List<object> dataList1 = new List<object>();
            foreach (var item in query)
            {
                dataList1.Add(item.l_val);

            }
            List<object> dataList2 = new List<object>();
            foreach (var item in query)
            {
                dataList2.Add(item.e_val);

            }


            allSeries.Add(new Series
            {
                Name = "Export Value",
                // Data = new Data(dataList.ToArray())
                Data = new Data(dataList2.ToArray())
            });

            allSeries.Add(new Series
            {
                Name = "License Total Value",
               // Data = new Data(dataList.ToArray())
               Data=new Data(dataList1.ToArray())
            });

            var count_exp = exp_count.ToArray();

            Highcharts chart = new Highcharts("chart")
    .SetCredits(new Credits { Enabled = false })
    .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
    .SetTitle(new Title { Text = "License Export Value" })
    .SetXAxis(new XAxis { Categories = query.Select( o => o.lic_no).ToArray() })
    .SetYAxis(new YAxis
    {
        Min = 0,
        Title = new YAxisTitle { Text = "Value" }
       
    })
    .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.series.name +': '+ this.y +''; }" })
    .SetPlotOptions(new PlotOptions { Bar = new PlotOptionsBar { Stacking = Stackings.Normal } })
    .SetSeries(allSeries.Select(s => new Series { Name = s.Name,Data=s.Data}).ToArray());


            return View(chart);

           // return View();
        }
    }
}