using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfHolodSite.Models;

namespace ProfholodSite.Controllers
{
    public class HomeController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        public ActionResult Index()
        {
            if (User.Identity.Name == "")
            {
                return RedirectToAction("Login", "Account" ,routeValues: null);
            }
            
            StaffPerson person = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First();
                
            ViewBag.AccessType = person.AccessType;
            ViewBag.CurrentUser = User.Identity.Name;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}