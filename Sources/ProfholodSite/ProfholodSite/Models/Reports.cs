using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using ProfHolodSite.Models;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProfHolodSite.Models;

namespace ProfHolodSite.Models
{

    public class AnyOperation
    {
       [Display(Name = "Источник записи")]
        public string OperationReportSource {get;set;}
        public Int32 NativeID { get; set; }
         
        [Display(Name = "Название компонента")]
        public string Object { get; set; }
        public Int32 ObjectNativeID { get; set; }

        [Display(Name = "Действие")]
        public string Action { get; set; }
        public Int32 ActionNativeID { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        [Display(Name = "Причина")]
        public string Reason { get; set; }
        public Int32 ReasonNativeID { get; set; }

        [Display(Name = "Отчет по работе")]
        public string ReportText { get; set; }

        [Display(Name = "Запись сделал")]
        public string RecordAuthorText { get; set; }

        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date
        { get {return StartDateTime;} }

        [Display(Name = "Время")]
        public string Times
        { get { return StartDateTime.ToString("H:mm") + "-" + EndDateTime.ToString("H:mm"); } }

    }

    public class AnyMaintanace
    {
        public Int32 NativeID { get; set; }

        [Display(Name = "Регламентная работа")]
        public string MaintanaceObject { get; set; }
        public Int32 MaintanaceObjectNativeID { get; set; }


        [Display(Name = "Название компонента")]
        public string Object { get; set; }
        public Int32 ObjectNativeID { get; set; }

        [Display(Name = "Действие")]
        public string Action { get; set; }
        public Int32 ActionNativeID { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        [Display(Name = "Запись сделал")]
        public string RecordAuthorText { get; set; }

        [Display(Name = "Отчет по работе")]
        public string ReportText { get; set; }

        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date
        { get { return StartDateTime; } }

        [Display(Name = "Время")]
        public string Times
        { get { return StartDateTime.ToString("H:mm") + "-" + EndDateTime.ToString("H:mm"); } }

    }


    public class PerformMaintanaceSummary
    {
        private MachineObjectContext db = new MachineObjectContext();

        private string ConvertUserToString(string user)
        {
            return db.StaffPersons
                .Where(p => p.UserName == user).First().Surname;
        }

        public void CreatePerformReport(DateTime From, DateTime To)
        {
            dateFrom = From;
            dateTo = To;
            PrettyPeriodString = new MDTime().MonthToString(dateFrom.Month) + " " + dateFrom.Year.ToString();

            content = new List<AnyMaintanace>();

            var performMaintenanceReports = db.PerformMaintenanceReports.Include(p => p.MaintenacesObject)
                .Where(p => p.IsConfirm == true).
                 Where(c => c.DateTimeStart >= From).
                 Where(c => c.DateTimeStart <= To);


            foreach (PerformMaintenanceReport maintenace in performMaintenanceReports.ToList())
            {
                content.Add(new AnyMaintanace()
                {
                    NativeID = maintenace.Id,
                    MaintanaceObject = maintenace.MaintenacesObject.Description,
                    MaintanaceObjectNativeID = maintenace.MaintenacesObject.Id,
                    Object = maintenace.MaintenacesObject.MachineObject.Name,
                    ObjectNativeID = maintenace.MaintenacesObject.MachineObject.Id,
                    Action = maintenace.MaintenacesObject.MaintenaceAction.Caption,
                    ActionNativeID = maintenace.MaintenacesObject.MaintenaceAction.Id,
                    StartDateTime = maintenace.DateTimeStart,
                    EndDateTime = maintenace.DateTimeEnd,
                    ReportText = maintenace.ReportText,
                    RecordAuthorText = ConvertUserToString(maintenace.CreateUserName)

                });
            }

            
        }

        public List<AnyMaintanace> content;
        public DateTime dateFrom{ get; set; }
        public DateTime dateTo{ get; set; }

        public string PrettyPeriodString { get; set; }
        public bool isFull { get; set; }
    }


