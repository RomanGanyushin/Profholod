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

namespace ProfHolodSite.Models
{
    public class MDTime
      {
        public DateTime GetCurrentTime()
        { return DateTime.Now.AddHours(3); }
    
        public DateTime GetStartRange(int Month, int Year)
        {
           return new DateTime(Year, Month, 1);
        }
        public DateTime GetEndRange(int Month, int Year)
        {
            return new DateTime(Year, Month, 1).AddMonths(1);
        }
    }
    



    public class Periodical
    {
        public Int32 Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Период в секундах")]
        public Int32 Time_period_sec { get; set; }
        [Display(Name = "Код")]
        public string LCode { get; set; }

        public virtual ICollection<MaintenacesObject> MaintenacesObjects { get; set; }

        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }

    public class MaintenaceState
    {
        public Int32 Id { get; set; }
        [Display(Name = "Название состояния")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public virtual ICollection<MaintenacesObject> MaintenacesObjects { get; set; }

        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }

    public class MachineObjectGroup
    {
        [Key]
        public Int32 Id { get; set; }
        [Display(Name = "Название группы")]
        public string GroupName { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        public virtual ICollection<MachineObject> MachineObjects { get; set; }

        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

    }

    public class MachineObject
    {
        public Int32 Id { get; set; }

        [Display(Name = "Компонент объекта")]
        public Int32 ParentObjectId { get; set; }
    
        public virtual MachineObject ParentObject { get; set; }

        [Display(Name = "Группа")]
        public Int32 MachineObjectGroupId { get; set; }
        public virtual MachineObjectGroup MachineObjectGroup { get; set; }

        [Display(Name = "Название компонента")]
        public string Name { get; set; }

        [Display(Name = "Код")]
        public string Code { get; set; }

        public virtual ICollection<MaintenacesObject> MaintenacesObjects { get; set; }
        public virtual ICollection<MachineObject> ParentObjects { get; set; }
        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

       //
        public MachineObject Copy()
        {
            return new MachineObject() { 
                Id = this.Id, ParentObjectId=(Id!=ParentObjectId)?this.ParentObjectId:0 , Name = this.Name };
        }
    }

   
   
    public class MaintenaceAction
    {
        public Int32 Id { get; set; }
        [Display(Name = "Действие")]
        public string Caption { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public virtual ICollection<MaintenacesObject> MaintenacesObjects { get; set; }

        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }

    public class MaintenacesObject
    {
        public Int32 Id { get; set; }
        [Display(Name = "Действие")]
        public Int32 MaintenaceActionId { get; set; }
        [Display(Name = "Название компонента")]
        public Int32 MachineObjectId { get; set; }
       
        [Display(Name = "Периодичность")]
        public Int32 PeriodicalID { get; set; }

        [Display(Name = "Дата начала сервиса")]
        [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }

        [Display(Name = "Дата окончания сервиса")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateEnd { get; set; }

        [Display(Name = "Состояние")]
        public Int32 MaintenaceStateID { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }


        public virtual MaintenaceAction MaintenaceAction { get; set; }
        public virtual MachineObject MachineObject { get; set; }
        public virtual Periodical Periodical { get; set; }
        public virtual MaintenaceState MaintenaceState { get; set; }

        
        public virtual ICollection<PerformMaintenanceReport> PerformMaintenanceReports { get; set; }

        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }


    
    public class StaffPerson
    {
        public Int32 StaffPersonId { get; set; }
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        public string SecondName { get; set; }
        public virtual ICollection<StaffGroup> StaffGroups { get; set; }

        public string UserName   { get; set;}
        public string AccessType { get; set;}

    }

    public class StaffGroup
    {
        public Int32 StaffGroupId { get; set; }
        [Display(Name = "Название группы")]
        public string GroupName { get; set; }
        [Display(Name = "Персонал")]
        public virtual ICollection<StaffPerson> StaffPersons { get; set; }
      

        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }

    public class PerformMaintenanceReport
    {
        public Int32 Id { get; set; }

        
        [Display(Name = "Регламентная работа")]
        public Int32 MaintenacesObjectId { get; set; }

