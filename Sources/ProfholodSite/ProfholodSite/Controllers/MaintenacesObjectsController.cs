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
    public class MaintenacesObjectsController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: MaintenacesObjects
        public ActionResult Index()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            ViewBag.CurrentUser = User.Identity.Name;
        
            var maintenacesObjects = db.MaintenacesObjects.Include(m => m.MachineObject).Include(m => m.MaintenaceAction).Include(m => m.MaintenaceState).Include(m => m.Periodical);
            return View(maintenacesObjects.ToList());
        }

        public ActionResult ConfirmRegular()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type  =db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if(access_type!="Administrator" && access_type!="Chef") throw new Exception("Access not denid");


            ViewBag.AccessType = access_type;
            ViewBag.CurrentUser = User.Identity.Name;

            var maintenacesObjects = db.MaintenacesObjects.Include(m => m.MachineObject).Include(m => m.MaintenaceAction).Include(m => m.MaintenaceState).Include(m => m.Periodical);
            return View(maintenacesObjects.Where(m=> m.MaintenaceStateID == 1).Where(m => m.PeriodicalID != 1).ToList());
        }

        public ActionResult ConfirmOnce()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef") throw new Exception("Access not denid");


            ViewBag.AccessType = access_type;
            ViewBag.CurrentUser = User.Identity.Name;

            var maintenacesObjects = db.MaintenacesObjects.Include(m => m.MachineObject).Include(m => m.MaintenaceAction).Include(m => m.MaintenaceState).Include(m => m.Periodical);
            return View(maintenacesObjects.Where(m => m.MaintenaceStateID == 1).Where(m => m.PeriodicalID == 1).ToList());
        }


        public ActionResult ConfirmRegularTrue(int? id)
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

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef") throw new Exception("Access not denid");

            maintenacesObject.MaintenaceStateID = 2;
            db.Entry(maintenacesObject).State = EntityState.Modified;
            db.SaveChanges();

            if (maintenacesObject.PeriodicalID!=1)
                return RedirectToAction("ConfirmRegular");

                return RedirectToAction("ConfirmOnce");

        }




        // GET: MaintenacesObjects/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            ViewBag.MachineObjectId = new SelectList(db.MachineObjects, "Id", "Name");
            ViewBag.MaintenaceActionId = new SelectList(db.MaintenaceActions, "Id", "Caption");
            ViewBag.MaintenaceStateID = new SelectList(db.MaintenaceStates, "Id", "Name");
            ViewBag.PeriodicalID = new SelectList(db.Periodicals, "Id", "Name");

            MaintenacesObject obj = new MaintenacesObject();
            obj.DateStart = DateTime.Parse("01-01-2012");
            obj.DateEnd = DateTime.Parse("01-01-2022");
            return View(obj);
        }

        // POST: MaintenacesObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MaintenaceActionId,MachineObjectId,PeriodicalID,DateStart,DateEnd,MaintenaceStateID,Description")] MaintenacesObject maintenacesObject)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");


            maintenacesObject.CreateUserName =
               maintenacesObject.ModifyUserName = User.Identity.Name;

            maintenacesObject.CreateDate =
                 maintenacesObject.ModifyDate = DateTime.UtcNow;

            maintenacesObject.MaintenaceStateID = 1; /// Dlya Utvergdeniya

            if (ModelState.IsValid)
            {
                db.MaintenacesObjects.Add(maintenacesObject);
                db.SaveChanges();
              
                return RedirectToAction("Index");
            }

            ViewBag.MachineObjectId = new SelectList(db.MachineObjects, "Id", "Name", maintenacesObject.MachineObjectId);
            ViewBag.MaintenaceActionId = new SelectList(db.MaintenaceActions, "Id", "Caption", maintenacesObject.MaintenaceActionId);
            ViewBag.MaintenaceStateID = new SelectList(db.MaintenaceStates, "Id", "Name", maintenacesObject.MaintenaceStateID);
            ViewBag.PeriodicalID = new SelectList(db.Periodicals, "Id", "Name", maintenacesObject.PeriodicalID);
            return View(maintenacesObject);
        }

        // GET: MaintenacesObjects/Edit/5
        public ActionResult Edit(int? id)
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

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((maintenacesObject.CreateUserName != User.Identity.Name) || (maintenacesObject.MaintenaceStateID!=1)))
                throw new Exception("Access not denid");

            ViewBag.MachineObjectId = new SelectList(db.MachineObjects, "Id", "Name", maintenacesObject.MachineObjectId);
            ViewBag.MaintenaceActionId = new SelectList(db.MaintenaceActions, "Id", "Caption", maintenacesObject.MaintenaceActionId);
            ViewBag.MaintenaceStateID = new SelectList(db.MaintenaceStates, "Id", "Name", maintenacesObject.MaintenaceStateID);
            ViewBag.PeriodicalID = new SelectList(db.Periodicals, "Id", "Name", maintenacesObject.PeriodicalID);
            return View(maintenacesObject);
        }

        // POST: MaintenacesObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaintenaceActionId,MachineObjectId,PeriodicalID,DateStart,DateEnd,MaintenaceStateID,Description,CreateDate,CreateUserName")] MaintenacesObject maintenacesObject)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((maintenacesObject.CreateUserName != User.Identity.Name) || (maintenacesObject.MaintenaceStateID != 1)))
                throw new Exception("Access not denid");

            maintenacesObject.ModifyUserName = User.Identity.Name;
            maintenacesObject.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.Entry(maintenacesObject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MachineObjectId = new SelectList(db.MachineObjects, "Id", "Name", maintenacesObject.MachineObjectId);
            ViewBag.MaintenaceActionId = new SelectList(db.MaintenaceActions, "Id", "Caption", maintenacesObject.MaintenaceActionId);
            ViewBag.MaintenaceStateID = new SelectList(db.MaintenaceStates, "Id", "Name", maintenacesObject.MaintenaceStateID);
            ViewBag.PeriodicalID = new SelectList(db.Periodicals, "Id", "Name", maintenacesObject.PeriodicalID);
            return View(maintenacesObject);
        }

        // GET: MaintenacesObjects/Delete/5
        public ActionResult Delete(int? id)
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

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((maintenacesObject.CreateUserName != User.Identity.Name) || (maintenacesObject.MaintenaceStateID != 1)))
                throw new Exception("Access not denid");

            return View(maintenacesObject);
        }

        // POST: MaintenacesObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MaintenacesObject maintenacesObject = db.MaintenacesObjects.Find(id);

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((maintenacesObject.CreateUserName != User.Identity.Name) || (maintenacesObject.MaintenaceStateID != 1)))
                throw new Exception("Access not denid");

            db.MaintenacesObjects.Remove(maintenacesObject);
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
