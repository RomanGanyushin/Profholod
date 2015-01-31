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
    public class MaintenaceStatesController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: MaintenaceStates
        public ActionResult Index()
        {
            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            return View(db.MaintenaceStates.ToList());
        }

        // GET: MaintenaceStates/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            return View();
        }

        // POST: MaintenaceStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] MaintenaceState maintenaceState)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            maintenaceState.CreateUserName =
               maintenaceState.ModifyUserName = User.Identity.Name;

            maintenaceState.CreateDate =
                 maintenaceState.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.MaintenaceStates.Add(maintenaceState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maintenaceState);
        }

        // GET: MaintenaceStates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenaceState maintenaceState = db.MaintenaceStates.Find(id);
            if (maintenaceState == null)
            {
                return HttpNotFound();
            }
            return View(maintenaceState);
        }

        // POST: MaintenaceStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,CreateDate,CreateUserName")] MaintenaceState maintenaceState)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            maintenaceState.ModifyUserName = User.Identity.Name;
            maintenaceState.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.Entry(maintenaceState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maintenaceState);
        }

        // GET: MaintenaceStates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenaceState maintenaceState = db.MaintenaceStates.Find(id);
            if (maintenaceState == null)
            {
                return HttpNotFound();
            }
            return View(maintenaceState);
        }

        // POST: MaintenaceStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            MaintenaceState maintenaceState = db.MaintenaceStates.Find(id);
            db.MaintenaceStates.Remove(maintenaceState);
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
