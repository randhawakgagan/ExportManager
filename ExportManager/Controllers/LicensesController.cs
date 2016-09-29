using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExportManager.DBModel;
using System.Text;
using Microsoft.AspNet.Identity;
using PagedList;

namespace ExportManager.Controllers
{


    public class LicensesController : Controller
    {
        private LicenseManagerEntities db = new LicenseManagerEntities();

        
        public ActionResult Index(int? lic_id, int? item_id, string searchterm = null, int page = 1)
        {



            ViewBag.search = searchterm;
            var model = new Viewlicensecountryitem();
            var userId = User.Identity.GetUserId();

            var lic_ids = from l in db.Licenses where l.UserId == userId select l.Id;
            var ids = lic_ids.ToList();
            model.Licenses = db.Licenses.Where(r => ((searchterm == null || r.License_No.Contains(searchterm)) && (ids.Contains(r.Id)))).Include(i => i.License_Country.Select(z => z.Country)).Include(y => y.License_Item.Select(p => p.Item)).OrderBy(u => u.License_No).ToPagedList(page, 4);

           
            if (lic_id != null)
            {
                var items = db.Licenses.Where(x => x.Id == lic_id.Value).Include(z => z.License_Item.Select(y => y.Item));

                model.License_item = items;
                ViewBag.t_id = lic_id.Value;



            }
            if (item_id != null)
            {
                var countries = db.Licenses.Where(x => x.Id == item_id.Value).Include(z => z.License_Country.Select(y => y.Country));

                model.License_country = countries;
                ViewBag.c_id = item_id.Value;



            }
           


            return View(model);
        }

        // GET: Licenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = db.Licenses.Find(id);
            if (license == null)
            {
                return HttpNotFound();
            }
            return View(license);
        }

        // GET: Licenses/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Licenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "License_No,Expiry_Date,Notes")] License license, [Bind(Include = "Name")]Country c)
        {
            if (ModelState.IsValid)
            {
                db.Licenses.Add(license);
                //   foreach(var i in country_id)
                {
                    var n = new License_Country();
                    var c_id = from a in db.Countries
                               where a.Name == c.Name
                               select a.Id;
                    n.Country_Id = Convert.ToInt32(c_id.FirstOrDefault());


                    var no = from a in db.Licenses
                             where a.License_No == license.License_No
                             select a.Id;
                    n.License_Id = Convert.ToInt32(no.FirstOrDefault());
                    db.License_Country.Add(n);

                }
                // db.Countries.Add(country);
                // db.Items.Add(item);            
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(license);
        }

        // GET: Licenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = db.Licenses.Find(id);
            if (license == null)
            {
                return HttpNotFound();
            }
            return View(license);
        }

        // POST: Licenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,License_No,Expiry_Date,Notes")] License license)
        {
            if (ModelState.IsValid)
            {
                db.Entry(license).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(license);
        }

        // GET: Licenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = db.Licenses.Find(id);
            if (license == null)
            {
                return HttpNotFound();
            }
            return View(license);
        }



        public PartialViewResult itempartialView(int? Lic_id)
        {


            TempData["id"] = Lic_id.Value;
            LicenseEditView model = new LicenseEditView();

            model.item = db.Licenses.Where(z => z.Id == Lic_id.Value).Include(x => x.License_Item.Select(y => y.Item));

            var item_list =
              from itm in db.Items
              join lic in db.License_Item on itm.Id equals lic.Item_Id
              where lic.License_Id == Lic_id.Value
              select itm;

           
            //  model.Items= new SelectListItem(item_list.ToList(), "Id", "Name")

            model.Items =
             db.Items.Select(x => new SelectListItem
             {
                 Value = x.Id.ToString(),
                 Text = x.Name,
             })
             .ToList();

            model.SelectedItems = db.Items.Select(x => x.Id);


           
            return PartialView("EdititemPartialView", model);
        }





        public PartialViewResult CpartialView(int? Lic_id)
        {

            LicenseEditView model = new LicenseEditView();
            model.country = db.Licenses.Where(z => z.Id == Lic_id.Value).Include(x => x.License_Country.Select(y => y.Country));

            var item_list =
             from itm in db.Countries
             join lic in db.License_Country on itm.Id equals lic.Country_Id
             where lic.License_Id == Lic_id.Value
             select itm;

            model.Lic_id = Lic_id.Value;
            //  model.Items= new SelectListItem(item_list.ToList(), "Id", "Name")

            model.Items =
             db.Countries.Select(x => new SelectListItem
             {
                 Value = x.Id.ToString(),
                 Text = x.Name,
             })
             .ToList();

            model.SelectedCountries = db.Countries.Select(x => x.Id);






          
            return PartialView("EditCountryPartialView", model);
        }

        public ActionResult GetCDetails(int? Lic_id)
        {

            var model = new LicenseEditView();
            if(Lic_id==null)
            { return Content("lic not found "); }
            model.Lic_details = db.Licenses.Where(x => x.Id == Lic_id.Value).Single();


            var items = db.Licenses.Where(x => x.Id == Lic_id.Value).Include(z => z.License_Item.Select(y => y.Item));

            model.item = items;
            
            var country = db.Licenses.Where(x => x.Id == Lic_id.Value).Include(z => z.License_Country.Select(y => y.Country));

            model.country = country;
            

            return PartialView("LicCounEdit", model);
        }

