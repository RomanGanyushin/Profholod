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
    public class MaintenaceActionsController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: MaintenaceActions
        public ActionResult Index()
        {
            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            ViewBag.CurrentUser = User.Identity.Name;

            return View(db.MaintenaceActions.ToList());
        }

       

        // GET: MaintenaceActions/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            return View();
        }

        // POST: MaintenaceActions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Caption,Description")] MaintenaceAction maintenaceAction)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");


            maintenaceAction.CreateUserName =
               maintenaceAction.ModifyUserName = User.Identity.Name;

            maintenaceAction.CreateDate =
                 maintenaceAction.ModifyDate = DateTime.UtcNow;


            if (ModelState.IsValid)
            {
                db.MaintenaceActions.Add(maintenaceAction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maintenaceAction);
        }

        // GET: MaintenaceActions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            MaintenaceAction maintenaceAction = db.MaintenaceActions.Find(id);

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if(db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType!="Administrator"
                && maintenaceAction.CreateUserName!= User.Identity.Name) throw new Exception("Access not denid");

            if (maintenaceAction == null)
            {
                return HttpNotFound();
            }
            return View(maintenaceAction);
        }

        // POST: MaintenaceActions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Caption,Description,CreateDate,CreateUserName")] MaintenaceAction maintenaceAction)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && maintenaceAction.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            maintenaceAction.ModifyUserName = User.Identity.Name;
            maintenaceAction.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.Entry(maintenaceAction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maintenaceAction);
        }

        // GET: MaintenaceActions/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenaceAction maintenaceAction = db.MaintenaceActions.Find(id);

            if (maintenaceAction == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && maintenaceAction.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            return View(maintenaceAction);
        }

        // POST: MaintenaceActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MaintenaceAction maintenaceAction = db.MaintenaceActions.Find(id);

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && maintenaceAction.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            db.MaintenaceActions.Remove(maintenaceAction);
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
