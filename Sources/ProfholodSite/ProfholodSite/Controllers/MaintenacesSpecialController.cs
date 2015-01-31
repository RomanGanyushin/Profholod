

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
    public class MaintenacesSpecialController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

    
        // GET: MaintenacesObjects/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            ViewBag.MachineObjectId = new SelectList(db.MachineObjects, "Id", "Name");
            ViewBag.MaintenaceActionId = new SelectList(db.MaintenaceActions, "Id", "Caption");
            ViewBag.MaintenaceStateID = new SelectList(db.MaintenaceStates, "Id", "Name");
            ViewBag.PeriodicalID = new SelectList(db.Periodicals, "Id", "Name");

            MaintenacesObject obj = new MaintenacesObject();
            obj.DateEnd = DateTime.UtcNow.AddDays(7);

            return View(obj);
        }

        // POST: MaintenacesObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MaintenaceActionId,MachineObjectId,DateStart,DateEnd,MaintenaceStateID,Description")] MaintenacesObject maintenacesObject)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            maintenacesObject.PeriodicalID = 1;
            maintenacesObject.MaintenaceStateID = 1;
            maintenacesObject.DateStart = DateTime.UtcNow;

            maintenacesObject.CreateUserName =
               maintenacesObject.ModifyUserName = User.Identity.Name;

            maintenacesObject.CreateDate =
                 maintenacesObject.ModifyDate = DateTime.UtcNow;


            if (ModelState.IsValid)
            {
                db.MaintenacesObjects.Add(maintenacesObject);
                db.SaveChanges();
                return RedirectToAction("Index", "MaintenacesObjects");
            }

            ViewBag.MachineObjectId = new SelectList(db.MachineObjects, "Id", "Name", maintenacesObject.MachineObjectId);
            ViewBag.MaintenaceActionId = new SelectList(db.MaintenaceActions, "Id", "Caption", maintenacesObject.MaintenaceActionId);
            ViewBag.MaintenaceStateID = new SelectList(db.MaintenaceStates, "Id", "Name", maintenacesObject.MaintenaceStateID);
            ViewBag.PeriodicalID = new SelectList(db.Periodicals, "Id", "Name", maintenacesObject.PeriodicalID);
            return View(maintenacesObject);
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