        public ActionResult GetitemDetails(int? Lic_id)
        {

            var model = new LicenseEditView();
            
            if(Lic_id==null)
                return Content("lic not found ");

            model.Lic_details = db.Licenses.Where(x => x.Id == Lic_id.Value).Single();


            var items = db.Licenses.Where(x => x.Id == Lic_id.Value).Include(z => z.License_Item.Select(y => y.Item));

            model.item = items;
           
            var country = db.Licenses.Where(x => x.Id == Lic_id.Value).Include(z => z.License_Country.Select(y => y.Country));

            model.country = country;
            

            return PartialView("LicItemEdit", model);
        }
        public JsonResult Additem(LicenseEditView Licvalues)
        {
           
            var lic_id = Licvalues.Lic_id;
            ViewExportAdd model = new ViewExportAdd();
           
            if (Licvalues.SelectedCountries != null)
            {
                foreach (var i in Licvalues.SelectedCountries)
                {
                    var c_add = new License_Country();
                    var item_found = from itm in db.License_Country where itm.Country_Id == i && itm.License_Id == lic_id select itm;

                    if (item_found.Any())
                    {

                        var item_name = from itm in db.Countries where itm.Id == i select itm.Name;
                        model.msg = "Item - " + item_name.FirstOrDefault() + "item already added ";
                        //view.msg = model.msg;
                        //return RedirectToAction("GetExpDetails",new { exp_id = exp_id.Value, msg = model.msg });
                        break;

                    }


                    else
                    {
                        c_add.Country_Id = i;

                        c_add.License_Id = lic_id;
                        db.License_Country.Add(c_add);
                        db.SaveChanges();
                        model.msg = "country Added";
                    }

                }

            }
            if (Licvalues.SelectedItems != null)
            {
                foreach (var i in Licvalues.SelectedItems)
                {
                    var items_add = new License_Item();


                    var item_found = from itm in db.License_Item where itm.Item_Id == i && itm.License_Id == lic_id select itm;

                    if (item_found.Any())
                    {
                        item_found.First().No_Units = Licvalues.No_Of_Units;
                        db.SaveChanges();
                        var item_name = from itm in db.Items where itm.Id == i select itm.Name;
                        model.msg = "Item - " + item_name.FirstOrDefault() + "item updated ";
                        //view.msg = model.msg;
                        //return RedirectToAction("GetExpDetails",new { exp_id = exp_id.Value, msg = model.msg });
                        break;

                    }


                    else
                    {
                        items_add.Item_Id = i;
                        items_add.No_Units = Licvalues.No_Of_Units;
                        items_add.License_Id = lic_id;
                        db.License_Item.Add(items_add);
                        db.SaveChanges();
                        model.msg = "item Added";
                    }
                }

            }


            model.lic_id = lic_id;
           
            return Json(new { msg = model.msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Saveedit(LicenseEditView values)
        {

            if (ModelState.IsValid)
            {
                var lic_change = from lic in db.Licenses where lic.Id == values.Lic_details.Id select lic;

                lic_change.First().License_No = values.Lic_details.License_No;
                lic_change.First().Expiry_Date = values.Lic_details.Expiry_Date;
                lic_change.First().Notes = values.Lic_details.Notes;
                db.SaveChanges();

            }
            return RedirectToAction("Index");


        }
        public ActionResult Deleteitem(int? Lic_id, List<int> itemCheckedIds, List<int> countryCheckboxes)
        {
            var model = new LicenseEditView();
            if(Lic_id==null)
            return Content("lic not found ");
            model.Lic_details = db.Licenses.Where(x => x.Id == Lic_id.Value).Single();




            if (itemCheckedIds != null)
            {
                foreach (var itm_id in itemCheckedIds)
                {
                    License_Item lic_item = db.License_Item.Find(itm_id);
                    db.License_Item.Remove(lic_item);
                    db.SaveChanges();
                }

                var item = db.Licenses.Where(x => x.Id == Lic_id.Value).Include(z => z.License_Item.Select(y => y.Item));

                model.item = item;
                return PartialView("LicItemEdit", model);
            }
            if (countryCheckboxes != null)
            {
                foreach (var c_id in countryCheckboxes)
                {
                    License_Country c_item = db.License_Country.Find(c_id);
                    db.License_Country.Remove(c_item);
                    db.SaveChanges();
                }

                var country = db.Licenses.Where(x => x.Id == Lic_id.Value).Include(z => z.License_Country.Select(y => y.Country));

                model.country = country;


                return PartialView("LicCounEdit", model);
            }

            return PartialView("LicCounEdit", model);
        }
        public ActionResult EditLicense(int? Lic_id, string msg)
        {

            if (Lic_id == null)
                return Content("lic not found ");
            var model = new LicenseEditView();
            
            model.msg = msg;
            model.Lic_details = db.Licenses.Where(x => x.Id == Lic_id.Value).Single();


            var items = db.Licenses.Where(x => x.Id == Lic_id.Value).Include(z => z.License_Item.Select(y => y.Item));

            model.item = items;
            //    ViewBag.t_id = exp_id.Value;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Itemdisplay", model);
            }
          
            var country = db.Licenses.Where(x => x.Id == Lic_id.Value).Include(z => z.License_Country.Select(y => y.Country));

            model.country = country;
            



            return View(model);
           


        }


        // POST: Licenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            License license = db.Licenses.Find(id);
            db.Licenses.Remove(license);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
