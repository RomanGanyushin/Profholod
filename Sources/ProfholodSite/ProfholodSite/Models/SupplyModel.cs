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

using OpenPop.Pop3;
using OpenPop.Mime;

namespace ProfholodSite.SupplyModel
{
     



    [Table("sm.ReciveMessageTable")]
    public class ReciveMessage
    {
        public Int32 Id { get; set; }
        public string MessageId { get; set; }

        public string FromAddress { get; set; }
        public DateTime DataSent { get; set; }
        public string CopyTo { get; set; }
        public bool isActual { get; set; }
    }

    [Table("sm.SendMessageTable")]
    public class SentMessage
    {
        public Int32 Id { get; set; }
    }

    [Table("sm.StatusTable")]
    public class Status
    {
        public Int32 Id { get; set; }
        public string Caption { get; set; }
    }

    [Table("sm.StateTable")]
    public class State
    {
        public Int32 Id { get; set; }
        public string Caption { get; set; }
    }

    [Table("sm.PositionTable")]
    public class Position
    {
        [Key]
        public Int32 Id { get; set; }

       
        public Int32 StatusId { get; set; }
        public Int32 StateId { get; set; }

        public bool IsGroup { get; set; }
      
        public Int32 PositionGroupId { get; set; }
        [Required()]
        public string Caption { get; set; }
        public DateTime OpenDateTime { get; set; }
        public DateTime CloseDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
         [Required()]
        public string OpenUserEmail { get; set; }
         [Required()]
        public string CloseUserEmail { get; set; }
         public string OrderEmail { get; set; }

        public Int32 LinkProjectId { get; set; }

        public string Description { get; set; }
        public virtual Status Status { get; set; }
        public virtual State State { get; set; }

        public virtual double Price { get; set; }

        public string _c(DateTime s)
        {
            return s.Month.ToString("00") + "/" + s.Day.ToString("00") + "/" + s.Year.ToString() +
                " " + s.Hour.ToString("00") + ":" + s.Minute.ToString("00") + ":" + s.Second.ToString("00");
        }
        public virtual string strOpenDateTime 
        { 
            get{ return _c(OpenDateTime);}
        }

        public virtual string strCloseDateTime
        {
            get { return _c(OpenDateTime); }
        }

    }

    public class SupplyModelContext : DbContext
    {
        public SupplyModelContext()
            : base("DefaultConnection")
        {
            if (Statuses.ToList().Count() == 0)
            {
                Statuses.Add(new Status() { Id = 1, Caption = "Срочный" });
                Statuses.Add(new Status() { Id = 2, Caption = "Нормальный" });
                Statuses.Add(new Status() { Id = 3, Caption = "В перспективе" });
                Statuses.Add(new Status() { Id = 4, Caption = "Для анализа" });
            }

            if (States.ToList().Count() == 0)
            {
                States.Add(new State() { Id = 1, Caption = "Открыт" });
                States.Add(new State() { Id = 2, Caption = "Закрыт" });
                States.Add(new State() { Id = 3, Caption = "Заморожен" });
                States.Add(new State() { Id = 4, Caption = "Отменен" });
                States.Add(new State() { Id = 5, Caption = "Групповой" });

            }

           SaveChanges();

        }



        public static SupplyModelContext Create()
        {
            return new SupplyModelContext();
        }

        public DbSet<ReciveMessage> ReciveMessages { get; set; }
        public DbSet<SentMessage> SentMessages { get; set; }

        public DbSet<Status> Statuses { get; set; }
        public DbSet<Position> Positions { get; set; }

        public DbSet<State> States { get; set; }

        public  void UpdateDatabase()
        {
                 Pop3Client client = new Pop3Client();
  
                 client.Connect("pop.mail.ru", 995, true);
                 client.Authenticate("profholod_crm@mail.ru", "Thunder1980");

                int messageCount = client.GetMessageCount();

                for (Int32 iMessage = 1; iMessage <= client.GetMessageCount(); iMessage++)
                {
                    Message message = client.GetMessage(iMessage);

                    

                    ReciveMessage record = new ReciveMessage()
                    {
                        MessageId = message.Headers.MessageId,
                        FromAddress = message.Headers.From.Address,
                        DataSent = message.Headers.DateSent,
                        isActual = true
                    };
                        
                    foreach(var cc in message.Headers.Cc)
                    { if (record.CopyTo!=null)record.CopyTo+=",";record.CopyTo+=cc.Address;}

       

                        if (ReciveMessages.Where(m => m.MessageId == record.MessageId).ToList().Count() == 0)
                            ReciveMessages.Add(record);

                   
                }

                SaveChanges();
               
        }

    }

}