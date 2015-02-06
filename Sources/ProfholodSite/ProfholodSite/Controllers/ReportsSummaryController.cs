using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfHolodSite.Models;

namespace ProfholodSite.Controllers
{
    public class ReportsSummaryController : Controller
    {
        //
        // GET: /ReportsSummary/
        public ActionResult Index()
        {
            PerformOperationsSummary summary = new PerformOperationsSummary();
            var report = summary.CreatePerformReport();

            return View("ReportSummary", report);
        }
	}
}