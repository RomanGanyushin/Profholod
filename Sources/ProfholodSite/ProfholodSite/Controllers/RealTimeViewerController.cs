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
        public JsonResult GetDataJson()
        {
            return Json(db.CastingProccess.OrderBy(m=> m.RecordTime).ToList(), JsonRequestBehavior.AllowGet);
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
