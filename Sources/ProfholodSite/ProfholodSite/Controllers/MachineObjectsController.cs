using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProfHolodSite.Models;
using System.Web.Script.Serialization;

namespace ProfholodSite.Controllers
{
    public class MachineObjectsController : Controller
    {
        private MachineObjectContext db = new MachineObjectContext();

        // GET: MachineObjects
        public ActionResult Index()
        {
            ViewBag.AccessType = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            ViewBag.CurrentUser = User.Identity.Name;
            var machineObjects = db.MachineObjects.Include(m => m.MachineObjectGroup).Include(m => m.ParentObject);
            return View(machineObjects.ToList());
        }

        public ActionResult TreeView()
        {  
            return View();
        }

        public class ListItem {
            public Int32 Id { set; get; }
            public Int32 ParentId { set; get; }
            public  string Text { set; get; }
        };

        public JsonResult GetDataJson()
        {
         
            var list = new List<ListItem>();
            var machineObjects = db.MachineObjects.Include(m => m.MachineObjectGroup).Include(m => m.ParentObject);
            foreach (MachineObject m in machineObjects.ToList())
            {
                ListItem l = new ListItem() { Text = m.Name, Id = m.Id, ParentId = m.ParentObjectId };
                if (l.Id == l.ParentId) l.ParentId = 0;
                list.Add(l);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }



    // GET: MachineObjects/Create
    public ActionResult Create()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");

            ViewBag.MachineObjectGroupId = new SelectList(db.MachineObjectGroups, "Id", "GroupName");
            ViewBag.ParentObjectId = new SelectList(db.MachineObjects, "Id", "Name");
            return View();
        }

        // POST: MachineObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParentObjectId,MachineObjectGroupId,Name,Code")] MachineObject machineObject)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");


            machineObject.CreateUserName =
               machineObject.ModifyUserName = User.Identity.Name;

            machineObject.CreateDate =
                 machineObject.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.MachineObjects.Add(machineObject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MachineObjectGroupId = new SelectList(db.MachineObjectGroups, "Id", "GroupName", machineObject.MachineObjectGroupId);
            ViewBag.ParentObjectId = new SelectList(db.MachineObjects, "Id", "Name", machineObject.ParentObjectId);
            return View(machineObject);
        }

        // GET: MachineObjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MachineObject machineObject = db.MachineObjects.Find(id);
            if (machineObject == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && machineObject.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            ViewBag.MachineObjectGroupId = new SelectList(db.MachineObjectGroups, "Id", "GroupName", machineObject.MachineObjectGroupId);
            ViewBag.ParentObjectId = new SelectList(db.MachineObjects, "Id", "Name", machineObject.ParentObjectId);
            return View(machineObject);
        }

        // POST: MachineObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentObjectId,MachineObjectGroupId,Name,Code,CreateDate,CreateUserName")] MachineObject machineObject)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && machineObject.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            machineObject.ModifyUserName = User.Identity.Name;
            machineObject.ModifyDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                db.Entry(machineObject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MachineObjectGroupId = new SelectList(db.MachineObjectGroups, "Id", "GroupName", machineObject.MachineObjectGroupId);
            ViewBag.ParentObjectId = new SelectList(db.MachineObjects, "Id", "Name", machineObject.ParentObjectId);
            return View(machineObject);
        }

        // GET: MachineObjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MachineObject machineObject = db.MachineObjects.Find(id);
            if (machineObject == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && machineObject.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            return View(machineObject);
        }

        // POST: MachineObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MachineObject machineObject = db.MachineObjects.Find(id);

            if (User.Identity.Name == "") throw new Exception("Access not denid");
            if (db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType != "Administrator"
                && machineObject.CreateUserName != User.Identity.Name)
                throw new Exception("Access not denid");

            db.MachineObjects.Remove(machineObject);
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
