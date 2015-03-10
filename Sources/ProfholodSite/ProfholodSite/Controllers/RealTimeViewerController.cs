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
            public DateTime D { get; set; }
        };

        public JsonResult GetDataJson(string Material, string LastDate)
        {
            List <Material>    Result = new List<Material>();      
            

            DateTime FromDateTime = new DateTime(2014, 1, 1, 0, 0, 0);
            if (LastDate != null)
            {
                try { FromDateTime = DateTime.Parse(LastDate); }
                catch (SystemException e) { };
            }

            var castingProccess = db.CastingProccess.Where(m => m.RecordTime > FromDateTime).OrderBy(m => m.RecordTime).ToList();

            foreach (var m in castingProccess)
            {
                if (Material == "A")
                {
                    if (m.setMeterialA != 0)
                        Result.Add(new Material() { D = m.RecordTime, S = Math.Round(m.setMeterialA, 4), E = Math.Round(m.errorMeterialA, 4), R = Math.Round(m.realMeterialA, 4), P = Math.Round(m.errorMeterialA_Percent, 4) });
                }
                else if (Material == "B")
                {
                    if (m.setMeterialB != 0)
                        Result.Add(new Material() { D = m.RecordTime, S = Math.Round(m.setMeterialB, 4), E = Math.Round(m.errorMeterialB, 4), R = Math.Round(m.realMeterialB, 4), P = Math.Round(m.errorMeterialB_Percent, 4) });
                }
                else if (Material == "C")
                {
                    if (m.setMeterialC != 0)
                        Result.Add(new Material() { D = m.RecordTime, S = Math.Round(m.setMeterialC, 4), E = Math.Round(m.errorMeterialC, 4), R = Math.Round(m.realMeterialC, 4), P = Math.Round(m.errorMeterialC_Percent, 4) });

                }
                else if (Material == "D")
                {
                    if (m.setMeterialD != 0)
                        Result.Add(new Material() { D = m.RecordTime, S = Math.Round(m.setMeterialD, 4), E = Math.Round(m.errorMeterialD, 4), R = Math.Round(m.realMeterialD, 4), P = Math.Round(m.errorMeterialD_Percent, 4) });
                }
                else if (Material == "E")
                {
                    if (m.setMeterialE != 0)
                        Result.Add(new Material() { D = m.RecordTime, S = Math.Round(m.setMeterialE, 4), E = Math.Round(m.errorMeterialE, 4), R = Math.Round(m.realMeterialE, 4), P = Math.Round(m.errorMeterialE_Percent, 4) });
                }
                else if (Material == "F")
                {
                    if (m.setMeterialF != 0)
                        Result.Add(new Material() { D = m.RecordTime, S = Math.Round(m.setMeterialF, 4), E = Math.Round(m.errorMeterialF, 4), R = Math.Round(m.realMeterialF, 4), P = Math.Round(m.errorMeterialF_Percent, 4) });
                }
                else if (Material == "N")
                {
                    if (m.setMeterialN != 0)
                        Result.Add(new Material() { D = m.RecordTime, S = Math.Round(m.setMeterialN, 4), E = Math.Round(m.errorMeterialN, 4), R = Math.Round(m.realMeterialN, 4), P = Math.Round(m.errorMeterialN_Percent, 4) });
                }
                else if (Material == "Nuc")
                {
                    if (m.setMeterialNuc != 0)
                        Result.Add(new Material() { D = m.RecordTime, S = Math.Round(m.setMeterialNuc, 4), E = Math.Round(m.errorMeterialNuc, 4), R = Math.Round(m.realMeterialNuc, 4), P = Math.Round(m.errorMeterialNuc_Percent, 4) });
                }
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /RealTimeViewer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /RealTimeViewer/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RealTimeViewer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RealTimeViewer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /RealTimeViewer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RealTimeViewer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /RealTimeViewer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
