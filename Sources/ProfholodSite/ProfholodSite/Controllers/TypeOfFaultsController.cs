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
    public class TypeOfFaultsController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: TypeOfFaults
        public ActionResult Index()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            ViewBag.CurrentUser = User.Identity.Name;

            var typeOfFaults = db.TypeOfFaults.Include(t => t.FaultIndex);
            return View(typeOfFaults.ToList());
        }

        // GET: TypeOfFaults/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            ViewBag.FaultIndexId = new SelectList(db.FaultIndexs, "Id", "Name");
            return View();
        }

        // POST: TypeOfFaults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,LCode,FaultIndexId")] TypeOfFault typeOfFault)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            typeOfFault.CreateUserName =
              typeOfFault.ModifyUserName = User.Identity.Name;

            typeOfFault.CreateDate =
                 typeOfFault.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.TypeOfFaults.Add(typeOfFault);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FaultIndexId = new SelectList(db.FaultIndexs, "Id", "Name", typeOfFault.FaultIndexId);
            return View(typeOfFault);
        }

        // GET: TypeOfFaults/Edit/5
        public ActionResult Edit(int? id)
        {
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfFault typeOfFault = db.TypeOfFaults.Find(id);
            if (typeOfFault == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((typeOfFault.CreateUserName != User.Identity.Name)))
                throw new Exception("Access not denid");

            ViewBag.FaultIndexId = new SelectList(db.FaultIndexs, "Id", "Name", typeOfFault.FaultIndexId);
            return View(typeOfFault);
        }

        // POST: TypeOfFaults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,LCode,FaultIndexId,CreateUserName,CreateDate")] TypeOfFault typeOfFault)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((typeOfFault.CreateUserName != User.Identity.Name)))
                throw new Exception("Access not denid");

            typeOfFault.ModifyUserName = User.Identity.Name;
            typeOfFault.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.Entry(typeOfFault).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FaultIndexId = new SelectList(db.FaultIndexs, "Id", "Name", typeOfFault.FaultIndexId);
            return View(typeOfFault);
        }

        // GET: TypeOfFaults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfFault typeOfFault = db.TypeOfFaults.Find(id);


            if (typeOfFault == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && ((typeOfFault.CreateUserName != User.Identity.Name)))
                throw new Exception("Access not denid");

            return View(typeOfFault);
        }

        // POST: TypeOfFaults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeOfFault typeOfFault = db.TypeOfFaults.Find(id);
            db.TypeOfFaults.Remove(typeOfFault);
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
