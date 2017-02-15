using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository;
using System.IO;
using ZemogaDevTest.Models;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZemogaDevTest.Controllers
{
    public class EmployeesController : Controller
    {
        private ZemogaModel_Repository db = new ZemogaModel_Repository();

        // GET: Employees
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Get_Employees_Result");
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            ViewBag.Project = db.Projects.Find(employee.EmpProject).ProDesc;
            ViewBag.Position = db.Position.Find(employee.EmpPosition).PosDesc;
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.EmpPosition = new SelectList(db.Position, "PosID", "PosDesc");
            ViewBag.EmpProject = new SelectList(db.Projects, "ProID", "ProDesc");
            IList<skill> skList = new List<skill>();
            var skills = db.Skill;
            foreach (var item in skills)
            {
                skList.Add(new skill() { SkillID = item.SkiID, SkillDesc = item.SkiDesc, SkillChecked = false });
            }
            ViewData["skillsList"] = skList;
            return View();
        }

        [HttpPost]
        public ActionResult GenerateInfo() {
            var tal = db.Get_Employees("-1").ToList();

            var dt = new DataTable();

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Employee Name", typeof(string));
            dt.Columns.Add("Position", typeof(string));
            dt.Columns.Add("Project", typeof(string));
            dt.Columns.Add("Skills", typeof(string));

            foreach (var itm in tal) {
                dt.Rows.Add(itm.EmpID, itm.EmpName, itm.Position, itm.Project, itm.Skills);
            }

            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View("MyView");
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "EmpPhoto")]Employee employee)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase EmployeeImage = Request.Files["UserPhoto"];

                    using (var binary = new BinaryReader(EmployeeImage.InputStream))
                    {
                        imageData = binary.ReadBytes(EmployeeImage.ContentLength);
                    }
                }
                
                employee.EmpPhoto = imageData;
                db.Employee.Add(employee);
                db.SaveChanges();
                var idEmpCreated = db.Employee.Where(x => x.EmpName == employee.EmpName).Select(x => x.EmpID).FirstOrDefault();
                string listSkillsEmp = string.Empty;
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key.StartsWith("Chk_"))
                    {
                        listSkillsEmp += listSkillsEmp == "" ? Request.Form[key] : "," + Request.Form[key];
                    }
                }

                db.AddSkillEmp(idEmpCreated, listSkillsEmp);

                return RedirectToAction("Index", "Get_Employees_Result");
            }

            ViewBag.EmpPosition = new SelectList(db.Position, "PosID", "PosDesc", employee.EmpPosition);
            ViewBag.EmpProject = new SelectList(db.Projects, "ProID", "ProDesc", employee.EmpProject);
            return RedirectToAction("Index", "Get_Employees_Result");
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpPosition = new SelectList(db.Position, "PosID", "PosDesc", employee.EmpPosition);
            ViewBag.EmpProject = new SelectList(db.Projects, "ProID", "ProDesc", employee.EmpProject);
            IList<skill> skList = new List<skill>();
            var skills = db.GetSkillsByEmployee(id);// db.Skill;
            foreach (var item in skills)
            {
                skList.Add(new skill() { SkillID = item.SkiID, SkillDesc = item.SkiDesc, SkillChecked = (bool)item.Checked });
            }
            ViewData["skillsList"] = skList;
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "EmpPhoto")]Employee employee)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase EmployeeImage = Request.Files["UserPhoto"];

                    using (var binary = new BinaryReader(EmployeeImage.InputStream))
                    {
                        imageData = binary.ReadBytes(EmployeeImage.ContentLength);
                    }
                }
                employee.EmpPhoto = imageData;
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Get_Employees_Result");
            }
            ViewBag.EmpPosition = new SelectList(db.Position, "PosID", "PosDesc", employee.EmpPosition);
            ViewBag.EmpProject = new SelectList(db.Projects, "ProID", "ProDesc", employee.EmpProject);
            //return View(employee);
            return RedirectToAction("Index", "Get_Employees_Result");
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            ViewBag.Project = db.Projects.Find(employee.EmpProject).ProDesc;
            ViewBag.Position = db.Position.Find(employee.EmpPosition).PosDesc;
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.DeleteEmployee(id);
            return RedirectToAction("Index", "Get_Employees_Result");
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
