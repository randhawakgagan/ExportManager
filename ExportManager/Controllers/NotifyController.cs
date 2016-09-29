using ExportManager.DBModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExportManager.Controllers
{
   
    public class emaillist
    {
        public string address { get; set; }
    }

    public class emaildata
    {
        public List<emaillist> email { get; set; }
       public int lic_id { get; set; }
    }

    public class emaildelete
    {
        public string email { get; set; }
        public int lic_id { get; set; }
    }
    public class searchterm
    {
        public string search { get; set; }
    }
    public class emailadd
    {
        public int lic_id { get; set; } 
    }

    public class NotifyController : Controller
    {
        private LicenseManagerEntities db = new LicenseManagerEntities();


    
        // GET: Notify
        public ActionResult NotifyV()
        {
            //var userId = User.Identity.GetUserId();
            //var email_list = from emil in db.Notifies where emil.UserId == userId select emil;
            //if(id!=null)
            //{
            //    Notify found = db.Notifies.Find(id.Value);
            //    db.Notifies.Remove(found);
            //    db.SaveChanges();
            //}
            //return View(email_list.ToList());
            return View();
        }

        
             public JsonResult DeleteEmail(emaildelete data)
        {
            
            var userId = User.Identity.GetUserId();
            var email_id = from email in db.Notifies where email.UserId == userId && email.Email_Id == data.email && email.LicenseId==data.lic_id select email;
            Notify found = db.Notifies.Find(email_id.FirstOrDefault().Id);
            db.Notifies.Remove(found);
            db.SaveChanges();
            return Json(new { success = true });
        }



        //public JsonResult GetLicensedata(List<searchterm> search)
        public JsonResult GetLicensedata()
        {
            var userId = User.Identity.GetUserId();
            
         //   foreach (var search1 in search)
            {
               var  lic_no = (from L in db.Licenses where L.UserId == userId select new { lic_no = L.License_No ,lic_id=L.Id});
                return Json(new { lic_nos = lic_no }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            //if (id != null)
            //{
            //    Notify found = db.Notifies.Find(id.Value);
            //    db.Notifies.Remove(found);
            //    db.SaveChanges();
            //}

        }

        public JsonResult Emails(int? lic_id)
        {
           // if (lic_id.lic_id != null)
            {
                var userId = User.Identity.GetUserId();
                var email_list = (from emil in db.Notifies where emil.UserId == userId && emil.LicenseId == lic_id.Value select new { Emailid = emil.Email_Id });
                //if (id != null)
                //{
                //    Notify found = db.Notifies.Find(id.Value);
                //    db.Notifies.Remove(found);
                //    db.SaveChanges();
                //}
                return Json(new { emails = email_list }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true });
        }

        public JsonResult Save(emaildata data)
        {
            var list_addr = data.email.ToList();
            var userId = User.Identity.GetUserId();
            var lic_id = data.lic_id;
            var emailadd = new Notify();
            foreach (var addr in list_addr)
            {
                emailadd.UserId = userId;
                var emailtoadd=addr.address;
                emailadd.Email_Id = emailtoadd;
                emailadd.LicenseId = lic_id;
                db.Notifies.Add(emailadd);
                db.SaveChanges();
            }

            return Json(new { success=true});
        }
    }
}