    public class PerformOperationsSummary
    {
        private MachineObjectContext db = new MachineObjectContext();
        private string ConvertUserToString(string user)
        {
            return db.StaffPersons
                .Where(p => p.UserName == user).First().Surname;
        }
        public void  CreatePerformReport(DateTime From, DateTime To)
        {
            dateFrom = From;
            dateTo = To;
            PrettyPeriodString = new MDTime().MonthToString(dateFrom.Month) + " " + dateFrom.Year.ToString();

             content = new List<AnyOperation>();

            var complitedRepairReports = db.ComplitedRepairReports.
                Where(c => c.DateTimeStart >= From).
                Where(c => c.DateTimeStart <= To).
                Include(c => c.MachineObject).
                Include(c => c.MaintenaceAction).
                Include(c => c.TypeOfFault);


            foreach (ComplitedRepairReport complite_repair in complitedRepairReports.ToList())
            {
                content.Add(new AnyOperation()
                {
                    OperationReportSource = "отчет о ремонтных работах",
                    NativeID = complite_repair.Id,
                    Object = complite_repair.MachineObject.Name,
                    ObjectNativeID = complite_repair.MachineObject.Id,
                    Action = complite_repair.MaintenaceAction.Caption,
                    ActionNativeID = complite_repair.MaintenaceAction.Id,
                    StartDateTime = complite_repair.DateTimeStart,
                    EndDateTime = complite_repair.DateTimeEnd,
                    Reason = complite_repair.TypeOfFault.Name,
                    ReasonNativeID = complite_repair.TypeOfFault.Id,
                    ReportText = complite_repair.ReportText,
                    RecordAuthorText = ConvertUserToString(complite_repair.CreateUserName)
                });          
            }


            var performNoteRepairs = db.PerformNoteRepairs.
               Where(c => c.DateTimeStart >= From).
               Where(c => c.DateTimeStart <= To);

            foreach (PerformNoteRepair note_repair in performNoteRepairs.ToList())
            {
                content.Add(new AnyOperation()
                {
                    OperationReportSource = "записки о ремнонтных работах",
                    NativeID = note_repair.Id,
                    Object = note_repair.RepairObjectText,
                    ObjectNativeID = -1,
                    Action = note_repair.RepairActionText,
                    ActionNativeID = -1,
                    StartDateTime = note_repair.DateTimeStart,
                    EndDateTime = note_repair.DateTimeEnd,
                    Reason = note_repair.TypeOfFaultText,
                    ReasonNativeID = -1,
                    ReportText = note_repair.ReportText,
                    RecordAuthorText = ConvertUserToString(note_repair.CreateUserName)

                });
            }

            var performMaintenanceReports = db.PerformMaintenanceReports.Include(p => p.MaintenacesObject)
                .Where(p => p.IsConfirm == true).
                 Where(c => c.DateTimeStart >= From).
                 Where(c => c.DateTimeStart <= To);


            foreach (PerformMaintenanceReport maintenace in performMaintenanceReports.ToList())
            {
                content.Add(new AnyOperation()
                {
                    OperationReportSource = "отчет регламентых работ",
                    NativeID = maintenace.Id,
                    Object = maintenace.MaintenacesObject.MachineObject.Name,
                    ObjectNativeID = maintenace.MaintenacesObject.MachineObject.Id,
                    Action = maintenace.MaintenacesObject.MaintenaceAction.Caption,
                    ActionNativeID = maintenace.MaintenacesObject.MaintenaceAction.Id,
                    StartDateTime = maintenace.DateTimeStart,
                    EndDateTime = maintenace.DateTimeEnd,
                    Reason = "Регламентная работа",
                    ReasonNativeID = -1,
                    ReportText = maintenace.ReportText,
                    RecordAuthorText = ConvertUserToString(maintenace.CreateUserName)

                });
            }


            content.Sort(delegate(AnyOperation op1, AnyOperation op2)
            { return op1.StartDateTime.CompareTo(op2.StartDateTime); });

           
        }

        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public string PrettyPeriodString { get; set; }
        public bool isFull { get; set; }
        public List<AnyOperation> content;
    }

   
}