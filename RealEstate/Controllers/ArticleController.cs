using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Models;
using RealEstate.DAL.Repository;
using RealEstate.DAL.IRepository;
using System.IO;
using PagedList;
using RealEstate.Common;

namespace RealEstate.Controllers
{
    public class ArticleController : Controller
    {
       private readonly IArticleRepository _articleRepository;
       private ICategoryRepository _categoryRepository;
       private const int pageSize = 35;
       private string ImageUploadsFolder
       {
           get
           {
               return Path.Combine(Server.MapPath("/Media/Images/article"));
           }
       }
       public ArticleController(IArticleRepository articleRepository, ICategoryRepository categoryRepository)
        {
            this._articleRepository = articleRepository;
            this._categoryRepository = categoryRepository;
        }
       public ActionResult Index(int? page)
       {
           int pageNum = (page ?? 1);
           List<Article> ListArticle = _articleRepository.GetAll();
           List<Category> lstCategory = new List<Category>();
           lstCategory = _categoryRepository.GetAll();
           ViewBag.lstCategory = lstCategory;
           return View(ListArticle.ToPagedList(pageNum, pageSize));
       }
       public ActionResult Create()
       {
           ViewBag.ListTags = GetTagsArticles();
           loadData();
           return View();
       }
       [HttpPost, ValidateInput(false)]
       public ActionResult Create(Article model, HttpPostedFileBase txtFile)
       {
           ViewBag.error = null;
           ViewBag.ListTags = GetTagsArticles();
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
                   model.Image = nameImage;
               }
               model.CreateDate = DateTime.Now;      

               if (model.Alias == null)
                   model.Alias = Functions.generateAlias(model.Title);
               if (model.MetaTitle == null)
                   model.MetaTitle = model.Title;
               model.CreateDate = DateTime.Now;
               var rs = _articleRepository.Insert(model);
              
               return RedirectToAction("Index");
           }
           else
               loadData();
           return View(model);
       }
       public ActionResult Edit(long id)
       {
           ViewBag.ListTags = GetTagsArticles();
           Article model = new Article();
           model = _articleRepository.GetById(id);
           loadData();
           return View(model);
       }
       [HttpPost, ValidateInput(false)]
       public ActionResult Edit(Article model, HttpPostedFileBase txtFile)
       {
           ViewBag.error = null;
           ViewBag.ListTags = GetTagsArticles();
           if (ModelState.IsValid)
           {
               if (txtFile != null)
               {
                   if (txtFile.ContentLength / 1024 > 1500)
                   {
                       ModelState.AddModelError("txtFile", "Image maximum 1.5MB");
                       loadData();
                       return View(model);
                   }
                   string server = string.Empty;
                   server = ImageUploadsFolder;
                   String nameImage = CreateNewName(Path.GetExtension(txtFile.FileName));
                   var fullName = Path.Combine(server, nameImage);
                   ImageHelper.UploadImage(txtFile, fullName);
                   model.Image = nameImage;
               }             
               if (model.Alias == null)
                   model.Alias = Functions.generateAlias(model.Title);
               if (model.MetaTitle == null)
                   model.MetaTitle = model.Title;
               var rs = _articleRepository.Edit(model);
               return RedirectToAction("Index");
           }
           else
               loadData();
           return View(model);
       }
       public ActionResult Delete(long id)
       {
           _articleRepository.Delete(id, true);
           return RedirectToAction("Index");
       }
       public ActionResult UnDelete(long id)
       {
           _articleRepository.Delete(id, false);
           return RedirectToAction("Index");
       }
       private string CreateNewName(string str)
       {
           return DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString() + str;
       }
       private string GetTagsArticles()
       {
           var list = _articleRepository.GetAll();
           string temp = "";
           foreach (var item in list)
           {
               temp += item.Tags;
           }
           var listTemp = temp.Split(',').Distinct().ToList();
           var rs = "";
           foreach (var item in listTemp)
           {
               rs += item + ",";
           }
           return rs;
       }
       private void loadData()
       {
           ViewBag.Category = new SelectList(_categoryRepository.GetAll(false), "CategoryId", "CategoryName", null);
       }

    }
}