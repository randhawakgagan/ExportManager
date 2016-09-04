using ExportManager.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;

namespace ExportManager.Controllers
{
    public class ExportController : Controller
    {
        private LicenseManagerEntities db = new LicenseManagerEntities();

        public PartialViewResult itempartialView(int? exp_id)
        {


            TempData["id"] = exp_id.Value;
            ViewExportAdd model = new ViewExportAdd();
            model.exp_item = db.Exports.Where(z => z.Id == exp_id.Value).Include(x => x.Export_Item.Select(y => y.Item));
            
            var item_list =
              from itm in db.Items
              join lic in db.License_Item on itm.Id equals lic.Item_Id
              where lic.License_Id == model.exp_item.FirstOrDefault().License.Id
              select itm;

            model.lic_id =Convert.ToInt32(model.exp_item.First().License.Id);
            model.exp_id = exp_id.Value;
            //  model.Items= new SelectListItem(item_list.ToList(), "Id", "Name")

            model.Items =
             item_list.Select(x => new SelectListItem
             {
                 Value = x.Id.ToString(),
                 Text = x.Name,
             })
             .ToList();

            model.SelectedItems = db.Items.Select(x => x.Id);


            // db.SaveChanges();

          //  return View(model);
            //return RedirectToAction("GetExpDetails",new { exp_id = exp_id.Value });
            return PartialView("EdititemPartialView", model);
        }

        public ActionResult GetExpDetails(int? exp_id)
        {

            var exp_detail = from l in db.Exports
                             where l.Id == exp_id.Value
                             select new { ref_no = l.Reference_No, exp_date = l.Export_Date, lic_date = l.License.Expiry_Date, lic_no = l.License.License_No };
            var item_list =
            from itm in db.Items
            join exp in db.Export_Item on itm.Id equals exp.Item_Id
            where exp.Exporter_Id== exp_id.Value
            select new { item_id = itm.Id, item_name = itm.Name ,units=exp.No_Of_Units};

            var model = new Export_view();
            //model.exports = db.Exports.Where(r => searchterm == null || r.Reference_No.StartsWith(searchterm)).Include(k => k.License).
            //     Include(i => i.Export_Country.Select(z => z.Country)).Include(y => y.Export_Item.Select(p => p.Item)).OrderBy(u => u.Reference_No).ToPagedList(page, 4);


            model.export_val = db.Exports.Where(x => x.Id == exp_id.Value).Include(z => z.License).Single();
            ////model.exports = db.Exports.Where(r => searchterm == null || r.Reference_No.StartsWith(searchterm)).
            ////Include(i => i.);

            ////model.exp_item= model.exports.Include(y => y.Item);
            //if (exp_id != null)
            //{

               var items = db.Exports.Where(x => x.Id == exp_id.Value).Include(z => z.Export_Item.Select(y => y.Item));

              model.exp_item = items;
            //    ViewBag.t_id = exp_id.Value;



            //}
            //if (item_id != null)
            //{
            //    var items = db.Exports.Where(x => x.Id == item_id.Value).Include(z => z.Export_Country.Select(y => y.Country));

            //    model.exp_country = items;
            //    ViewBag.c_id = item_id.Value;



            //}


            return View(model);
            //var country_list =
            //from country in db.Countries
            //join lic in db.License_Country on country.Id equals lic.Country_Id
            //where lic.License_Id == val
            //select new { country_id = country.Id, country_name = country.Name };

            //var lic_details = from l in db.Licenses where l.Id == val select new { expiry_date = l.Expiry_Date, notes = l.Notes };

            //var CountryList = country_list.ToList();

            //var res = new { detail=exp_detail,items= item_list };
            ////   return Json(countryDTOList, JsonRequestBehavior.AllowGet);

            //return this.Json(res, JsonRequestBehavior.AllowGet);


        }

