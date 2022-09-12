using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Models;
using CustomRoles;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using PagedList;

namespace RealEstate.Controllers
{
 [CustomAuthorize(Roles = "SuperAdmin,Admin,Staff")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentsRepository _departmentRepository;
        private readonly ICompanyRepository _companyRepository;
        private const int pageSize = 30;
        public DepartmentsController(IDepartmentsRepository departmentsRepository, ICompanyRepository companyRepository)
        {
            this._departmentRepository = departmentsRepository;
            this._companyRepository = companyRepository;
        }
        //
        // GET: /Departments/

        public ActionResult Index(int? page)
        {
             int pageNum = (page ?? 1);
            IList<Department> model = new List<Department>();
            model = _departmentRepository.GetAll();

            return View(model.ToPagedList(pageNum,pageSize));
        }

        //
        // GET: /Departments/Create

        public ActionResult Create()
        {
            LoadData();
            return View();
        }

        //
        // POST: /Departments/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                department.CreatDate = DateTime.Now;
                department.IsDelete = false;
                _departmentRepository.Insert(department);
                return RedirectToAction("Index");
            }

            return View(department);
        }

        //
        // GET: /Departments/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Department department = _departmentRepository.GetById(id);
            LoadData();
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // POST: /Departments/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentRepository.Edit(department);
                return RedirectToAction("Index");
            }
            return View(department);
        }

        //
        // GET: /Departments/Delete/5

        public ActionResult Delete(long id)
        {
            _departmentRepository.Delete(id, true);
            return RedirectToAction("Index");
        }
        public ActionResult UnDelete(long id)
        {
            _departmentRepository.Delete(id, false);
            return RedirectToAction("Index");
        }

        private void LoadData()
        {
            ViewBag.Companies = new SelectList(_companyRepository.GetAll(), "Id", "CompanyName", null);

        }


    }
}