        //[Required]
        [Display(Name = "Время начала работ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeStart { get; set; }

       // [Required]
        [Display(Name = "Время окончания работ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeEnd { get; set; }

        [Display(Name = "Статус")]
        public bool IsConfirm { get; set; }

        [Display(Name = "Текст отчета")]
        [DataType(DataType.MultilineText)]
        public string ReportText { get; set; }

        public virtual MaintenacesObject MaintenacesObject { get; set; }
       

        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }

    public class FaultIndex
    {
        public Int32 Id { get; set; }

        [Display(Name = "Индекс аварийности")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

    }

    public class TypeOfFault
    {
        public Int32 Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Код")]
        public string LCode { get; set; }

        [Display(Name = "Индекс аварийности")]
        public Int32 FaultIndexId { get; set; }


        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        [Display(Name = "Индекс аварийности")]
        public virtual FaultIndex FaultIndex { get; set; }
    }

    public class PerformNoteRepair
    {
        public Int32 Id { get; set; }

        [Display(Name = "Время начала работ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeStart { get; set; }

        // [Required]
        [Display(Name = "Время окончания работ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeEnd { get; set; }


        [Display(Name = "Объект")]
        public string RepairObjectText { get; set; }

        [Display(Name = "Тип неисправности")]
        public string TypeOfFaultText { get; set; }

        [Display(Name = "Действие")]
        public string RepairActionText { get; set; }

        [Display(Name = "Текст отчета")]
        [DataType(DataType.MultilineText)]
        public string ReportText { get; set; }

        [Display(Name = "Завершено")]
        public bool IsCompleted { get; set; }


        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

    }

    public class ComplitedRepairReport
    {
        public Int32 Id { get; set; }

        [Display(Name = "Название компонента")]
        public Int32 MachineObjectId { get; set; }

        [Display(Name = "Тип неисправности")]
        public Int32 TypeOfFaultId { get; set; }

        [Display(Name = "Действие")]
        public Int32 MaintenaceActionId { get; set; }

        [Display(Name = "Время начала работ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeStart { get; set; }

        // [Required]
        [Display(Name = "Время окончания работ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeEnd { get; set; }


        [Display(Name = "Текст отчета")]
        [DataType(DataType.MultilineText)]
        public string ReportText { get; set; }

        [Display(Name = "Завершено")]
        public bool IsCompleted { get; set; }


        public string CreateUserName { get; set; } // Author of record
        public string ModifyUserName { get; set; } // Author of modify

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual MaintenaceAction MaintenaceAction { get; set; }
        public virtual MachineObject MachineObject { get; set; }
        public virtual TypeOfFault TypeOfFault { get; set; }

    }

    public class MachineObjectContext : DbContext
        {
        public MachineObjectContext()
            : base("DefaultConnection")
        {
            
        }

        public static MachineObjectContext Create()
        {
            return new MachineObjectContext();
        }

            public DbSet<Periodical> Periodicals { get; set; }
            public DbSet<MachineObjectGroup> MachineObjectGroups { get; set; }
            public DbSet<MaintenaceState> MaintenaceStates { get; set; }
            public DbSet<MaintenaceAction> MaintenaceActions { get; set; }
            public DbSet<MachineObject> MachineObjects { get; set; }
            public DbSet<MaintenacesObject> MaintenacesObjects { get; set; }
            public DbSet<StaffPerson> StaffPersons { get; set; }
            public DbSet<StaffGroup> StaffGroups { get; set; }
            public DbSet<TypeOfFault> TypeOfFaults { get; set; }
            public DbSet<FaultIndex> FaultIndexs { get; set; }
        


            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Entity<StaffGroup>().HasMany(c => c.StaffPersons)
                    .WithMany(s => s.StaffGroups)
                    .Map(t => t.MapLeftKey("StaffGroupId")
                    .MapRightKey("StaffPersonId")
                    .ToTable("StaffPersonGroup"));
            }

            public DbSet<PerformMaintenanceReport> PerformMaintenanceReports { get; set; }
            public DbSet<PerformNoteRepair> PerformNoteRepairs { get; set; }
            public DbSet<ComplitedRepairReport> ComplitedRepairReports { get; set; }
        

    }


    public class MaintenaceModel
    {

    }

   

}