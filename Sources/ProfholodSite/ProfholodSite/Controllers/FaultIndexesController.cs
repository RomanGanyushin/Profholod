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
    public class FaultIndexesController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: FaultIndexes
        public ActionResult Index()
        {
            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            return View(db.FaultIndexs.ToList());
        }


        // GET: FaultIndexes/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            return View();
        }

        // POST: FaultIndexes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] FaultIndex faultIndex)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            faultIndex.CreateUserName =
                faultIndex.ModifyUserName = User.Identity.Name;

            faultIndex.CreateDate =
                 faultIndex.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.FaultIndexs.Add(faultIndex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(faultIndex);
        }

        // GET: FaultIndexes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FaultIndex faultIndex = db.FaultIndexs.Find(id);
            if (faultIndex == null)
            {
                return HttpNotFound();
            }
            return View(faultIndex);
        }

        // POST: FaultIndexes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,CreateUserName,CreateDate")] FaultIndex faultIndex)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            faultIndex.ModifyUserName = User.Identity.Name;
            faultIndex.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.Entry(faultIndex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(faultIndex);
        }

        // GET: FaultIndexes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FaultIndex faultIndex = db.FaultIndexs.Find(id);
            if (faultIndex == null)
            {
                return HttpNotFound();
            }
            return View(faultIndex);
        }

        // POST: FaultIndexes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            FaultIndex faultIndex = db.FaultIndexs.Find(id);
            db.FaultIndexs.Remove(faultIndex);
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
