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
        private MachineObjectContext db = new MachineObjectContext();
        // GET: /ReportsSummary/
        public ActionResult Index()
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef") throw new Exception("Access not denid");

            return View();
        }

        public PartialViewResult _ReportSummary(int Month,int Year, bool bFull)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef") throw new Exception("Access not denid");

           
            PerformOperationsSummary summary = new PerformOperationsSummary();
             summary.CreatePerformReport(new MDTime().GetStartRange(Month, Year), 
                new MDTime().GetEndRange(Month, Year));

             summary.isFull = bFull;
           
            return PartialView("ReportSummary", summary);
        }

        public ActionResult _ReportSummaryPDF(int Month,int Year, bool bFull)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef") throw new Exception("Access not denid");


            PerformOperationsSummary summary = new PerformOperationsSummary();
            summary.CreatePerformReport(new MDTime().GetStartRange(Month, Year), 
                new MDTime().GetEndRange(Month, Year));

            summary.isFull = bFull;

            string text = new ReportManagement.HtmlViewRenderer().RenderViewToString(this, "ReportSummary", summary);
            byte[] buffer = new ReportManagement.StandardPdfRenderer().Render(text, "Testing");
             return new  ReportManagement.BinaryContentResult(buffer, "application/pdf");
        }

        public PartialViewResult _ReportMaintenace(int Month, int Year, bool bFull)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef") throw new Exception("Access not denid");


            PerformMaintanaceSummary summary = new PerformMaintanaceSummary();
            summary.CreatePerformReport(new MDTime().GetStartRange(Month, Year),
               new MDTime().GetEndRange(Month, Year));

            summary.isFull = bFull;

            return PartialView("ReportMaintanace", summary);
        }

        public ActionResult _ReportMaintanacePDF(int Month, int Year, bool bFull)
        {
            if (User.Identity.Name == "") throw new Exception("Access not denid");
            string access_type = db.StaffPersons.Where(p => p.UserName == User.Identity.Name).First().AccessType;
            if (access_type != "Administrator" && access_type != "Chef") throw new Exception("Access not denid");


            PerformMaintanaceSummary summary = new PerformMaintanaceSummary();
            summary.CreatePerformReport(new MDTime().GetStartRange(Month, Year),
                new MDTime().GetEndRange(Month, Year));

            summary.isFull = bFull;

            string text = new ReportManagement.HtmlViewRenderer().RenderViewToString(this, "ReportMaintanace", summary);
            byte[] buffer = new ReportManagement.StandardPdfRenderer().Render(text, "Testing");
            return new ReportManagement.BinaryContentResult(buffer, "application/pdf");
        }

	}
}