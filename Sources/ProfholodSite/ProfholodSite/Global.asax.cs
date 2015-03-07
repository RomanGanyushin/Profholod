using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Net;
using System.Net.Mail;
using System.Data.Entity;
using ProfHolodSite.Models;
using ProfholodSite.DataAcquisition;

namespace ProfholodSite
{

    public class MvcApplication : System.Web.HttpApplication
    {
        private System.Threading.Timer timer4h;

      

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
           
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

    
           // Database.SetInitializer(new DataAcquisitioneInitializer());

            timer4h = new System.Threading.Timer(new System.Threading.TimerCallback(DoRun4h), null, 0, 1000 * 60 * 60 * 4);
        }
   
        private void DoRun4h(object state)
        {
            NetworkCredential loginpass = new NetworkCredential("thunder_glfx@list.ru", "thunderaspect");
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.mail.ru";
            smtp.Port = 25;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = loginpass;
            smtp.EnableSsl= (true);

            MailMessage oMsg = new MailMessage("thunder_glfx@list.ru", "thunder_glfx@list.ru");
            oMsg.Subject = "Tema";
            oMsg.IsBodyHtml = false;
            oMsg.Body = "hekko fro";
         //   smtp.Send(oMsg);

        }
    }

}
