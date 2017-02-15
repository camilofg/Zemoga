using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZemogaDevTest.Models;

namespace ZemogaDevTest.Controllers
{
    public class UploadFileController : Controller
    {
        // GET: UploadFile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddImage(Picture picture)
        {
            if (!isValidContentType(picture.file.ContentType)){
                ViewBag.Error = "The file is not valid";
                return View("Index");
            }
            if (picture.file.ContentLength > 0) {
                var fileName = Path.GetFileName(picture.file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                picture.file.SaveAs(path);
                
            }

            return View();
        }

        private bool isValidContentType(string contentType) {
            return contentType.Equals("image/png") || contentType.Equals("image/gif") || contentType.Equals("image/jpg") || contentType.Equals("image/jpeg");
        }
    }
}