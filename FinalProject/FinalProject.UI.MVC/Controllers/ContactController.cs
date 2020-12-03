using FinalProject.UI.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.UI.MVC.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        //Post: contact
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            //Message to send to me
            string message = $"You have received an email from <strong>{cvm.FirstName}</strong> with a subject <strong>{cvm.MessageTitle}</strong>. Please respond to <strong>{cvm.EmailAddress}</strong> with your response to the following message:<br />{cvm.Message}";

            //Setup Mail Message
            MailMessage mm = new MailMessage(
                    "admin@joshready.com",
                    "joshmready@gmail.com",
                    cvm.MessageTitle,
                    message
                );

            mm.IsBodyHtml = true;
            mm.Priority = MailPriority.High;
            mm.ReplyToList.Add(cvm.EmailAddress);
            SmtpClient client = new SmtpClient("mail.joshready.com");
            client.Credentials = new NetworkCredential("admin@joshready.com", "Chiefs01*");
            client.Port = 8889;

            //Try to send email
            try
            {
                client.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.CustomerMessage = $"We're sorry your request could not be completed at this time. Please try again later.<br />Error Message:<br />{ex.StackTrace}";
                return View(cvm);
            }


            return View("ContactConfirmation", cvm);
        }
    }
}