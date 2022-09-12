using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using RealEstate.Common;
using CustomRoles;
namespace RealEstate.Controllers
{
      [CustomAuthorize(Roles = "Admin,Editor,Staff")]
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        private IProductRepository _IproductRepository;
        private ICategoryRepository _ICategoryRepository;
        private IVendorRepository _IvendorRepository;
        private IAlbumRepository _albumRepository;
        private const int pageSize = 30;
        private string ImageUploadsFolder
        {
            get
            {
                return Path.Combine(Server.MapPath("/Media/Images/product"));
            }
        }
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository,IVendorRepository vendorRepository,IAlbumRepository albumRepository)
        {
            _IproductRepository = productRepository;
            _IvendorRepository = vendorRepository;
            _ICategoryRepository = categoryRepository;
            _albumRepository = albumRepository;
           
        }
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            List<Product> ListProduct = new List<Product>();
            ViewBag.Vendor = _IvendorRepository.GetAll();
            ListProduct = _IproductRepository.GetAll();
            return View(ListProduct.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Create()
        {
            ViewBag.ListTags = GetTagsProducts();
            loadData();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Product model, HttpPostedFileBase txtFile)
        {
            ViewBag.error = null;
            ViewBag.ListTags = GetTagsProducts();
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
                model.IsDelete = false;
               
                if(_IproductRepository.CheckExit(model.ProductName))
                    {
                        ViewBag.error = "This product already existed";
                        loadData();
                        return View(model);
                }
                if (model.Alias == null)
                    model.Alias = Functions.generateAlias(model.ProductName);
                if (model.MetaTitle == null)
                    model.MetaTitle = model.ProductName;
               // long productDetailId = _productDetailRepository.Insert(model.ProductDetail);
               // model.ProductDetailId = productDetailId;
                var rs = _IproductRepository.Insert(model);
                if (model.ProductCode == null || model.ProductCode == "")
                { 
                    model.ProductCode = "8930" + rs.ToString();
                    model.ProductId =  rs;
                    _IproductRepository.Edit(model);
                }
                return RedirectToAction("Index");
            }
            else
                loadData();
                return View(model);
        }

        public ActionResult Edit(long id)
        
        {
            Product model = _IproductRepository.GetById(id);
            //if (model.ProductCode != null && model.ProductCode != "")
            //    {
            //    //model.ProductCode
            //     model.ProductCode = BarCodeToHTML.get39(model.ProductCode, 2,40);
            //    }
            ViewBag.ListTags = GetTagsProducts();
            
            loadData();
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Product model, HttpPostedFileBase txtFile)
        {
            ViewBag.ListTags = GetTagsProducts();
            ViewBag.error = null;
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
                if(model.ProductCode == null || model.ProductCode =="" || model.ProductCode.Length >=15)
                model.ProductCode = "8930" + model.ProductId.ToString();
                model.ModifiedDate = DateTime.Now;
                if (_IproductRepository.CheckExit(model.ProductName))
                {
                    ViewBag.error = "Edit successfully!";
                    loadData();
                    var rs = _IproductRepository.Edit(model);         
                    model.ProductCode = BarCodeToHTML.get39(model.ProductCode, 2, 40);
                    return View(model);
                }
                if (model.Alias == null)
                    model.Alias = Functions.generateAlias(model.ProductName);
                if (model.MetaTitle == null)
                    model.MetaTitle = model.ProductName;
                //var e = _productDetailRepository.Edit(model.ProductDetail);
                _IproductRepository.Edit(model);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Error encountered, please try again!";
                loadData();
                return View(model);
            }
        }
        public ActionResult Delete(long id)
        {
            _IproductRepository.Delete(id, true);
            return RedirectToAction("Index");
        }
        public ActionResult UnDelete(long id)
        {
            _IproductRepository.Delete(id, false);
            return RedirectToAction("Index");
        }
        private void loadData()
        {
           ViewBag.Vendor = new SelectList(_IvendorRepository.GetAll(false), "VendorId", "VendorName", null);
          //  ViewBag.Warehouse = new SelectList(_WarehourRepository.GetAll(false), "WarehouseId", "WarehouseName", null);
            ViewBag.Category = new SelectList(_ICategoryRepository.GetAll(false), "CategoryId", "CategoryName", null);
            ViewBag.Albums = new SelectList(_albumRepository.GetAll(false), "AlbumId", "Name", null);
        }
        private string CreateNewName(string str)
        {
            return DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString() + str;
        }
        private string GetTagsProducts()
        {
            var list = _IproductRepository.GetAll();
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
    }
}
