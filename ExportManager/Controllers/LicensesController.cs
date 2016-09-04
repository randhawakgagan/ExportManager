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

namespace ExportManager.Controllers
{
    

    public class LicensesController : Controller
    {
        private LicenseManagerEntities db = new LicenseManagerEntities();

        // GET: Licenses
        public ActionResult Index(int? lic_id,int ? item_id)
        {

            var model = new Viewlicensecountryitem();
            model.Licenses = db.Licenses.Include(i => i.License_Country.Select(z => z.Country)).Include(y => y.License_Item.Select(p => p.Item));

            //model.exp_item= model.exports.Include(y => y.Item);
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
            //List<Viewlicensecountryitem> model = new List<Viewlicensecountryitem>();

            //StringBuilder item_name = new StringBuilder();

            //var id = from a in db.Licenses select a;
            //foreach(var i in id)
            //{
            //    Viewlicensecountryitem list_view = new Viewlicensecountryitem();
            //    list_view.allLicense = i;

            //    var query =
            //   from itm in db.Items
            //   join lic in db.License_Item on itm.Id equals lic.Item_Id
            //   where lic.License_Id == i.Id
            //   select new { name= itm.Name };

            //    foreach (var n in query)
            //    {
            //        item_name.Append(n.name);
            //        item_name.Append(",");
            //    }


            //    list_view.allItems = item_name.ToString();
            //    item_name.Clear();

            //    var query1 =
            //    from itm in db.Countries
            //    join lic in db.License_Country on itm.Id equals lic.Country_Id
            //    where lic.License_Id== i.Id
            //    select new { name = itm.Name };

            //    foreach (var n in query1)
            //    {
            //        item_name.Append(n.name);
            //        item_name.Append(",");
            //    }


            //    list_view.allCountry = item_name.ToString();
            //    model.Add(list_view);
            //    item_name.Clear();

            //}


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
                    
                    
                    var no= from a in db.Licenses
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