        public ActionResult UpdateExportitem(int ? item_id,int ? exp_item_id , int? exp_id, int? lic_id, ViewExportAdd exportvalues)
        {
            //var item_found = from itm in db.Export_Item where itm.Item_Id == item_id.Value && itm.Exporter_Id == exp_id.Value select itm;
            var item_found = from itm in db.Export_Item where itm.Id== exp_item_id.Value select itm;

            item_found.First().No_Of_Units= exportvalues.No_Of_Units;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
            }
          
            return RedirectToAction("AddExportitem", new { exp_id = exp_id.Value, lic_id = lic_id.Value });
        }

        public ActionResult DeleteExportitem( int? exp_item_id, int? exp_id, int? lic_id)
        {

           

            Export_Item exp_item = db.Export_Item.Find(exp_item_id.Value);
            db.Export_Item.Remove(exp_item);
            db.SaveChanges();
            return RedirectToAction("AddExportitem" ,new {exp_id=exp_id.Value,lic_id=lic_id.Value });
        }

        public ActionResult Details(int? id,int? exp_id,int? item_id,string searchterm=null,int page=1)
        {
            //  var model = db.Exports.Include(x => x.Item_Id);

            //     var model=db.Exports.Select(x => x.Id);
            var model = new Export_view();
            model.exports = db.Exports.Where(r => searchterm == null || r.Reference_No.StartsWith(searchterm)).Include(k =>k.License).
                 Include(i => i.Export_Country.Select(z => z.Country)).Include(y => y.Export_Item.Select(p => p.Item)).OrderBy(u =>u.Reference_No).ToPagedList(page, 4);
            


            //model.exports = db.Exports.Where(r => searchterm == null || r.Reference_No.StartsWith(searchterm)).
            //Include(i => i.);

            //model.exp_item= model.exports.Include(y => y.Item);
            if (exp_id!=null)
            {
                var items = db.Exports.Where(x => x.Id == exp_id.Value).Include(z => z.Export_Item.Select(y=>y.Item));

                model.exp_item = items;
                ViewBag.t_id = exp_id.Value;



            }
            if (item_id != null)
            {
                var items = db.Exports.Where(x => x.Id == item_id.Value).Include(z => z.Export_Country.Select(y => y.Country));

                model.exp_country = items;
                ViewBag.c_id = item_id.Value;



            }
            

            return View(model);
        }
        // GET: Export
        public ActionResult Index()
        {

            ViewExportAdd viewpass = new ViewExportAdd();
          //  viewpass.Selectedlicense=db.Licenses.Select(x => x.Id);
            viewpass.LicenseNos = db.Licenses
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.License_No,
                })
                .ToList();

            viewpass.Selectedlicense = db.Licenses.Select(x => x.Id);
           

            return View(viewpass);
         }

        public ActionResult AddExportitem(int? exp_id,int ? lic_id,ViewExportAdd exportvalues)
        {
           // int exp_id = Convert.ToInt32(TempData["id"]);
            TempData["id"] = exp_id.Value;
            ViewExportAdd model = new ViewExportAdd();
            model.exp_item = db.Exports.Where(z => z.Id == exp_id.Value).Include(x => x.Export_Item.Select(y => y.Item));
            if (exportvalues.SelectedItems != null)
            {
                foreach (var i in exportvalues.SelectedItems)
                {
                    var items_add = new Export_Item();
                    var item_found=from itm in db.Export_Item where itm.Item_Id==i && itm.Exporter_Id==exp_id.Value select itm;
                    
                    if (item_found.Any())
                    {
                        var item_name= from itm in db.Items where itm.Id == i select itm.Name;
                        model.msg = "Item - "+ item_name.FirstOrDefault()+"-already added ,please choose different one ";

                        break;
                    }
                    var units_allowed = from itm in db.License_Item where itm.Item_Id == i select itm.No_Units;

                    
                    if (Convert.ToInt32(
                        units_allowed.FirstOrDefault()) < exportvalues.No_Of_Units)
                    {
                        model.msg = "no of units can't  exceed than" + Convert.ToInt32(
                        units_allowed.FirstOrDefault());

                    }
                  
                    else
                    {
                        items_add.Item_Id = i;
                        items_add.No_Of_Units = exportvalues.No_Of_Units;
                        items_add.Exporter_Id = exp_id.Value;
                        db.Export_Item.Add(items_add);
                        db.SaveChanges();
                    }
                    }
              
            }
            var item_list =
              from itm in db.Items
              join lic in db.License_Item on itm.Id equals lic.Item_Id
              where lic.License_Id == lic_id.Value
              select itm;

            model.lic_id = lic_id.Value;
            model.exp_id = exp_id.Value;
         //  model.Items= new SelectListItem(item_list.ToList(), "Id", "Name")

            model.Items =
             item_list.Select(x => new SelectListItem
             {
                 Value = x.Id.ToString(),
                 Text = x.Name,
             })
             .ToList();

            model.SelectedItems = db.Items.Select(x => x.Id);


           // db.SaveChanges();

            return View(model);
            
            //  return Json(new { lic_i
        }


       [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewExportAdd exportvalues)
        {

            if (ModelState.IsValid)
            {
                Export export = new Export();
                export.Reference_No = exportvalues.Export.Reference_No;
                export.Export_Date = exportvalues.Export.Export_Date;
                export.License_Id = exportvalues.Selectedlicense.FirstOrDefault();


                db.Exports.Add(export);
                db.SaveChanges();
                var export_id = from a in db.Exports
                                 where a.Reference_No == exportvalues.Export.Reference_No
                                 select a.Id;
                TempData["id"]= Convert.ToInt32(export_id.FirstOrDefault());
                foreach (var i in exportvalues.Selectedcountrylist)
                {
                    var exp_country = new Export_Country();
                    exp_country.Country_Id = i;
                    exp_country.Export_Id= Convert.ToInt32(export_id.FirstOrDefault());
                    db.Export_Country.Add(exp_country);

                }

              /*  foreach (var i in exportvalues.SelectedItems)
                {
                    var items_add = new Export_Item();

                    items_add.Item_Id = i;
                    items_add.No_Of_Units = exportvalues.No_Of_Units;
                    items_add.Exporter_Id = Convert.ToInt32(export_id.FirstOrDefault());
                    db.Export_Item.Add(items_add);

                }*/


                db.SaveChanges();
                return RedirectToAction("AddExportitem",new { exp_id = Convert.ToInt32(export_id.FirstOrDefault()), lic_id = exportvalues.Selectedlicense.FirstOrDefault() });
             //  return Content("done");
            }
            return Content("done");
            // return View(exportvalues);
        }



          //  return View();
       

        public JsonResult GetItemsCountry(int? val)
          {  
                if (val != null)
                {
                 
        //Values are hard coded for demo. you may replae with values 
        // coming from your db/service based on the passed in value ( val.Value)
              var item_list =
             from itm in db.Items
             join lic in db.License_Item on itm.Id equals lic.Item_Id
             where lic.License_Id == val
             select new { item_id= itm.Id,item_name=itm.Name };

            var country_list =
            from country in db.Countries
            join lic in db.License_Country on country.Id equals lic.Country_Id
            where lic.License_Id == val
            select new {country_id= country.Id, country_name = country.Name };

                var lic_details = from l in db.Licenses where l.Id == val select new {expiry_date =l.Expiry_Date, notes=l.Notes };

                var CountryList = country_list.ToList();

                var res = new {items=item_list,country_list=CountryList ,licdetails= lic_details };
                //   return Json(countryDTOList, JsonRequestBehavior.AllowGet);

                return this.Json(res, JsonRequestBehavior.AllowGet);
                }

               return Json(new { Success = "false" });


            }
    }
}
