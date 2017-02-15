using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository;

namespace ZemogaDevTest.Controllers
{
    public class Get_Employees_ResultController : Controller
    {
        private ZemogaModel_Repository db = new ZemogaModel_Repository();

        // GET: Get_Employees_Result
        public ActionResult Index(string searchString)
        {
            var algo = db.Get_Employees(searchString ?? "").ToList();
            return View(algo);
        }

        // GET: Get_Employees_Result/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Details", "Employees", new { id = id });
        }

        // GET: Get_Employees_Result/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Get_Employees_Result/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpID,EmpPhotoFile,EmpName,Position,Project,Skills")] Get_Employees_Result get_Employees_Result)
        {
            if (ModelState.IsValid)
            {
                db.Get_Employees_Result.Add(get_Employees_Result);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(get_Employees_Result);
        }

        // GET: Get_Employees_Result/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Edit", "Employees", new { id = id });
        }

        // POST: Get_Employees_Result/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmpID,EmpPhotoFile,EmpName,Position,Project,Skills")] Get_Employees_Result get_Employees_Result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(get_Employees_Result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(get_Employees_Result);
        }

        // GET: Get_Employees_Result/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Delete", "Employees", new { id = id });
        }

        // POST: Get_Employees_Result/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Get_Employees_Result get_Employees_Result = db.Get_Employees_Result.Find(id);
            db.Get_Employees_Result.Remove(get_Employees_Result);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
