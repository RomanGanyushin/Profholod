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
using System.IO;
using ProfholodSite.DataAcquisition;


namespace ProfholodSite.Controllers
{


    public class RealTimeViewerController : Controller
    {
        private DataAcquisitionContext db = new DataAcquisitionContext();

        //
        // GET: /RealTimeViewer/
        public class Material
        {
            public double S { get; set; }
            public double R { get; set; }
            public double E { get; set; }
            public double P { get; set; }
            public string D { get; set; }
            public string L { get; set; }
        };

        public string _c(DateTime s)
        {
            return s.Month.ToString("00") + "/" + s.Day.ToString("00") + "/" + s.Year.ToString() +
                " " + s.Hour.ToString("00") + ":" + s.Minute.ToString("00") + ":" + s.Second.ToString("00");
        }



        public JsonResult GetDataJson(string LastDate, int LastCount)
        {

            List<Material> Result = new List<Material>();
            if (LastCount == 0) LastCount = Int32.MaxValue;

            DateTime FromDateTime = new DateTime(2014, 1, 1, 0, 0, 0);
            if (LastDate != "none")
            {
                try { FromDateTime = DateTime.Parse(LastDate); }
                catch (SystemException e) { };
            }

            var castingProccess = db.CastingProccess.
                Where(m => m.RecordTime > FromDateTime).
                OrderByDescending(m => m.RecordTime).Take(LastCount).
                OrderBy(m => m.RecordTime).ToList();

            foreach (var m in castingProccess)
            {

                if (m.setMeterialA != 0)
                    Result.Add(new Material() { L = "A", D = _c(m.RecordTime), S = Math.Round(m.setMeterialA, 4), E = Math.Round(m.errorMeterialA, 4), R = Math.Round(m.realMeterialA, 4), P = Math.Round(m.errorMeterialA_Percent, 4) });

                if (m.setMeterialB != 0)
                    Result.Add(new Material() { L = "B", D = _c(m.RecordTime), S = Math.Round(m.setMeterialB, 4), E = Math.Round(m.errorMeterialB, 4), R = Math.Round(m.realMeterialB, 4), P = Math.Round(m.errorMeterialB_Percent, 4) });


                if (m.setMeterialC != 0)
                    Result.Add(new Material() { L = "C", D = _c(m.RecordTime), S = Math.Round(m.setMeterialC, 4), E = Math.Round(m.errorMeterialC, 4), R = Math.Round(m.realMeterialC, 4), P = Math.Round(m.errorMeterialC_Percent, 4) });


                if (m.setMeterialD != 0)
                    Result.Add(new Material() { L = "D", D = _c(m.RecordTime), S = Math.Round(m.setMeterialD, 4), E = Math.Round(m.errorMeterialD, 4), R = Math.Round(m.realMeterialD, 4), P = Math.Round(m.errorMeterialD_Percent, 4) });

                if (m.setMeterialE != 0)
                    Result.Add(new Material() { L = "E", D = _c(m.RecordTime), S = Math.Round(m.setMeterialE, 4), E = Math.Round(m.errorMeterialE, 4), R = Math.Round(m.realMeterialE, 4), P = Math.Round(m.errorMeterialE_Percent, 4) });

                if (m.setMeterialF != 0)
                    Result.Add(new Material() { L = "F", D = _c(m.RecordTime), S = Math.Round(m.setMeterialF, 4), E = Math.Round(m.errorMeterialF, 4), R = Math.Round(m.realMeterialF, 4), P = Math.Round(m.errorMeterialF_Percent, 4) });

                if (m.setMeterialN != 0)
                    Result.Add(new Material() { L = "N", D = _c(m.RecordTime), S = Math.Round(m.setMeterialN, 4), E = Math.Round(m.errorMeterialN, 4), R = Math.Round(m.realMeterialN, 4), P = Math.Round(m.errorMeterialN_Percent, 4) });

                if (m.setMeterialNuc != 0)
                    Result.Add(new Material() { L = "Nuc", D = _c(m.RecordTime), S = Math.Round(m.setMeterialNuc, 4), E = Math.Round(m.errorMeterialNuc, 4), R = Math.Round(m.realMeterialNuc, 4), P = Math.Round(m.errorMeterialNuc_Percent, 4) });

            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDataJson_DB(string LastDate, int LastCount)
        {
            List<Material> Result = new List<Material>();
            if (LastCount == 0) LastCount = Int32.MaxValue;

            DateTime FromDateTime = new DateTime(2014, 1, 1, 0, 0, 0);
            if (LastDate != "none")
            {
                try { FromDateTime = DateTime.Parse(LastDate); }
                catch (SystemException e) { };
            }

            var dbgeneralProccess = db.GeneralDoubleBeltProccess.
                Where(m => m.RecordTime > FromDateTime).
                OrderByDescending(m => m.RecordTime).Take(LastCount).
                OrderBy(m => m.RecordTime).ToList();

            foreach (var m in dbgeneralProccess)
            {

                if (m.setVelocity != 0)
                    Result.Add(new Material() { L = "A", D = _c(m.RecordTime), S = Math.Round(m.setVelocity, 4), E = Math.Round(m.errorVelocity, 4), R = Math.Round(m.realVelocity, 4), P = Math.Round(m.realVelocity_Percent, 4) });

            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetLastDataInfo()
        {
            var castingProccess = db.CastingProccess.OrderBy(m => m.RecordTime).ToList();

            if (castingProccess.Count() == 0)
            {
                return Json(new { isEmptyCastingTable = true }, JsonRequestBehavior.AllowGet);
            }

            var lRecord = castingProccess.Last();
            var CastingSession = db.CastingSessions.Where(m => m.OpenSessionDateTime == lRecord.CastingSessionId).ToList();

            var GeneralDB = db.GeneralDoubleBeltProccess.OrderBy(m => m.RecordTime).ToList();

            var Result = new
            {
                isEmptyCastingTable = false,
                lastRecordDate = _c(lRecord.RecordTime),
                castingStart = _c(lRecord.CastingSessionId),
                castingEnd = (CastingSession.Count() == 0) ? "" :
                    ((CastingSession.Last().CloseSessionCode == -1) ? "идет заливка ..." : _c(CastingSession.Last().CloseSessionDateTime)),


                dbVelocity = (GeneralDB.Count() == 0) ? "..." :
                   (Math.Abs((lRecord.RecordTime - GeneralDB.Last().RecordTime).TotalSeconds) < 5) ? GeneralDB.Last().realVelocity.ToString("0.00") : "<>"
            };


            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MatOnlineIndex()
        {
            return View();
        }

        public ActionResult DBVelocityOnlineIndex()
        {
            return View();
        }


    }
}