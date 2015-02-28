using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProfholodSite.Models;

namespace ProfholodSite.Controllers
{
    public class DASessionController : Controller
    {
        private DataAcquisitionContext db = new DataAcquisitionContext();

        // GET: /DASession/
        public ActionResult Index()
        {
            return View(db.DataAcquisitionSessions.ToList());
        }

        // GET: /DASession/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataAcquisitionSession dataacquisitionsession = db.DataAcquisitionSessions.Find(id);
            if (dataacquisitionsession == null)
            {
                return HttpNotFound();
            }
            return View(dataacquisitionsession);
        }


        public JsonResult CreateSessionAcquisition(int IDTable,int OpenSessionCode, string OpenSessionDateTime)
        {
          
            try
            {
               

                if (ModelState.IsValid)
                {
                    switch(IDTable)
                    {
                        case 0:
                            {
                                DataAcquisitionSession dataacquisitionsession = new DataAcquisitionSession();

                                dataacquisitionsession.OpenSessionCode = OpenSessionCode;
                                dataacquisitionsession.CloseSessionDateTime =
                                 dataacquisitionsession.OpenSessionDateTime = DateTime.Parse(OpenSessionDateTime);
                                dataacquisitionsession.CloseSessionCode = -1;
                                db.DataAcquisitionSessions.Add(dataacquisitionsession);
                            }
                            break;

                        case 1:
                            {
                                LinearSession dataacquisitionsession = new LinearSession();

                                dataacquisitionsession.OpenSessionCode = OpenSessionCode;
                                dataacquisitionsession.CloseSessionDateTime =
                                 dataacquisitionsession.OpenSessionDateTime = DateTime.Parse(OpenSessionDateTime);
                                dataacquisitionsession.CloseSessionCode = -1;
                                db.LinearSessions.Add(dataacquisitionsession);
                          
                            }
                            break;

                            case 2:
                            {
                                CastingSession dataacquisitionsession = new CastingSession();

                                dataacquisitionsession.OpenSessionCode = OpenSessionCode;
                                dataacquisitionsession.CloseSessionDateTime =
                                 dataacquisitionsession.OpenSessionDateTime = DateTime.Parse(OpenSessionDateTime);
                                dataacquisitionsession.CloseSessionCode = -1;
                                db.CastingSessions.Add(dataacquisitionsession);
                          
                            }
                            break;

                            case 3:
                            {
                                AlarmsSession dataacquisitionsession = new AlarmsSession();

                                dataacquisitionsession.OpenSessionCode = OpenSessionCode;
                                dataacquisitionsession.CloseSessionDateTime =
                                 dataacquisitionsession.OpenSessionDateTime = DateTime.Parse(OpenSessionDateTime);
                                dataacquisitionsession.CloseSessionCode = -1;
                                db.AlarmsSessions.Add(dataacquisitionsession);

                            }
                            break;
                            
                        default:
                        throw new SystemException();
                    }
                    db.SaveChanges();
                    return Json("Ok", JsonRequestBehavior.AllowGet);   
                }
            }
            catch (SystemException) { }
            return Json("Error", JsonRequestBehavior.AllowGet);     
        }

