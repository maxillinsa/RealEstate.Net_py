using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Models;
using CustomRoles;
using PagedList;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
namespace RealEstate.Controllers
{
    [CustomAuthorize(Roles = "Admin,Editor")]
    public class CategoryTypeController : Controller
    {
          private readonly ICategoryTypeRepository _CategoryTypeRepository;
          private const int pageSize = 30;
        //
        // GET: /CategoryType/
          public CategoryTypeController(ICategoryTypeRepository categoryTypeRepository)
              {
                  this._CategoryTypeRepository = categoryTypeRepository;
              }
        public ActionResult Index(int ? page)
        {
            int pageNum = (page ?? 1);
            List<CategoryType> model = new List<CategoryType>();
            model = _CategoryTypeRepository.GetAll();
            return View(model.ToPagedList(pageNum,pageSize));
        }

        //
        // GET: /CategoryType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CategoryType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryType CategoryType)
        {
            if (ModelState.IsValid)
            {
                _CategoryTypeRepository.Insert(CategoryType);
                return RedirectToAction("Index");
            }
            return View(CategoryType);
        }

        //
        // GET: /CategoryType/Edit/5

        public ActionResult Edit(long id = 0)
        {
            CategoryType CategoryType = _CategoryTypeRepository.GetById(id);
            if (CategoryType == null)
            {
                return HttpNotFound();
            }
            return View(CategoryType);
        }

        //
        // POST: /CategoryType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryType CategoryType)
        {
            if (ModelState.IsValid)
            {
                _CategoryTypeRepository.Edit(CategoryType);
                return RedirectToAction("Index");
            }
            return View(CategoryType);
        }

    }
}