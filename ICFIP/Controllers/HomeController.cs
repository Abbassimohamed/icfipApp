using ICFIP.Common.Config;
using ICFIP.Entites;
using ICFIP.Filters;
using ICFIP.Services.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;


namespace ICFIP.Controllers
{
    [AllowAnonymous]
    [InitializeSimpleMembershipAttribute]
    [ExceptionFilter]
    [LoggerFilter]
    public class HomeController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
       [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                //string s = "ee";
                //int a = Convert.ToInt32(s);

                return View();
            }
            catch(Exception ex)
            {
                return RedirectToAction("Erreur",ex.Message);

                //throw new Exception(ex.Message,ex.InnerException);
            }
           
        }

        public ActionResult Erreur(string message)
        {
            ViewBag.MsgErreur = message;
            return View();
        }

        [Authorize(Roles="Admin")]
          [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }




        
          [AllowAnonymous]
        public ActionResult Contact(string emailBody)
        {
            ViewBag.Message = "Contact ICFIP";
          

              return View();
        }



          [HttpPost]
          public ActionResult Contacts(Sendmail Model)
          {
              string Text = "<html> <head> </head>" +
              " <body style= \" font-size:12px; font-family: Arial\">" +
              "Nom :"+ Model.Name+"<br>"
              +
              "Télèphone :"+ Model.Phone+"<br>"
                +
                "Email :" + Model.Email + "<br>"
            +
               "Question :" + Model.Message 
              +
              "</body></html>";


              


              string smtpAddress = "smtp.gmail.com";
              int portNumber = 587;
              bool enableSSL = true;

              string emailFrom = "anasabassi11@gmail.com";
              string password = "bigboss1992";
              string emailTo = Model.Email;
              string subject = "Hello";
              string body = Text;

              using (MailMessage mail = new MailMessage())
              {
                  mail.From = new MailAddress(emailFrom);
                  mail.To.Add(emailTo);
                  mail.Subject = subject;
                  mail.Body = body;
                  mail.IsBodyHtml = true;


                  using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                  {
                      smtp.Credentials = new NetworkCredential(emailFrom, password);
                      smtp.EnableSsl = enableSSL;
                      smtp.Send(mail);
                      return RedirectToAction("Index");
                  }
              }



             
              
          }


     
      
        

    }
}
