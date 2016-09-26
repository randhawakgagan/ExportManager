using ExportManager.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ExportManager.Models
{
    public class EmailSend
    {
        private LicenseManagerEntities db = new LicenseManagerEntities();
        public void send_expiry_mail()
        {

            var expiry = from l in db.Licenses select l;

            foreach (var lic in expiry)
            {
               // DateTime myDate = DateTime.ParseExact(lic.Expiry_Date.ToString(), "dd/MM/yyyy",
                                        //  System.Globalization.CultureInfo.InvariantCulture);

               if (lic.Expiry_Date.Date < DateTime.Today)
                {
                    var sendmail = from n in db.AspNetUsers where n.Id == lic.UserId select n.Email;
                    if (sendmail.Any())
                    {
                        //var sendmail = from n in db.Notifies where n.License_Id == lic.Id select n.Email_Id;
                        var tomail = sendmail.FirstOrDefault().ToString();
                        // var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                        var message = new MailMessage();
                        message.To.Add(new MailAddress(tomail.ToString()));  // replace with valid value 
                        message.From = new MailAddress("deep.gagan.randhawa@gmail.com");  // replace with valid value
                        message.Subject = "Your email subject";
                        message.Body = "lic expired";//string.Format(body, "", "", "License expired today");
                                                     // message.IsBodyHtml = true;


                        var smtp = new SmtpClient("smtp.gmail.com", 587);

                        smtp.Credentials = new NetworkCredential("deep.gagan.randhawa@gmail.com", "gmailgagan1");





                        //  smtp.Host = "smtp.gmail.com";
                        // smtp.Port = 587;
                        smtp.EnableSsl = true;

                        //   smtp.SendMailAsync(message);
                        smtp.Send(message);

                        // return RedirectToAction("Sent");

                    }
                }


            }
        }

    }
}