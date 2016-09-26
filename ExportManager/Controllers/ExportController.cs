using ExportManager.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;
using Microsoft.AspNet.Identity;

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

        public PartialViewResult CpartialView(int? exp_id)
        {


            TempData["id"] = exp_id.Value;
            ViewExportAdd model = new ViewExportAdd();
            model.exp_item = db.Exports.Where(z => z.Id == exp_id.Value).Include(x => x.Export_Item.Select(y => y.Item));

            var item_list =
              from itm in db.Countries
              join lic in db.License_Country on itm.Id equals lic.Country_Id
              where lic.License_Id == model.exp_item.FirstOrDefault().License.Id
              select itm;

            model.lic_id = Convert.ToInt32(model.exp_item.First().License.Id);
            model.exp_id = exp_id.Value;
            //  model.Items= new SelectListItem(item_list.ToList(), "Id", "Name")

            model.Items =
             item_list.Select(x => new SelectListItem
             {
                 Value = x.Id.ToString(),
                 Text = x.Name,
             })
             .ToList();

            model.Selectedcountrylist =db.Countries.Select(x => x.Id);


            // db.SaveChanges();

            //  return View(model);
            //return RedirectToAction("GetExpDetails",new { exp_id = exp_id.Value });
            return PartialView("EditCountryPartialView", model);
        }

        public ActionResult GetCountryDetails(int? exp_id)
        {
            var model = new Export_view();


            var country = db.Exports.Where(x => x.Id == exp_id.Value).Include(z => z.Export_Country.Select(y => y.Country));

            model.exp_country = country;
            return PartialView("Displaycountry", model);
           
            
            
        }
        public ActionResult GetExpDetails(int? exp_id,string msg)
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

            model.msg = msg;
            model.export_val = db.Exports.Where(x => x.Id == exp_id.Value).Include(z => z.License).Single();
            ////model.exports = db.Exports.Where(r => searchterm == null || r.Reference_No.StartsWith(searchterm)).
            ////Include(i => i.);

            ////model.exp_item= model.exports.Include(y => y.Item);
            //if (exp_id != null)
            //{

               var items = db.Exports.Where(x => x.Id == exp_id.Value).Include(z => z.Export_Item.Select(y => y.Item));

              model.exp_item = items;
            //    ViewBag.t_id = exp_id.Value;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Itemdisplay", model);
            }
            //}
            //if (item_id != null)
            //{
               var country= db.Exports.Where(x => x.Id == exp_id.Value).Include(z => z.Export_Country.Select(y => y.Country));

              model.exp_country = country;
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

        public JsonResult GetItemDetails(int? exp_id)
        {

           
            var item_list =
            from itm in db.Items
            join exp in db.Export_Item on itm.Id equals exp.Item_Id
            where exp.Exporter_Id == exp_id.Value
            select new { item_id = itm.Id, item_name = itm.Name, units = exp.No_Of_Units ,Uvalue=itm.Unit_Value};
            //var country_list =
            //from country in db.Countries
            //join lic in db.License_Country on country.Id equals lic.Country_Id
            //where lic.License_Id == val
            //select new { country_id = country.Id, country_name = country.Name };

            //var lic_details = from l in db.Licenses where l.Id == val select new { expiry_date = l.Expiry_Date, notes = l.Notes };

            //var CountryList = country_list.ToList();

            var res = new { items= item_list };
            ////   return Json(countryDTOList, JsonRequestBehavior.AllowGet);

            return this.Json(res, JsonRequestBehavior.AllowGet);


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

        public ActionResult DeleteExportitem( int exp_id , List<int> itemCheckedIds ,List<int> countryCheckboxes)
        {
            var model = new Export_view();

            if (itemCheckedIds != null)
            {
                foreach (var itm_id in itemCheckedIds)
                {
                    Export_Item exp_item = db.Export_Item.Find(itm_id);
                    db.Export_Item.Remove(exp_item);
                    db.SaveChanges();
                }
            }
            if(countryCheckboxes!=null)
            {
                foreach (var c_id in countryCheckboxes)
                {
                    Export_Country c_item = db.Export_Country.Find(c_id);
                    db.Export_Country.Remove(c_item);
                    db.SaveChanges();
                }
                
                var country = db.Exports.Where(x => x.Id == exp_id).Include(z => z.Export_Country.Select(y => y.Country));

                model.exp_country = country;
                //    ViewBag.t_id = exp_id.Value;

                return PartialView("Displaycountry", model);
            }
           
            //model.exports = db.Exports.Where(r => searchterm == null || r.Reference_No.StartsWith(searchterm)).Include(k => k.License).
            //     Include(i => i.Export_Country.Select(z => z.Country)).Include(y => y.Export_Item.Select(p => p.Item)).OrderBy(u => u.Reference_No).ToPagedList(page, 4);

           
            //model.export_val = db.Exports.Where(x => x.Id == exp_id.Value).Include(z => z.License).Single();
            ////model.exports = db.Exports.Where(r => searchterm == null || r.Reference_No.StartsWith(searchterm)).
            ////Include(i => i.);

            ////model.exp_item= model.exports.Include(y => y.Item);
            //if (exp_id != null)
            //{

            var items = db.Exports.Where(x => x.Id == exp_id).Include(z => z.Export_Item.Select(y => y.Item));

            model.exp_item = items;
          
            //    ViewBag.t_id = exp_id.Value;

            return PartialView("Itemdisplay", model);
           
            // return RedirectToAction("AddExportitem" ,new {exp_id=exp_id.Value,lic_id=lic_id.Value });
        }

        public ActionResult Details(int? id,int? exp_id,int? item_id,string searchterm=null,int page=1)
        {
            //  var model = db.Exports.Include(x => x.Item_Id);

            //     var model=db.Exports.Select(x => x.Id);
            var model = new Export_view();
            var userId = User.Identity.GetUserId();

            var lic_ids = from l in db.Licenses where l.UserId == userId select l.Id;
            var ids = lic_ids.ToList();
            model.exports = db.Exports.Where(r =>( (searchterm == null || r.Reference_No.StartsWith(searchterm))&& (ids.Contains(r.License_Id.Value)))).Include(k => k.License).
                 Include(i => i.Export_Country.Select(z => z.Country)).Include(y => y.Export_Item.Select(p => p.Item)).OrderBy(u => u.Reference_No).ToPagedList(page, 4);
            


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
            var user_id = User.Identity.GetUserId();
            viewpass.LicenseNos = db.Licenses.Where(x=>x.UserId == user_id)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.License_No,
                })
                .ToList();
          // var  dt = new DateTime(2013,01,01);

            DateTime now = DateTime.Now;

            viewpass.Expiry_Date = new DateTime(now.Year, now.Month, now.Day, 16, 0, 0);
         
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
                  //  var item_found=from itm in db.Export_Item where itm.Item_Id==i && itm.Exporter_Id==exp_id.Value select itm;

                    var units_total = from itm in db.License_Item where itm.Item_Id == i && itm.License_Id == lic_id.Value select itm.No_Units;
                    var exp_ids = db.Exports.Where(z => z.License_Id == lic_id.Value).Select(y => y.Id);

                    var sum_units = from f in db.Export_Item
                                    where exp_ids.Contains(f.Exporter_Id)
                                    group f by f.Item_Id into g
                                    orderby g.Sum(x => x.No_Of_Units)
                                    select
                                    new
                                    {
                                        item_code = g.Key,
                                        //item_value = g.First().,
                                        total = g.Sum(x => x.No_Of_Units)
                                    };

                    var sum = 0;

                    foreach (var n in sum_units)
                    {

                        if (n.item_code == i)
                        {
                            sum = n.total;
                            break;
                        }


                    }

                    var units_allowed = (units_total.SingleOrDefault()) - sum;
                    if ((
                        units_allowed) < exportvalues.No_Of_Units)
                    {
                        model.msg = "no of units can't  exceed than" + (
                        units_allowed);
                      //  view.msg = model.msg;
                        break;
                        //  return RedirectToAction("GetExpDetails", new { exp_id = exp_id.Value, msg = model.msg });
                    }
                    var item_found = from itm in db.Export_Item where itm.Item_Id == i && itm.Exporter_Id == exp_id select itm;

                    if (item_found.Any())
                    {
                        item_found.First().No_Of_Units = exportvalues.No_Of_Units;
                        db.SaveChanges();
                        var item_name = from itm in db.Items where itm.Id == i select itm.Name;
                        model.msg = "Item - " + item_name.FirstOrDefault() + "item updated ";
                     //   view.msg = model.msg;
                        //return RedirectToAction("GetExpDetails",new { exp_id = exp_id.Value, msg = model.msg });
                        break;

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


        public JsonResult Additem( ViewExportAdd exportvalues)
        {
            // int exp_id = Convert.ToInt32(TempData["id"]);
           // TempData["id"] = exp_id.Value;
            var exp_id = exportvalues.exp_id;
            ViewExportAdd model = new ViewExportAdd();
            Export_view view = new Export_view();
            //model.exp_item = db.Exports.Where(z => z.Id == exp_id).Include(x => x.Export_Item.Select(y => y.Item));
            var lic_id = db.Exports.Where(z => z.Id == exp_id).Include(x => x.License).Single();
            if(exportvalues.Selectedcountrylist!=null)
            {
                foreach (var i in exportvalues.Selectedcountrylist)
                {
                    var c_add = new Export_Country();
                    var item_found = from itm in db.Export_Country where itm.Country_Id== i && itm.Export_Id == exp_id select itm;

                    if (item_found.Any())
                    {
                       
                        var item_name = from itm in db.Countries where itm.Id == i select itm.Name;
                        model.msg = "Item - " + item_name.FirstOrDefault() + "item already added ";
                        view.msg = model.msg;
                        //return RedirectToAction("GetExpDetails",new { exp_id = exp_id.Value, msg = model.msg });
                        break;

                    }


                    else
                    {
                        c_add.Country_Id = i;
                       
                        c_add.Export_Id = exp_id;
                        db.Export_Country.Add(c_add);
                        db.SaveChanges();
                        model.msg = "country Added";
                    }

                }

                }
            if (exportvalues.SelectedItems != null)
            {
                foreach (var i in exportvalues.SelectedItems)
                {
                    var items_add = new Export_Item();
                    var units_total = from itm in db.License_Item where itm.Item_Id == i && itm.License_Id == lic_id.License_Id select itm.No_Units;
                    var exp_ids = db.Exports.Where(z => z.License_Id == lic_id.License_Id).Select(y => y.Id);

                    var sum_units = from f in db.Export_Item
                                    where exp_ids.Contains(f.Exporter_Id)
                                    group f by f.Item_Id into g
                                    orderby g.Sum(x => x.No_Of_Units)
                                    select
                                    new
                                    {
                                        item_code = g.Key,
                                       //item_value = g.First().,
                                       total = g.Sum(x => x.No_Of_Units)
                                    };

                    var sum = 0;

                    foreach (var n in sum_units)
                    {

                        if (n.item_code == i)
                        {
                            sum = n.total;
                            break;
                        }


                    }

                    var units_allowed = (units_total.SingleOrDefault()) - sum;
                    if ((
                        units_allowed) < exportvalues.No_Of_Units)
                    {
                        model.msg = "no of units can't  exceed than" + (
                        units_allowed);
                        view.msg = model.msg;
                        break;
                        //  return RedirectToAction("GetExpDetails", new { exp_id = exp_id.Value, msg = model.msg });
                    }
                    var item_found = from itm in db.Export_Item where itm.Item_Id == i && itm.Exporter_Id == exp_id select itm;

                    if (item_found.Any())
                    {
                        item_found.First().No_Of_Units = exportvalues.No_Of_Units;
                        db.SaveChanges();
                        var item_name = from itm in db.Items where itm.Id == i select itm.Name;
                        model.msg = "Item - " + item_name.FirstOrDefault() + "item updated ";
                        view.msg = model.msg;
                        //return RedirectToAction("GetExpDetails",new { exp_id = exp_id.Value, msg = model.msg });
                        break;
                        
                    }
                     

                    else
                    {
                        items_add.Item_Id = i;
                        items_add.No_Of_Units = exportvalues.No_Of_Units;
                        items_add.Exporter_Id = exp_id;
                        db.Export_Item.Add(items_add);
                        db.SaveChanges();
                        model.msg = "item Added";
                    }
                }

            }
           
           
            model.exp_id = exp_id;
            //  model.Items= new SelectListItem(item_list.ToList(), "Id", "Name")




            // db.SaveChanges();
            // return PartialView("EdititemPartialView", model);
            // return View(model);
            //   return RedirectToAction("GetExpDetails", new { exp_id = exp_id.Value, msg = "item added" });
            return Json(new { msg=model.msg},JsonRequestBehavior.AllowGet);
        }

        

            public ActionResult Saveedit(Export_view exportvalues)
        {

            if (ModelState.IsValid)
            {
                var export = from ex in db.Exports where ex.Id == exportvalues.export_val.Id select ex;

                export.First().Reference_No = exportvalues.export_val.Reference_No;
                export.First().Export_Date = exportvalues.export_val.Export_Date;
                
                db.SaveChanges();

            }
            return RedirectToAction("Details");


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