        public JsonResult IsCreateSessionAcquisition(int IDTable, string OpenSessionDateTime)
        {
            try
            {
        

                switch (IDTable)
                {
                    case 0:
                        {
                            DataAcquisitionSession dataacquisitionsession = db.DataAcquisitionSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            return Json((dataacquisitionsession == null) ? "No" : "Yes", JsonRequestBehavior.AllowGet); 
                        }
                        break;

                    case 1:
                        {
                            LinearSession dataacquisitionsession = db.LinearSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            return Json((dataacquisitionsession == null) ? "No" : "Yes", JsonRequestBehavior.AllowGet); 
                        }
                        break;

                    case 2:
                        {
                            CastingSession dataacquisitionsession = db.CastingSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            return Json((dataacquisitionsession == null) ? "No" : "Yes", JsonRequestBehavior.AllowGet);
                        }
                        break;

                        case 3:
                        {
                            AlarmsSession dataacquisitionsession = db.AlarmsSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            return Json((dataacquisitionsession == null) ? "No" : "Yes", JsonRequestBehavior.AllowGet);
                        }
                        break;


                        

                    default:
                    throw new SystemException();
                }
            }
            catch (SystemException) { }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsClosedSessionAcquisition(int IDTable, string OpenSessionDateTime)
        {
            try
            {
                switch (IDTable)
                {
                    case 0:
                        {
                            DataAcquisitionSession dataacquisitionsession = db.DataAcquisitionSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            if (dataacquisitionsession == null) throw new SystemException();
                            return Json((dataacquisitionsession.CloseSessionCode == -1) ? "No" : "Yes", JsonRequestBehavior.AllowGet);
                        }
                        break;

                    case 1:
                        {
                            LinearSession dataacquisitionsession = db.LinearSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            if (dataacquisitionsession == null) throw new SystemException();
                            return Json((dataacquisitionsession.CloseSessionCode == -1) ? "No" : "Yes", JsonRequestBehavior.AllowGet);
                        }
                        break;

                          case 2:
                        {
                            CastingSession dataacquisitionsession = db.CastingSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            if (dataacquisitionsession == null) throw new SystemException();
                            return Json((dataacquisitionsession.CloseSessionCode == -1) ? "No" : "Yes", JsonRequestBehavior.AllowGet);
                        }
                        break;

                         case 3:
                        {
                            AlarmsSession dataacquisitionsession = db.AlarmsSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            if (dataacquisitionsession == null) throw new SystemException();
                            return Json((dataacquisitionsession.CloseSessionCode == -1) ? "No" : "Yes", JsonRequestBehavior.AllowGet);
                        }
                        break;

                        
                        
                    default:
                        throw new SystemException();
                }

               
            }
            catch (SystemException) { }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult CloseSessionAcquisition(int IDTable,string OpenSessionDateTime, int CloseSessionCode, string CloseSessionDateTime)
        {
            try
            {
                switch (IDTable)
                {
                    case 0:
                        {
                            DataAcquisitionSession dataacquisitionsession = db.DataAcquisitionSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            if (dataacquisitionsession == null) throw new SystemException();
                            dataacquisitionsession.CloseSessionCode = CloseSessionCode;
                            dataacquisitionsession.CloseSessionDateTime = DateTime.Parse(CloseSessionDateTime);

                            db.Entry(dataacquisitionsession).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        break;

                    case 1:
                        {
                            LinearSession dataacquisitionsession = db.LinearSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            if (dataacquisitionsession == null) throw new SystemException();
                            dataacquisitionsession.CloseSessionCode = CloseSessionCode;
                            dataacquisitionsession.CloseSessionDateTime = DateTime.Parse(CloseSessionDateTime);

                            db.Entry(dataacquisitionsession).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        break;

                          case 2:
                        {
                            CastingSession dataacquisitionsession = db.CastingSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            if (dataacquisitionsession == null) throw new SystemException();
                            dataacquisitionsession.CloseSessionCode = CloseSessionCode;
                            dataacquisitionsession.CloseSessionDateTime = DateTime.Parse(CloseSessionDateTime);

                            db.Entry(dataacquisitionsession).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        break;

                        case 3:
                        {
                            AlarmsSession dataacquisitionsession = db.AlarmsSessions.Find(DateTime.Parse(OpenSessionDateTime));
                            if (dataacquisitionsession == null) throw new SystemException();
                            dataacquisitionsession.CloseSessionCode = CloseSessionCode;
                            dataacquisitionsession.CloseSessionDateTime = DateTime.Parse(CloseSessionDateTime);

                            db.Entry(dataacquisitionsession).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        break;
                        
                        

                    default:
                        throw new SystemException();
                }

               

                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            catch (SystemException) { }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

       


        // GET: /DASession/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataAcquisitionSession dataacquisitionsession = db.DataAcquisitionSessions.Find(id);
            if (dataacquisitionsession == null)
            {
                return HttpNotFound();
            }
            return View(dataacquisitionsession);
        }

        // POST: /DASession/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,OpenSessionCode,CloseSessionCode,Name")] DataAcquisitionSession dataacquisitionsession)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dataacquisitionsession).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dataacquisitionsession);
        }

        // GET: /DASession/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataAcquisitionSession dataacquisitionsession = db.DataAcquisitionSessions.Find(id);
            if (dataacquisitionsession == null)
            {
                return HttpNotFound();
            }
            return View(dataacquisitionsession);
        }

        // POST: /DASession/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataAcquisitionSession dataacquisitionsession = db.DataAcquisitionSessions.Find(id);
            db.DataAcquisitionSessions.Remove(dataacquisitionsession);
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
