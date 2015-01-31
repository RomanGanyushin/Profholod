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
    public class MachineObjectGroupsController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: MachineObjectGroups
        public ActionResult Index()
        {
            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            ViewBag.CurrentUser = User.Identity.Name;
            return View(db.MachineObjectGroups.ToList());
        }

        // GET: MachineObjectGroups/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            return View();
        }

        // POST: MachineObjectGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GroupName,Description")] MachineObjectGroup machineObjectGroup)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");


            machineObjectGroup.CreateUserName =
               machineObjectGroup.ModifyUserName = User.Identity.Name;

            machineObjectGroup.CreateDate =
                 machineObjectGroup.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.MachineObjectGroups.Add(machineObjectGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(machineObjectGroup);
        }

        // GET: MachineObjectGroups/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MachineObjectGroup machineObjectGroup = db.MachineObjectGroups.Find(id);

            

            if (machineObjectGroup == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && machineObjectGroup.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            return View(machineObjectGroup);
        }

        // POST: MachineObjectGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GroupName,Description,CreateDate,CreateUserName")] MachineObjectGroup machineObjectGroup)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && machineObjectGroup.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            machineObjectGroup.ModifyUserName = User.Identity.Name;
            machineObjectGroup.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.Entry(machineObjectGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(machineObjectGroup);
        }

        // GET: MachineObjectGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MachineObjectGroup machineObjectGroup = db.MachineObjectGroups.Find(id);
            if (machineObjectGroup == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && machineObjectGroup.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            return View(machineObjectGroup);
        }

        // POST: MachineObjectGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MachineObjectGroup machineObjectGroup = db.MachineObjectGroups.Find(id);

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && machineObjectGroup.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            db.MachineObjectGroups.Remove(machineObjectGroup);
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
