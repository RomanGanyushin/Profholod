using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;

using ProfHolodSite.Models;
using System.Web.Script.Serialization;
using System.IO;


namespace ProfholodSite.Controllers
{
    public class FileSystemController : Controller
    {
        //
        // GET: /FileSystem/

         [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }


        private void _DiskScaner(List<FileObject> files, Int32 counter, string path)
        {
            var dirnames = Directory.GetDirectories(path);
            foreach (string f in dirnames)
            {
                files.Add(new FileObject() { FilePathName = f, Directory = path, Title = Path.GetFileName(f), IsDirectory = true,Id = counter++ });
            }

            var filenames = Directory.GetFiles(path);
            foreach (string f in filenames)
            {
                files.Add(new FileObject() { FilePathName = f, Directory = path, Title = Path.GetFileName(f), IsDirectory = false, Id = counter++ });
            }

            foreach (string f in dirnames)
            {
                _DiskScaner(files, counter, f);
            }
        }
        public JsonResult GetUploadFiles()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
            
            var files = new List<FileObject>();

            int counter = 1;
            _DiskScaner(files, counter, path);

            return Json(files, JsonRequestBehavior.AllowGet);
        }
	

         [HttpGet]
        public ActionResult Upload()
        {
            ViewBag.Path =   AppDomain.CurrentDomain.BaseDirectory;
            return View();
        }

        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> fileUpload)
        {
            foreach (var file in fileUpload)
            {
                if (file == null) continue;
                string path = AppDomain.CurrentDomain.BaseDirectory+"UploadedFiles/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string filename = Path.GetFileName(file.FileName);
                if (filename != null) file.SaveAs(Path.Combine(path, filename));
            }

            return RedirectToAction("Index");

        }
    }
}