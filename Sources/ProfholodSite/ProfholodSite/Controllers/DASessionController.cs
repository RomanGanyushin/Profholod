using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProfholodSite.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProfholodSite.DataAcquisition;

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

        public JsonResult Login()
        {
            var a =  HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            var result =  a.PasswordSignIn("thunder_glfx@list.ru", "62_Lawic", true, shouldLockout: false);
            return Json("Ok", JsonRequestBehavior.AllowGet);  
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
            string s = User.Identity.Name;
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

    
        public JsonResult PutAlarmRecord(string IdAlarmSession, string RecordDateTime, string Message)
        {
            try
            {
                var js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var objs = js.Deserialize<Dictionary<string, object>>(Message).Values;
                List<Int32> newalarms = new List<Int32>();

                /// Добавляем новые записи
                if (objs.ToList()[0] != "")
                {     
                    foreach (System.Collections.ArrayList item in objs)
                        foreach (Dictionary<string, object> _item in item)
                        {
                            string code_1 = _item["Code_1"].ToString();
                            int code_2 = Int32.Parse(_item["Code_2"].ToString());
                            int code_3 = Int32.Parse(_item["Code_3"].ToString());

                            Int32 alarm_id = db.GetNotNullPLCAlarm(code_1, code_2, code_3);

                            AlarmMessage alarmMessage = new AlarmMessage()
                            {
                                Begin = DateTime.Parse(RecordDateTime),
                                End = DateTime.Parse(RecordDateTime),
                                PLCAlarmId = alarm_id,
                                AlarmsSessionId = DateTime.Parse(IdAlarmSession)
                            }; newalarms.Add(alarm_id);

                            db.AlarmMessages.Add(alarmMessage);
                            db.SaveChanges();
                        }
                }

                    /// Закрываем старые
                    var notClosedRecords =  db.AlarmMessages
                        .Where(p =>p.Begin == p.End);

                    foreach (var for_close in notClosedRecords.ToList())
                    {
                        bool doClose = true;
                        foreach (var newalarm in newalarms)
                            if (for_close.PLCAlarmId == newalarm)
                                doClose = false;

                        if (doClose)
                        {
                            for_close.End = DateTime.Parse(RecordDateTime);
                            db.Entry(for_close).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

               
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }

            catch (SystemException) { }
            return Json("Error", JsonRequestBehavior.AllowGet);

        }

        public JsonResult PutMaterialRecord(string IdCastingSession, string RecordDateTime, string Message)
        {
            try
            {
                var js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var objs = js.Deserialize<Dictionary<string, string>>(Message);

               
                CastingProccessTable proc = new CastingProccessTable()
                {
                    RecordTime = DateTime.Parse(RecordDateTime),
                    CastingSessionId = DateTime.Parse(IdCastingSession),

                    setMeterialA = double.Parse(objs["DB_ForPC_S_Port_A"].Replace(".", ",")),
                    realMeterialA = double.Parse(objs["DB_ForPC_R_Port_A"].Replace(".", ",")),
                    errorMeterialA = double.Parse(objs["ERR_Port_A"].Replace(".", ",")),

                    setMeterialB = double.Parse(objs["DB_ForPC_S_Port_B"].Replace(".", ",")),
                    realMeterialB = double.Parse(objs["DB_ForPC_R_Port_B"].Replace(".", ",")),
                    errorMeterialB = double.Parse(objs["ERR_Port_B"].Replace(".", ",")),

                    setMeterialC = double.Parse(objs["DB_ForPC_S_Port_C"].Replace(".", ",")),
                    realMeterialC = double.Parse(objs["DB_ForPC_R_Port_C"].Replace(".", ",")),
                    errorMeterialC = double.Parse(objs["ERR_Port_C"].Replace(".", ",")),

                    setMeterialD = double.Parse(objs["DB_ForPC_S_Port_D"].Replace(".", ",")),
                    realMeterialD = double.Parse(objs["DB_ForPC_R_Port_D"].Replace(".", ",")),
                    errorMeterialD = double.Parse(objs["ERR_Port_D"].Replace(".", ",")),

                    setMeterialE = double.Parse(objs["DB_ForPC_S_Port_E"].Replace(".", ",")),
                    realMeterialE = double.Parse(objs["DB_ForPC_R_Port_E"].Replace(".", ",")),
                    errorMeterialE = double.Parse(objs["ERR_Port_E"].Replace(".", ",")),

                    setMeterialF = double.Parse(objs["DB_ForPC_S_Port_F"].Replace(".", ",")),
                    realMeterialF = double.Parse(objs["DB_ForPC_R_Port_F"].Replace(".", ",")),
                    errorMeterialF = double.Parse(objs["ERR_Port_F"].Replace(".", ",")),

                    setMeterialN = double.Parse(objs["DB_ForPC_S_Port_N"].Replace(".", ",")),
                    realMeterialN = double.Parse(objs["DB_ForPC_R_Port_N"].Replace(".", ",")),
                    errorMeterialN = double.Parse(objs["ERR_Port_N"].Replace(".", ",")),

                    setMeterialNuc = double.Parse(objs["DB_ParPmp_SetPorNuc"].Replace(".", ",")),
                    realMeterialNuc = double.Parse(objs["DB_ParPmp_RealPortNuc"].Replace(".", ",")),
                    errorMeterialNuc = double.Parse(objs["ERR_Port_Nuc"].Replace(".", ","))

                };

                if (db.CastingProccess.Count() > 0)
                {
                    var _last = db.CastingProccess.OrderBy(p => p.RecordTime).ToList().Last();
                    if (_last.RecordTime >= proc.RecordTime)
                    {
                        return Json("Ok", JsonRequestBehavior.AllowGet);
                    }
                }
                
                if (db.CastingProccess.Count() < 1000)
                {
                    db.CastingProccess.Add(proc);               
                }
                else
                {
                    var _replace = db.CastingProccess.OrderBy(p => p.RecordTime).First();
                    _replace.CopyFrom(proc);
                    db.Entry(_replace).State = EntityState.Modified;
                }
                db.SaveChanges();

                return Json("Ok", JsonRequestBehavior.AllowGet);
            }

            catch (SystemException e) { }
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
