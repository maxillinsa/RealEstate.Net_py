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
    public class GroupCategoryController : Controller
    {
        //private PerfectRealDataContext db = new PerfectRealDataContext();
          private readonly IGroupCategoryRepository _groupCategoryRepository;
          private const int pageSize = 30;
        //
        // GET: /GroupCategory/
          public GroupCategoryController(IGroupCategoryRepository groupCategoryRepository)
              {
                  this._groupCategoryRepository = groupCategoryRepository;
              }
        public ActionResult Index(int ? page)
        {
            int pageNum = (page ?? 1);
            List<GroupCategory> model = new List<GroupCategory>();
            model = _groupCategoryRepository.GetAll();

            return View(model.ToPagedList(pageNum,pageSize));
        }

        //
        // GET: /GroupCategory/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /GroupCategory/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupCategory groupcategory)
        {
            if (ModelState.IsValid)
            {
                groupcategory.IsDelete = false;
                groupcategory.CreatedDate = DateTime.Now;
                _groupCategoryRepository.Insert(groupcategory);
                return RedirectToAction("Index");
            }
            return View(groupcategory);
        }

        //
        // GET: /GroupCategory/Edit/5

        public ActionResult Edit(long id = 0)
        {
            GroupCategory groupcategory = _groupCategoryRepository.GetById(id);
            if (groupcategory == null)
            {
                return HttpNotFound();
            }
            return View(groupcategory);
        }

        //
        // POST: /GroupCategory/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GroupCategory groupcategory)
        {
            if (ModelState.IsValid)
            {
                _groupCategoryRepository.Edit(groupcategory);
                return RedirectToAction("Index");
            }
            return View(groupcategory);
        }

        //
        // GET: /GroupCategory/Delete/5

        public ActionResult Delete(long id = 0)
        {
            _groupCategoryRepository.Delete(id, true);
            return RedirectToAction("Index"); 
        }
        public ActionResult UnDelete(long id = 0)
        {
            _groupCategoryRepository.Delete(id, false);
            return RedirectToAction("Index");
        }
    }
}