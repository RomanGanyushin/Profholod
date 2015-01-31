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
    public class ComplitedRepairReportsController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: ComplitedRepairReports
        public ActionResult Index()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            var complitedRepairReports = db.ComplitedRepairReports.Include(c => c.MachineObject).Include(c => c.MaintenaceAction).Include(c => c.TypeOfFault);

            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            ViewBag.CurrentUser = User.Identity.Name;
            return View(complitedRepairReports.ToList());
        }

   

        // GET: ComplitedRepairReports/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            ViewBag.MachineObjectId = new SelectList(db.MachineObjects, "Id", "Name");
            ViewBag.MaintenaceActionId = new SelectList(db.MaintenaceActions, "Id", "Caption");
            ViewBag.TypeOfFaultId = new SelectList(db.TypeOfFaults, "Id", "Name");

            ComplitedRepairReport complitedRepairReport = new ComplitedRepairReport();
            complitedRepairReport.DateTimeStart =
                complitedRepairReport.DateTimeEnd = new MyLocalResource().GetCurrentTime();

            return View(complitedRepairReport);
        }

        // POST: ComplitedRepairReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MachineObjectId,TypeOfFaultId,MaintenaceActionId,DateTimeStart,DateTimeEnd,ReportText,IsCompleted")] ComplitedRepairReport complitedRepairReport)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            complitedRepairReport.CreateUserName =
              complitedRepairReport.ModifyUserName = User.Identity.Name;

            complitedRepairReport.CreateDate =
                 complitedRepairReport.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.ComplitedRepairReports.Add(complitedRepairReport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MachineObjectId = new SelectList(db.MachineObjects, "Id", "Name", complitedRepairReport.MachineObjectId);
            ViewBag.MaintenaceActionId = new SelectList(db.MaintenaceActions, "Id", "Caption", complitedRepairReport.MaintenaceActionId);
            ViewBag.TypeOfFaultId = new SelectList(db.TypeOfFaults, "Id", "Name", complitedRepairReport.TypeOfFaultId);
            return View(complitedRepairReport);
        }

        // GET: ComplitedRepairReports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplitedRepairReport complitedRepairReport = db.ComplitedRepairReports.Find(id);
            if (complitedRepairReport == null)
            {
                return HttpNotFound();
            }
            ViewBag.MachineObjectId = new SelectList(db.MachineObjects, "Id", "Name", complitedRepairReport.MachineObjectId);
            ViewBag.MaintenaceActionId = new SelectList(db.MaintenaceActions, "Id", "Caption", complitedRepairReport.MaintenaceActionId);
            ViewBag.TypeOfFaultId = new SelectList(db.TypeOfFaults, "Id", "Name", complitedRepairReport.TypeOfFaultId);

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((complitedRepairReport.CreateUserName != User.Identity.Name)))
                throw new Exception("Access not denid");

            return View(complitedRepairReport);
        }

        // POST: ComplitedRepairReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MachineObjectId,TypeOfFaultId,MaintenaceActionId,DateTimeStart,DateTimeEnd,ReportText,IsCompleted,CreateUserName,CreateDate")] ComplitedRepairReport complitedRepairReport)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((complitedRepairReport.CreateUserName != User.Identity.Name)))
                throw new Exception("Access not denid");
            complitedRepairReport.ModifyUserName = User.Identity.Name;
            complitedRepairReport.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.Entry(complitedRepairReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MachineObjectId = new SelectList(db.MachineObjects, "Id", "Name", complitedRepairReport.MachineObjectId);
            ViewBag.MaintenaceActionId = new SelectList(db.MaintenaceActions, "Id", "Caption", complitedRepairReport.MaintenaceActionId);
            ViewBag.TypeOfFaultId = new SelectList(db.TypeOfFaults, "Id", "Name", complitedRepairReport.TypeOfFaultId);
            return View(complitedRepairReport);
        }

        // GET: ComplitedRepairReports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplitedRepairReport complitedRepairReport = db.ComplitedRepairReports.Find(id);
            if (complitedRepairReport == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((complitedRepairReport.CreateUserName != User.Identity.Name)))
                throw new Exception("Access not denid");

            return View(complitedRepairReport);
        }

        // POST: ComplitedRepairReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ComplitedRepairReport complitedRepairReport = db.ComplitedRepairReports.Find(id);
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((complitedRepairReport.CreateUserName != User.Identity.Name)))
                throw new Exception("Access not denid");

            db.ComplitedRepairReports.Remove(complitedRepairReport);
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
