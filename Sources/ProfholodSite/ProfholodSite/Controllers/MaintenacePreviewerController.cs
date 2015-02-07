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
    public class MaintenacePreviewerController : Controller
    {
        // GET: MaintenacePreviewer
        private MachineObjectContext db = new MachineObjectContext();

        public ActionResult Index()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            double ViewBefore_min = 10080;
            List<MaintenacesObject> filtered = new List<MaintenacesObject>();

            //  try {
            var maintenacesObjects =
                db.MaintenacesObjects.Include(m => m.MachineObject).Include(m => m.MaintenaceAction).Include(m => m.MaintenaceState).Include(m => m.Periodical)
                  .Where(m =>m.MaintenaceStateID == 2).Where(m=>m.DateEnd > DateTime.UtcNow);


            DateTime viewdatetime = DateTime.UtcNow.AddMinutes(ViewBefore_min);


            foreach (MaintenacesObject maintenace in maintenacesObjects.ToList())
            {

               // try
                {
                    var performMaintenanceReports = db.PerformMaintenanceReports.Include(p => p.MaintenacesObject)
                    //.Where(p => p.IsConfirm)
                    .Where(p =>p.MaintenacesObjectId == maintenace.Id)
                    ;

                    if (performMaintenanceReports.ToList().Count() == 0)
                    {
                        filtered.Add(maintenace);
                    }
                    else
                    {
                        DateTime lastdoing = performMaintenanceReports.AsEnumerable().Last().DateTimeEnd;
                        DateTime needdoing = lastdoing.AddSeconds(maintenace.Periodical.Time_period_sec);
                        if (needdoing < viewdatetime)
                        {
                            filtered.Add(maintenace);
                        }
                    }
                    
                }
                       // catch(SystemException) { filtered.Add(maintenace); }
            }

            //   }
            //   catch (SystemException )
            //  {

            // }

            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            ViewBag.CurrentUser = User.Identity.Name;
            return View(filtered);
        }

        public ActionResult Report(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            MaintenacesObject maintenacesObject = db.MaintenacesObjects.Find(id);
            if (maintenacesObject == null)
            {
                return HttpNotFound();
            }

            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (access_type != "Administrator" && access_type != "Chef" && access_type != "Specialist") throw new Exception("Access not denid");

            PerformMaintenanceReport obj = new PerformMaintenanceReport();
            obj.DateTimeEnd = obj.DateTimeStart = new MDTime().GetCurrentTime();
            obj.MaintenacesObjectId = maintenacesObject.Id;
            obj.MaintenacesObject = maintenacesObject;
            ViewBag.MaintenanceDescription = maintenacesObject.Description;

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report([Bind(Include = "Id,MaintenacesObjectId,DateTimeStart,DateTimeEnd,ReportText")] PerformMaintenanceReport performMaintenanceReport)
        {

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef" && access_type != "Specialist") throw new Exception("Access not denid");


            performMaintenanceReport.CreateUserName =
             performMaintenanceReport.ModifyUserName = User.Identity.Name;

            performMaintenanceReport.CreateDate =
                 performMaintenanceReport.ModifyDate = new MDTime().GetCurrentTime();

            performMaintenanceReport.IsConfirm = false;

            if (ModelState.IsValid)
            {
                db.PerformMaintenanceReports.Add(performMaintenanceReport);
                db.SaveChanges();
                
            }

            return RedirectToAction("Index");
        }

    }
}