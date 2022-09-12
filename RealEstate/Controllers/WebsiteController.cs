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
      [CustomAuthorize(Roles = "Admin,Editor")] // Editor là quyền quản trị website.
    public class WebsiteController : Controller
    {
        //
        // GET: /Estate/
        private IEstateRepository _IEstateRepository;
        private ITownRepository _ITownRepository;
        private ISaleUnitRepository _ISaleUnitRepository;
        private IRentUnitRepository _IRentUnitRepository;
        private  IAccountRepository _accountRepository;
        private ICommentRepository _commentRepository;
        private const int pageSize = 500;
        private string ImageUploadsFolder
        {
            get
            {
                return Path.Combine(Server.MapPath("/Media/Images/Estate"));
            }
        }
        public WebsiteController(ITownRepository khuPhoRepository,ISaleUnitRepository donViBanRepository,IRentUnitRepository donViThueRepository
            ,IEstateRepository estateRepository, ICommentRepository commentRepository, IAccountRepository accountRepository)
        {
            _ITownRepository = khuPhoRepository;
            _ISaleUnitRepository = donViBanRepository;
            _IRentUnitRepository = donViThueRepository;
            _IEstateRepository = estateRepository;
            _commentRepository = commentRepository;
            this._accountRepository = accountRepository;
         
        }

          public ActionResult Index()
        {
            return View();
        }
          public ActionResult QuanLyDanhMuc()
          {
              return RedirectToAction("Index", "Category");
          }
          public ActionResult QuanLySanPham()
          {
              return RedirectToAction("Index", "Product");
          }
          public ActionResult QuanLyTinBai()
          {
              return RedirectToAction("Index", "Article");
          }
          public ActionResult QuanLyLoaiDanhMuc()
          {
              return RedirectToAction("Index", "CategoryType");
          }
          public ActionResult QuanLyNhomDanhMuc()
          {
              return RedirectToAction("Index", "GroupCategory");
          }
          public ActionResult QuanLyNhaCungCap(){
              return RedirectToAction("Index", "Vendor");
          }
          public ActionResult QuanLyAlBum()
          {
              return RedirectToAction("Index", "Album");
          }
      

        private void loadData()
        {
            ViewBag.Khupho = new SelectList(_ITownRepository.GetAll(false), "TownId", "Name", null);
            ViewBag.Donviban = new SelectList(_ISaleUnitRepository.GetAll(false), "SaleUnitId", "Name", null);  
            ViewBag.Donvithue = new SelectList(_IRentUnitRepository.GetAll(false), "RentUnitId", "Name", null);
        }
        private string CreateNewName(string str)
        {
            return DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString() + str;
        }
    }
}
