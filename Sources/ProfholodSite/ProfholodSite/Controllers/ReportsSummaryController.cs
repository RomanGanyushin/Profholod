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
            /*
           
             */
             return View();
        }

        public PartialViewResult _ReportSummary(int Month,int Year, bool bFull)
        {
           
            PerformOperationsSummary summary = new PerformOperationsSummary();
             summary.CreatePerformReport(new MDTime().GetStartRange(Month, Year), 
                new MDTime().GetEndRange(Month, Year));

             summary.isFull = bFull;
           
            return PartialView("ReportSummary", summary);
        }

        public ActionResult _ReportSummaryPDF(int Month,int Year, bool bFull)
        {
          PerformOperationsSummary summary = new PerformOperationsSummary();
            summary.CreatePerformReport(new MDTime().GetStartRange(Month, Year), 
                new MDTime().GetEndRange(Month, Year));

            summary.isFull = bFull;

            string text = new ReportManagement.HtmlViewRenderer().RenderViewToString(this, "ReportSummary", summary);
            byte[] buffer = new ReportManagement.StandardPdfRenderer().Render(text, "Testing");
             return new  ReportManagement.BinaryContentResult(buffer, "application/pdf");
        }

	}
}