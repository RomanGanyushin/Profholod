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
    public class PerformNoteRepairsController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: PerformNoteRepairs
        public ActionResult Index()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            ViewBag.CurrentUser = User.Identity.Name;
            return View(db.PerformNoteRepairs.ToList());
        }

     

        // GET: PerformNoteRepairs/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            PerformNoteRepair performNoteRepair = new PerformNoteRepair();
            performNoteRepair.DateTimeStart =
                performNoteRepair.DateTimeEnd = new MDTime().GetCurrentTime();

            return View(performNoteRepair);
        }

        // POST: PerformNoteRepairs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateTimeStart,DateTimeEnd,RepairObjectText,TypeOfFaultText,RepairActionText,ReportText,IsCompleted")] PerformNoteRepair performNoteRepair)
        {
           
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            performNoteRepair.CreateUserName =
              performNoteRepair.ModifyUserName = User.Identity.Name;

            performNoteRepair.CreateDate =
                 performNoteRepair.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.PerformNoteRepairs.Add(performNoteRepair);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(performNoteRepair);
        }

        // GET: PerformNoteRepairs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerformNoteRepair performNoteRepair = db.PerformNoteRepairs.Find(id);
            if (performNoteRepair == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((performNoteRepair.CreateUserName != User.Identity.Name) ))
                throw new Exception("Access not denid");

            return View(performNoteRepair);
        }

        // POST: PerformNoteRepairs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateTimeStart,DateTimeEnd,RepairObjectText,TypeOfFaultText,RepairActionText,ReportText,IsCompleted,CreateUserName,CreateDate")] PerformNoteRepair performNoteRepair)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((performNoteRepair.CreateUserName != User.Identity.Name)))
                throw new Exception("Access not denid");

            performNoteRepair.ModifyUserName = User.Identity.Name;
            performNoteRepair.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.Entry(performNoteRepair).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(performNoteRepair);
        }

        // GET: PerformNoteRepairs/Delete/5
        public ActionResult Delete(int? id)
        {
           

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerformNoteRepair performNoteRepair = db.PerformNoteRepairs.Find(id);
            if (performNoteRepair == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((performNoteRepair.CreateUserName != User.Identity.Name)))
                throw new Exception("Access not denid");

            return View(performNoteRepair);
        }

        // POST: PerformNoteRepairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            

            PerformNoteRepair performNoteRepair = db.PerformNoteRepairs.Find(id);

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((performNoteRepair.CreateUserName != User.Identity.Name)))
                throw new Exception("Access not denid");

            db.PerformNoteRepairs.Remove(performNoteRepair);
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
