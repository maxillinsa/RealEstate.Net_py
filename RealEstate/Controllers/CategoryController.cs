using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Models;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using System.IO;
using RealEstate.Common;
using CustomRoles;
using PagedList;

namespace RealEstate.Controllers
{
    [CustomAuthorize(Roles = "Admin,Editor")]
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoryReposiory;
        private IGroupCategoryRepository _groupCategoryReposiory;
        private readonly ICategoryTypeRepository _categoryTypeRepository;
        private const int pageSize = 35;
        private string ImageUploadsFolder
        {
            get
            {
                return Path.Combine(Server.MapPath("/Media/Images/category"));
            }
        }
        public CategoryController(ICategoryRepository categoryRepository, IGroupCategoryRepository groupCategoryRepository, ICategoryTypeRepository categoryTypeRepository)
        {
            _categoryReposiory = categoryRepository;
            _groupCategoryReposiory =groupCategoryRepository;
            this._categoryTypeRepository = categoryTypeRepository;
        }
        //
        // GET: /Category/

        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            ViewBag.GroupCategory = _groupCategoryReposiory.GetAll();
            List<Category> ListCategory = _categoryReposiory.GetAll().OrderByDescending(x=>x.OrderNumber).ToList();
            return View(ListCategory.ToPagedList(pageNum, pageSize));
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            LoadData();
            return View();
        }
        //
        // POST: /Category/Create

      
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Category category, HttpPostedFileBase txtFile)
        {
            if (ModelState.IsValid)
            {
                if (txtFile != null)
                {
                    if (txtFile.ContentLength / 1024 > 1500)
                    {
                        ModelState.AddModelError("txtFile", "Image maximum 1.5MB");
                    }
                    string server = string.Empty;
                    server = ImageUploadsFolder;
                    String nameImage = CreateNewName(Path.GetExtension(txtFile.FileName));
                    var fullName = Path.Combine(server, nameImage);
                    ImageHelper.UploadImage(txtFile, fullName);
                    category.Image = nameImage;
                }
                else
                {
                    ModelState.AddModelError("txtFile", "Image 1 is empty");
                    LoadData();
                    return View(category);
                }
                if(category.Alias == null)
                category.Alias = Functions.generateAlias(category.CategoryName);
                if (category.MetaTitle == null)
                    category.MetaTitle = category.CategoryName;

                category.IsDelete = false;
                category.CreatedDate = DateTime.Now;
                category.ModifiedDate = DateTime.Now;
                var rs = _categoryReposiory.Insert(category);
                return RedirectToAction("Index");
                //return RedirectToAction("Edit", new { id = rs });
            }
            else
                return View(category);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(long id)
        {
            LoadData();
            var model = _categoryReposiory.GetById(id);
            return View(model);
        }

        //
        // POST: /Category/Edit/5

       [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Category category, HttpPostedFileBase txtFile)
        {
            if (ModelState.IsValid)
            {
                if (txtFile != null)
                {
                    if (txtFile.ContentLength / 1024 > 1500)
                    {
                        ModelState.AddModelError("txtFile", "Image maximum 1.5MB");
                    }
                    string server = string.Empty;
                    server = ImageUploadsFolder;
                    String nameImage = CreateNewName(Path.GetExtension(txtFile.FileName));
                    var fullName = Path.Combine(server, nameImage);
                    ImageHelper.UploadImage(txtFile, fullName);
                    category.Image = nameImage;
                }
                if (category.Alias == null)
                    category.Alias = Functions.generateAlias(category.CategoryName);
                if (category.MetaTitle == null)
                    category.MetaTitle = category.CategoryName;
                category.ModifiedDate = DateTime.Now;
                _categoryReposiory.Edit(category);
                return RedirectToAction("Index");
            }
            else
                return View(category);
        }

        //
        // GET: /Category/Delete/5

        public ActionResult Delete(long id)
        {
            _categoryReposiory.Delete(id, true);
            return RedirectToAction("Index");
        }
        public ActionResult UnDelete(long id)
        {
            _categoryReposiory.Delete(id, false);
            return RedirectToAction("Index");
        }

        private void LoadData()
        {
            ViewBag.GroupCategory = new SelectList(_groupCategoryReposiory.GetAll(false), "GroupCategoryId", "GroupCategoryName", null);
            ViewBag.CategoryType = new SelectList(_categoryTypeRepository.GetAll(false), "CategoryTypeId", "Name", null);
            ViewBag.CategoryParent = new SelectList(_categoryReposiory.GetAll(false), "CategoryId", "CategoryName", null);
        }
      
        private string CreateNewName(string str)
        {
            return DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString() + str;
        }
    }
}