using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProfHolodSite.Models;

namespace ProfholodSite.Controllers
{
    public class ConfirmPerformMaintenanceController : Controller
    {
        // GET: ConfirmPerformMaintenance
        private MachineObjectContext db = new MachineObjectContext();

        public ActionResult Index()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef") throw new Exception("Access not denid");

            var performMaintenanceReports = db.PerformMaintenanceReports.Include(p => p.MaintenacesObject)
                .Where(p=>p.IsConfirm == false);
            return View(performMaintenanceReports.ToList());
        }

        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PerformMaintenanceReport performMaintenanceReport = db.PerformMaintenanceReports.Find(id);
            if (performMaintenanceReport == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef") throw new Exception("Access not denid");

            performMaintenanceReport.IsConfirm = true;
            db.Entry(performMaintenanceReport).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
