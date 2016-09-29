using ExportManager.DBModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExportManager.Controllers
{

    public class LicenseAddController : Controller
    {
        private LicenseManagerEntities db = new LicenseManagerEntities();


        // GET: LicenseAdd
        /* public ActionResult Index()
         {
             return View(db.Licenses.ToList());
         }*/

        public ActionResult Create()
        {
            // Country c = new Country();
            LicenseAdd Licenseview = new LicenseAdd();
            Licenseview.SelectedCountries = db.Countries.Select(x => x.Id);
            Licenseview.Counties = db.Countries
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                })
                .ToList();

            Licenseview.SelectedItems = db.Items.Select(x => x.Id);
            Licenseview.Items = db.Items
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                })
                .ToList();
            var item_unit = new item_units();
            Licenseview.Itms_db = new List<item_units>();
            item_unit.Itm_names = "item_name";
            item_unit.Itm_No_Units = 0;

            Licenseview.Itms_db.Add(item_unit);

            // pass the view model to the view
            return View(Licenseview);

            /*var Country_list = from d in db.Countries
                                orderby d.Name
                                select d;*/
            //   ViewBag.Country_list = new SelectList(Country_list.ToList(), "Id", "Name");

            //  ViewBag.Country_list = new MultiSelectList(Country_list.ToList(), "Id", "Name");
            //  return View();
        }

      //  [HttpGet]
        public ActionResult Additem(int? lic_id,LicenseAdd licenseadd)
        {
            if (lic_id != null)
            {
                LicenseAdd Licenseview1 = new LicenseAdd();
                TempData["Lic"] = lic_id.Value;
                Licenseview1.Id = lic_id.Value;
                Licenseview1.SelectedItems = db.Items.Select(x => x.Id);
                Licenseview1.Items = db.Items
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name,
                    })
                    .ToList();

                /*  var item_unit = new item_units();
                  Licenseview.Itms_db = new List<item_units>();
                  item_unit.Itm_names = "item_name";
                  item_unit.Itm_No_Units = 0;

                  Licenseview.Itms_db.Add(item_unit);*/

                // pass the view model to the view
                return View(Licenseview1);
            }
            else
            {
               // var license_id = licenseadd.Id;
              int  license_id = Convert.ToInt32(TempData["Lic"]);

                TempData["Lic"] = license_id;
                // int val = Convert.ToInt32(license_id.FirstOrDefault());
                if (licenseadd.SelectedItems != null)
                {
                    foreach (var i in licenseadd.SelectedItems)
                    {
                        var items_add = new License_Item();
                        var item_found = from itm in db.License_Item where itm.Item_Id == i && itm.License_Id == license_id select itm;
                        if (!item_found.Any())
                        {
                            items_add.Item_Id = i;

                            items_add.License_Id = license_id;
                            items_add.No_Units = licenseadd.No_Units;
                            db.License_Item.Add(items_add);
                        }
                        else
                        {

                            item_found.FirstOrDefault().No_Units = licenseadd.No_Units;


                        }
                    }
                    db.SaveChanges();
                }
                var item_names =
                from itm in db.Items
                join lic in db.License_Item on itm.Id equals lic.Item_Id
                where lic.License_Id == license_id
                select new { item_name = itm.Name };

                var item_units =
               from lic in db.License_Item
               where lic.License_Id == license_id
               select new { no_Units = lic.No_Units };


                var name_unit = item_names.ToList().Zip(item_units.ToList(), (n, w) => new { name = n, units = w });
               
                licenseadd.Itms_db = new List<item_units>();
                foreach (var n in name_unit.ToList())
                {
                    var item_unit = new item_units();
                    item_unit.Itm_names = n.name.item_name;
                    item_unit.Itm_No_Units = n.units.no_Units;
                    licenseadd.Itms_db.Add(item_unit);


                }

                licenseadd.SelectedItems = db.Items.Select(x => x.Id);
                licenseadd.Items = db.Items
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name,
                    })
                    .ToList();


                // return PartialView("Additem_list", licenseadd);
                // return cont
                return View(licenseadd);
            }
            //  return Json(new { lic_i
        }

        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public ActionResult Add_item( LicenseAdd licenseadd)

        {
           // if (ModelState.IsValid)
            {
                var license_id = licenseadd.Id;
                license_id = Convert.ToInt32(TempData["Lic"]);

               // int val = Convert.ToInt32(license_id.FirstOrDefault());
                foreach (var i in licenseadd.SelectedItems)
                {
                    var items_add = new License_Item();

                    items_add.Item_Id = i;

                    items_add.License_Id = license_id;
                    items_add.No_Units = licenseadd.No_Units;
                    db.License_Item.Add(items_add);

                }
                db.SaveChanges();
                var item_names =
                from itm in db.Items
                join lic in db.License_Item on itm.Id equals lic.Item_Id
                where lic.License_Id == license_id
                select new { item_name = itm.Name };

                var item_units =
               from lic in db.License_Item
               where lic.License_Id == license_id
               select new { no_Units = lic.No_Units };


                var name_unit = item_names.ToList().Zip(item_units.ToList(), (n, w) => new { name = n, units = w });
                //var item_units = new item_units();
                //item_units.Itm_No_Units=
                //licenseadd.Itms_db
              //  LicenseAdd licenseadd = new LicenseAdd();
                licenseadd.Itms_db = new List<item_units>();
                foreach (var n in name_unit.ToList())
                {
                    var item_unit = new item_units();
                    item_unit.Itm_names = n.name.item_name;
                    item_unit.Itm_No_Units = n.units.no_Units;
                    licenseadd.Itms_db.Add(item_unit);


                }

               


                // return cont
                return View(licenseadd);
                //  return Json(new { lic_id= license_id });
                //  return RedirectToAction("Index", "Licenses");
            }

           // return View(licenseadd);
        }


    
    // POST: Licenses/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(LicenseAdd licenseadd)
    {
        if (ModelState.IsValid)
        {
            License l = new License();
            l.License_No = licenseadd.License_No;
            l.Expiry_Date = licenseadd.Expiry_Date;
            l.Notes = licenseadd.Notes;
            l.UserId  = User.Identity.GetUserId();

            db.Licenses.Add(l);
            db.SaveChanges();
            var license_id = from a in db.Licenses
                             where a.License_No == licenseadd.License_No
                             select a.Id;
                var val= Convert.ToInt32(license_id.FirstOrDefault());
                TempData["ID"] = Convert.ToInt32(license_id.FirstOrDefault());
            foreach (var i in licenseadd.SelectedCountries)
            {
                var n = new License_Country();
                n.Country_Id = i;
                n.License_Id = Convert.ToInt32(license_id.FirstOrDefault());
                db.License_Country.Add(n);

            }

            /*     foreach (var i in licenseadd.SelectedItems)
                 {
                     var items_add = new License_Item();

                     items_add.Item_Id= i;

                     items_add.License_Id = Convert.ToInt32(license_id.FirstOrDefault());
                     db.License_Item.Add(items_add);

                 }*/

            db.SaveChanges();


            // return cont
           // return Content("License Created");
            //  return Json(new { lic_id= license_id });
             return RedirectToAction("Additem",new  { lic_id= val});
        }

        return View(licenseadd);
    }


}


    
}