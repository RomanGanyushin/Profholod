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
    public class PerformMaintenanceReportsController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: PerformMaintenanceReports
        public ActionResult Index()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef" && access_type != "Specialist") throw new Exception("Access not denid");

            var performMaintenanceReports = db.PerformMaintenanceReports.Include(p => p.MaintenacesObject).Where(p => p.IsConfirm == false);

            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            ViewBag.CurrentUser = User.Identity.Name;
            return View(performMaintenanceReports.ToList());
        }

    
        // GET: PerformMaintenanceReports/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef" && access_type != "Specialist") throw new Exception("Access not denid");

            ViewBag.MaintenacesObjectId = new SelectList(db.MaintenacesObjects, "Id", "Description");

            PerformMaintenanceReport obj = new PerformMaintenanceReport();
            obj.DateTimeEnd = obj.DateTimeStart = new MDTime().GetCurrentTime();
         
            return View(obj);
        }

       

        // POST: PerformMaintenanceReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MaintenacesObjectId,DateTimeStart,DateTimeEnd,IsConfirm,ReportText")] PerformMaintenanceReport performMaintenanceReport)
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
                return RedirectToAction("Index");
            }

            ViewBag.MaintenacesObjectId = new SelectList(db.MaintenacesObjects, "Id", "Description", performMaintenanceReport.MaintenacesObjectId);
            return View(performMaintenanceReport);
        }

        // GET: PerformMaintenanceReports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef" && access_type != "Specialist") throw new Exception("Access not denid");



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerformMaintenanceReport performMaintenanceReport = db.PerformMaintenanceReports.Find(id);
            if (performMaintenanceReport == null)
            {
                return HttpNotFound();
            }

            if(access_type == "Specialist" && User.Identity.Name != performMaintenanceReport.CreateUserName)
                throw new Exception("Access not denid");
            if(performMaintenanceReport.IsConfirm!= false)
                throw new Exception("Access not denid");



            ViewBag.MaintenacesObjectId = new SelectList(db.MaintenacesObjects, "Id", "Description", performMaintenanceReport.MaintenacesObjectId);
            return View(performMaintenanceReport);
        }

        // POST: PerformMaintenanceReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaintenacesObjectId,DateTimeStart,DateTimeEnd,ReportText,,CreateDate,CreateUserName")] PerformMaintenanceReport performMaintenanceReport)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef" && access_type != "Specialist") throw new Exception("Access not denid");

            if (access_type == "Specialist" && User.Identity.Name != performMaintenanceReport.CreateUserName)
                throw new Exception("Access not denid");
           

            performMaintenanceReport.ModifyUserName = User.Identity.Name;
            performMaintenanceReport.ModifyDate = new MDTime().GetCurrentTime();
            performMaintenanceReport.IsConfirm = false;


            if (ModelState.IsValid)
            {
                db.Entry(performMaintenanceReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaintenacesObjectId = new SelectList(db.MaintenacesObjects, "Id", "Description", performMaintenanceReport.MaintenacesObjectId);
            return View(performMaintenanceReport);
        }

        // GET: PerformMaintenanceReports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef" && access_type != "Specialist") throw new Exception("Access not denid");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerformMaintenanceReport performMaintenanceReport = db.PerformMaintenanceReports.Find(id);

            if (access_type == "Specialist" && User.Identity.Name != performMaintenanceReport.CreateUserName)
                throw new Exception("Access not denid");
            if (performMaintenanceReport.IsConfirm != false)
                throw new Exception("Access not denid");

            if (performMaintenanceReport == null)
            {
                return HttpNotFound();
            }
            return View(performMaintenanceReport);
        }

        // POST: PerformMaintenanceReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PerformMaintenanceReport performMaintenanceReport = db.PerformMaintenanceReports.Find(id);

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef" && access_type != "Specialist") throw new Exception("Access not denid");
            if (access_type == "Specialist" && User.Identity.Name != performMaintenanceReport.CreateUserName)
                throw new Exception("Access not denid");
            if (performMaintenanceReport.IsConfirm != false)
                throw new Exception("Access not denid");

            db.PerformMaintenanceReports.Remove(performMaintenanceReport);
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
