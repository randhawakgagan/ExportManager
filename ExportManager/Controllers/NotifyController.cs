﻿using ExportManager.DBModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExportManager.Controllers
{
   
    public class email
    {
        public string address { get; set; }
    }
    public class searchterm
    {
        public string search { get; set; }
    }

    public class NotifyController : Controller
    {
        private LicenseManagerEntities db = new LicenseManagerEntities();


        public JsonResult Getcity()
        {
            var city = (from L in db.Licenses select L.License_No ).ToList();
            return Json(city, JsonRequestBehavior.AllowGet);

        }
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

        
             public JsonResult DeleteEmail(string Emailid)
        {
            
            var userId = User.Identity.GetUserId();
            var email_id = from email in db.Notifies where email.UserId == userId && email.Email_Id == Emailid select email;
            Notify found = db.Notifies.Find(email_id.FirstOrDefault().Id);
            db.Notifies.Remove(found);
            db.SaveChanges();
            return Json(new { success = true });
        }

        

            public JsonResult GetLicensedata(List<searchterm> search)
        {
            var userId = User.Identity.GetUserId();
            
            foreach (var search1 in search)
            {
               var  lic_no = (from L in db.Licenses where L.UserId == userId && ((search1.search == null) || L.License_No.StartsWith(search1.search)) select new { lic_no = L.License_No });
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

        public JsonResult Emails()
        {
            var userId = User.Identity.GetUserId();
            var email_list = (from emil in db.Notifies where emil.UserId == userId select new { Emailid=emil.Email_Id });
            //if (id != null)
            //{
            //    Notify found = db.Notifies.Find(id.Value);
            //    db.Notifies.Remove(found);
            //    db.SaveChanges();
            //}
            return Json(new { emails=email_list}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(List<email> newemail)
        {
            var list_addr =newemail.ToList();
            var userId = User.Identity.GetUserId();

            var emailadd = new Notify();
            foreach (var addr in list_addr)
            {
                emailadd.UserId = userId;
                var emailtoadd=addr.address;
                emailadd.Email_Id = emailtoadd;
                db.Notifies.Add(emailadd);
                db.SaveChanges();
            }

            return Json(new { success=true});
        }
    }
}