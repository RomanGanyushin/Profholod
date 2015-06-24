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
using ProfholodSite.SupplyModel;


namespace ProfholodSite.Controllers
{
    [Authorize]
    public class SupplyClientController : Controller
    {
        private SupplyModelContext db = new SupplyModelContext();
        private MachineObjectContext db2 = new MachineObjectContext();

        //
        // GET: /SupplyClient/
        public ActionResult Index()
        {
           // db.UpdateDatabase();

            var recived = db.ReciveMessages.ToList();
            return View(recived);
        }

        
    

      public JsonResult GetDataJson_DB()
        {
            List<Position> Result = db.Positions.Where(m=>m.StateId<=5).OrderByDescending(m=>m.OpenDateTime).ToList();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

      public JsonResult GetStatusItems()
      {
          List<Status> Result = db.Statuses.ToList();
          return Json(Result, JsonRequestBehavior.AllowGet);
      }
      
        /// <summary>
        ///  TOTO Перенести в другое мечто
        /// </summary>
        /// <returns></returns>
      public JsonResult GetContactsItems()
      {
           List<StaffPerson> Result = db2.StaffPersons.ToList();
           return Json(Result, JsonRequestBehavior.AllowGet);
      }

      public string GetCurrentUserName()
      {
          return User.Identity.Name;
      }


      public JsonResult CreateNewGroup(string GroupName, int GroupStatusId, string OrderUserName, string Decription)
      {
          if (User.Identity.Name == "")
              return Json(new { Error = "Нет прав на создание списка" }, JsonRequestBehavior.AllowGet);

          if (GroupName.Count()==0)
              return Json(new { Error = "Отсутствует название списка" }, JsonRequestBehavior.AllowGet);

          Int32 GroupStateId = 5;

          var currentDateTime = new MDTime().GetCurrentTime();
          
          try
          {
              Position new_group =new Position(){
                  Caption = GroupName, 
                  IsGroup=true,
                  PositionGroupId=0,
                  OpenDateTime = currentDateTime,
                  CloseDateTime = currentDateTime,
                  ModifyDateTime = currentDateTime,
                  StatusId = GroupStatusId,
                  StateId = GroupStateId,
                  OpenUserEmail = User.Identity.Name,
                  CloseUserEmail = User.Identity.Name,
                  OrderEmail = OrderUserName, 
                  Description = Decription, 
                  LinkProjectId = -1,
                  Price = 0
              };

              db.Positions.Add(new_group);
              db.SaveChanges();

              return Json(new_group, JsonRequestBehavior.AllowGet);

          }
          catch (SystemException e) {}

          return Json(new{Error = "Не удалось создать список"}, JsonRequestBehavior.AllowGet);
      }

      public JsonResult EditGroup(int Id, string GroupName, int GroupStatusId, string OrderUserName, string Decription)
      {
          if (User.Identity.Name == "")
              return Json(new { Error = "Нет прав на редакирование списка" }, JsonRequestBehavior.AllowGet);

          if (GroupName.Count() == 0)
              return Json(new { Error = "Отсутствует название списка" }, JsonRequestBehavior.AllowGet);
     
          var currentDateTime = new MDTime().GetCurrentTime();

          try
          {
              Position edit_group = db.Positions.Find(Id);

              edit_group.Caption = GroupName;
              edit_group.ModifyDateTime = currentDateTime;
              edit_group.StatusId = GroupStatusId;
              edit_group.OrderEmail = OrderUserName;
              edit_group.Description = Decription;


              db.Entry(edit_group).State = EntityState.Modified;
              db.SaveChanges();

              return Json(edit_group, JsonRequestBehavior.AllowGet);

          }
          catch (SystemException e) { }

          return Json(new { Error = "Не удалось отредактировать список" }, JsonRequestBehavior.AllowGet);
      }

      public JsonResult CreateNewRecordAtRoot(string RecordName, int RecordStatusId, string OrderUserName, string Decription)
      {
          if (User.Identity.Name == "")
              return Json(new { Error = "Нет прав на создание записи" }, JsonRequestBehavior.AllowGet);

          if (RecordName.Count() == 0)
              return Json(new { Error = "Отсутствует название записи" }, JsonRequestBehavior.AllowGet);

          Int32 RecordStateId = 1;

          var currentDateTime = new MDTime().GetCurrentTime();

          try
          {
              Position new_record = new Position()
              {
                  Caption = RecordName,
                  IsGroup = false,
                  PositionGroupId = 0,
                  OpenDateTime = currentDateTime,
                  CloseDateTime = currentDateTime,
                  ModifyDateTime = currentDateTime,
                  StatusId = RecordStatusId,
                  StateId = RecordStateId,
                  OpenUserEmail = User.Identity.Name,
                  CloseUserEmail = User.Identity.Name,
                  OrderEmail = OrderUserName,
                  Description = Decription,
                  LinkProjectId = -1,
                  Price = 0
              };

              db.Positions.Add(new_record);
              db.SaveChanges();

              return Json(new_record, JsonRequestBehavior.AllowGet);

          }
          catch (SystemException e) { }

          return Json(new { Error = "Не удалось создать запись" }, JsonRequestBehavior.AllowGet);
      }

      public JsonResult EditRecordAtRoot(int Id, string RecordName, int RecordStatusId, string OrderUserName, string Decription)
      {
          if (User.Identity.Name == "")
              return Json(new { Error = "Нет прав на редактирование записи" }, JsonRequestBehavior.AllowGet);

          if (RecordName.Count() == 0)
              return Json(new { Error = "Отсутствует название записи" }, JsonRequestBehavior.AllowGet);

          var currentDateTime = new MDTime().GetCurrentTime();

          try
          {
              Position edit_record = db.Positions.Find(Id);
              edit_record.Caption = RecordName;
              edit_record.ModifyDateTime = currentDateTime;
              edit_record.Description = Decription;
              edit_record.StatusId = RecordStatusId;
              edit_record.OrderEmail = OrderUserName;

              db.Entry(edit_record).State = EntityState.Modified;
              db.SaveChanges();


              return Json(edit_record, JsonRequestBehavior.AllowGet);

          }
          catch (SystemException e) { }

          return Json(new { Error = "Не удалось отредакировать запись" }, JsonRequestBehavior.AllowGet);
      }

      public JsonResult CreateNewRecordAtGroup(string RecordName, string Decription, int GroupId)
      {
          if (User.Identity.Name == "")
              return Json(new { Error = "Нет прав на создание записи" }, JsonRequestBehavior.AllowGet);

          if (RecordName.Count() == 0)
              return Json(new { Error = "Отсутствует название записи" }, JsonRequestBehavior.AllowGet);

          Position group = db.Positions.Where(m => m.Id == GroupId).ToList().First();
          if(!group.IsGroup)
              return Json(new { Error = "Указанный объект не группа" }, JsonRequestBehavior.AllowGet);


          Int32 RecordStateId = 1;

          var currentDateTime = new MDTime().GetCurrentTime();

          try
          {
              Position new_record = new Position()
              {
                  Caption = RecordName,
                  IsGroup = false,
                  PositionGroupId = group.Id,
                  OpenDateTime = currentDateTime,
                  CloseDateTime = currentDateTime,
                  ModifyDateTime = currentDateTime,
                  StatusId = group.StatusId,
                  StateId = RecordStateId,
                  OpenUserEmail = User.Identity.Name,
                  CloseUserEmail = User.Identity.Name,
                  OrderEmail = group.OrderEmail,
                  Description = Decription,
                  LinkProjectId = -1,
                  Price = 0
              };

              db.Positions.Add(new_record);
              db.SaveChanges();

              return Json(new_record, JsonRequestBehavior.AllowGet);

          }
          catch (SystemException e) { }

          return Json(new { Error = "Не удалось создать запись" }, JsonRequestBehavior.AllowGet);
      }

      public JsonResult EditRecordAtGroup(int Id, string RecordName, string Decription)
      {
          if (User.Identity.Name == "")
              return Json(new { Error = "Нет прав на редактирование записи" }, JsonRequestBehavior.AllowGet);

          if (RecordName.Count() == 0)
              return Json(new { Error = "Отсутствует название записи" }, JsonRequestBehavior.AllowGet);

          var currentDateTime = new MDTime().GetCurrentTime();

          try
          {
                Position edit_record = db.Positions.Find(Id);
                edit_record.Caption =RecordName;
                edit_record.ModifyDateTime = currentDateTime;
                edit_record.Description = Decription;

                db.Entry(edit_record).State = EntityState.Modified;
                db.SaveChanges();

              return Json(edit_record, JsonRequestBehavior.AllowGet);

          }
          catch (SystemException e) { }

          return Json(new { Error = "Не удалось отредактировать запись" }, JsonRequestBehavior.AllowGet);
      }

      public JsonResult DeleteRecordOrGroup(int Id)
      {
          if (User.Identity.Name == "")
              return Json(new { Error = "Нет прав на редактирование записи" }, JsonRequestBehavior.AllowGet);

          var currentDateTime = new MDTime().GetCurrentTime();

          try
          {
              Position edit_record = db.Positions.Find(Id);
              edit_record.ModifyDateTime = currentDateTime;
              edit_record.StateId = 6; // Удален
              db.Entry(edit_record).State = EntityState.Modified;

              List<Position> childs =
                  db.Positions.Where(m => m.PositionGroupId == Id).ToList();
              foreach (var i in childs)
              {
                  i.StateId = 6;
                  db.Entry(i).State = EntityState.Modified;
              }
              
              db.SaveChanges();

              return Json(edit_record, JsonRequestBehavior.AllowGet);

          }
          catch (SystemException e) { }
          return Json(new { Error = "Не удалось выполнить удаление" }, JsonRequestBehavior.AllowGet);
  
      }

      public Array BuildInfoStructures(int Id)
      {
          try
          {
              Position edit_record = db.Positions.Find(Id);

              //var result;
              object[] result = new object[1] ;
              object[]  desc = new object[2]; 
                desc[0] = new { label = "Подпозиция1" };
                desc[1] = new { label = "Подпозиция1" };

              result[0] = new{ label = "Описание позиции",  expanded= true,items = desc} ;

              return result;

          }
          catch (SystemException e) { }
          return null;

      }

    }

    

}


