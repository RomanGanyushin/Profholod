using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ProfHolodSite.Models;

namespace ProfholodSite.DataAcquisition
{
   
    public class Session
    {
        [Key]
        public DateTime OpenSessionDateTime { get; set; }
        public DateTime CloseSessionDateTime { get; set; }
        public Int32 OpenSessionCode { get; set; }
        public Int32 CloseSessionCode { get; set; }

    }

     [Table("da.DataAcquisitionSession")] 
    public class DataAcquisitionSession : Session
    {
      
    }

     [Table("da.LinearSession")] 
    public class LinearSession : Session
    {
       
    }
     [Table("da.CastingSession")] 
    public class CastingSession : Session
    {

    }

     [Table("da.AlarmsSession")] 
    public class AlarmsSession : Session
    {

    }

     [Table("da.PLCAlarm")] 
    public class PLCAlarm
    {
        public Int32 Id { get; set; }

        public string WordCode { get; set; }
        public Int32 ByteCode { get; set; }
        public Int32 BiteCode { get; set; }
        public string PlcDescription{ get; set; }
        public Int32 FaultIndexId { get; set; }

    }

    [Table("da.AlarmMessage")] 
    public class AlarmMessage
    {
        public Int32 Id { get;set;}

        public DateTime Begin { get; set;}
        public DateTime End { get; set; }

        public Int32 PLCAlarmId { get; set; }
        public virtual PLCAlarm PLCAlarm { get; set; }

        public DateTime AlarmsSessionId { get; set; }

    };

      [Table("da.CastingProccessTable")] 
    public class CastingProccessTable
    {
        public Int32 Id { get; set; }
        public DateTime RecordTime { get; set; }

        public double setMeterialA { get; set; }
        public double realMeterialA { get; set; }
        public double errorMeterialA { get; set; }

        public double setMeterialB { get; set; }
        public double realMeterialB { get; set; }
        public double errorMeterialB { get; set; }

        public double setMeterialC { get; set; }
        public double realMeterialC { get; set; }
        public double errorMeterialC { get; set; }

        public double setMeterialD { get; set; }
        public double realMeterialD { get; set; }
        public double errorMeterialD { get; set; }

        public double setMeterialE { get; set; }
        public double realMeterialE { get; set; }
        public double errorMeterialE { get; set; }

     

        public double setMeterialF { get; set; }
        public double realMeterialF { get; set; }
        public double errorMeterialF { get; set; }

        public double setMeterialN { get; set; }
        public double realMeterialN { get; set; }
        public double errorMeterialN { get; set; }

        public double setMeterialNuc { get; set; }
        public double realMeterialNuc { get; set; }
        public double errorMeterialNuc { get; set; }


        public DateTime CastingSessionId { get; set; }

        public void CopyFrom(CastingProccessTable c)
        {
            RecordTime = c.RecordTime;
            setMeterialA= c.setMeterialA; realMeterialA = c.realMeterialA; errorMeterialA = c.errorMeterialA;
            setMeterialB = c.setMeterialB; realMeterialB = c.realMeterialB; errorMeterialB = c.errorMeterialB;
            setMeterialC = c.setMeterialC; realMeterialC = c.realMeterialC; errorMeterialC = c.errorMeterialC;
            setMeterialD = c.setMeterialD; realMeterialD = c.realMeterialD; errorMeterialD = c.errorMeterialD;
            setMeterialE = c.setMeterialE; realMeterialE = c.realMeterialE; errorMeterialE = c.errorMeterialE;
            setMeterialF = c.setMeterialF; realMeterialF = c.realMeterialF; errorMeterialF = c.errorMeterialF;
            setMeterialN = c.setMeterialN; realMeterialN = c.realMeterialN; errorMeterialN = c.errorMeterialN;
            setMeterialNuc = c.setMeterialNuc; realMeterialNuc = c.realMeterialNuc; errorMeterialNuc = c.errorMeterialNuc;
        }

       virtual public double errorMeterialA_Percent{
           get { return (setMeterialA==0)?0:(100.0*errorMeterialA / setMeterialA); }
          }
       virtual public double errorMeterialB_Percent
       {
           get { return (setMeterialB == 0) ? 0 : (100.0 * errorMeterialB / setMeterialB); }
       }

       virtual public double errorMeterialC_Percent
       {
           get { return (setMeterialC == 0) ? 0 : (100.0 * errorMeterialC / setMeterialC); }
       }

       virtual public double errorMeterialD_Percent
       {
           get { return (setMeterialD == 0) ? 0 : (100.0 * errorMeterialD / setMeterialD); }
       }

       virtual public double errorMeterialE_Percent
       {
           get { return (setMeterialE == 0) ? 0 : (100.0 * errorMeterialE / setMeterialE); }
       }

       virtual public double errorMeterialF_Percent
       {
           get { return (setMeterialF == 0) ? 0 : (100.0 * errorMeterialF / setMeterialF); }
       }

       virtual public double errorMeterialN_Percent
       {
           get { return (setMeterialN == 0) ? 0 : (100.0 * errorMeterialN / setMeterialN); }
       }
       virtual public double errorMeterialNuc_Percent
       {
           get { return (setMeterialNuc == 0) ? 0 : (100.0 * errorMeterialNuc / setMeterialNuc); }
       }
    }

    public class DataAcquisitionContext : DbContext
    {
        public DataAcquisitionContext()
            : base("DefaultConnection")
        {
     
        }

        public static DataAcquisitionContext Create()
        {
         

            return new DataAcquisitionContext();
        }

        public Int32 GetNotNullPLCAlarm(string _WordCode, Int32 _ByteCode, Int32 _BiteCode)
        {
            var plcAlarmFind = PLCAlarms.Where(p => p.WordCode == _WordCode).
                 Where(p => p.ByteCode == _ByteCode).
                 Where(p => p.BiteCode == _BiteCode);

            PLCAlarm plcAlarm;
            if (plcAlarmFind.Count() != 0)
            {
                plcAlarm = plcAlarmFind.First();
            }
            else
            {
                plcAlarm = new PLCAlarm() { WordCode = _WordCode, ByteCode = _ByteCode, BiteCode = _BiteCode };
                PLCAlarms.Add(plcAlarm);
                SaveChanges();

            }

            return plcAlarm.Id;
        }

        
        public DbSet<DataAcquisitionSession> DataAcquisitionSessions { get; set; }
        public DbSet<LinearSession> LinearSessions { get; set; }

        public DbSet<CastingSession> CastingSessions { get; set; }

        public DbSet<AlarmsSession> AlarmsSessions { get; set; }

        public DbSet<AlarmMessage> AlarmMessages { get; set; }

        public DbSet<CastingProccessTable> CastingProccess{ get; set; }
        
        // Справочники
        public DbSet<PLCAlarm> PLCAlarms { get; set; }
      
    }

    public class DataAcquisitioneInitializer : DropCreateDatabaseAlways<DataAcquisitionContext>
    {
        protected override void Seed(DataAcquisitionContext context)
        {
            //for (int i = 1; i < 1000; i++)
           // {
            //    CastingProccessTable _dI = new CastingProccessTable() {  };
           //     context.CastingProccess.Add(_dI);
            //}
            base.Seed(context);
        }
    }

 


}