using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ProfholodSite.Models
{
    public class Session
    {
        [Key]
        public DateTime OpenSessionDateTime { get; set; }
        public DateTime CloseSessionDateTime { get; set; }
        public Int32 OpenSessionCode { get; set; }
        public Int32 CloseSessionCode { get; set; }

    }
    public class DataAcquisitionSession : Session
    {
      
    }

    public class LinearSession : Session
    {
       
    }

    public class CastingSession : Session
    {

    }

    public class AlarmsSession : Session
    {

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

        public DbSet<DataAcquisitionSession> DataAcquisitionSessions { get; set; }
        public DbSet<LinearSession> LinearSessions { get; set; }

        public DbSet<CastingSession> CastingSessions { get; set; }

        public DbSet<AlarmsSession> AlarmsSessions { get; set; }
        
        
    
    }

}