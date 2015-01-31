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
    public class PeriodicalsController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: Periodicals
        public ActionResult Index()
        {
            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            return View(db.Periodicals.ToList());
        }

       
        // GET: Periodicals/Create
        public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            return View();
        }

        // POST: Periodicals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Time_period_sec,LCode")] Periodical periodical)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName  == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            periodical.CreateUserName =
                periodical.ModifyUserName= User.Identity.Name;

            periodical.CreateDate =
                 periodical.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.Periodicals.Add(periodical);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(periodical);
        }

        // GET: Periodicals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodical periodical = db.Periodicals.Find(id);
            if (periodical == null)
            {
                return HttpNotFound();
            }
            return View(periodical);
        }

        // POST: Periodicals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Time_period_sec,LCode,CreateDate,CreateUserName")] Periodical periodical)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

                periodical.ModifyUserName = User.Identity.Name;
                periodical.ModifyDate = DateTime.UtcNow;
            

            if (ModelState.IsValid)
            {
                db.Entry(periodical).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(periodical);
        }

        // GET: Periodicals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodical periodical = db.Periodicals.Find(id);
            if (periodical == null)
            {
                return HttpNotFound();
            }
            return View(periodical);
        }

        // POST: Periodicals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator")
                throw new Exception("Access not denid");

            Periodical periodical = db.Periodicals.Find(id);
            db.Periodicals.Remove(periodical);
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
