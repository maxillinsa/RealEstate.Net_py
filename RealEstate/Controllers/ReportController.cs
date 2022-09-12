using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using RealEstate.Common;
using CustomRoles;
using MvcPaging;
using RealEstate.Models.ViewModels;

namespace RealEstate.Controllers
{
    [CustomAuthorize(Roles = "Admin,Staff,Editor")]
    public class ReportController : Controller
    {
        private const int pageSize = 200;
        #region Construction
        private IEstateRepository _IEstateRepository;
        private IEstate_GroupRepository _realestateGroupRepository;
        private IEstate_ProjectRepository _realestateProjectRepository;
        private IEstate_StatusRepository _realestateStatusRepository;
        private ITownRepository _ITownRepository;
        private ISaleUnitRepository _ISaleUnitRepository;
        private IRentUnitRepository _IRentUnitRepository;
        private IAccountRepository _accountRepository;
        private ICommentRepository _commentRepository;
        private IEstate_TypesRepository _Estate_TypesRepository;
        private IEstate_GroupRepository _Estate_GroupRepository;
        private IEstate_StatusRepository _estate_StatusRepository;
        private IProvinceRepository _provinceRepository;
        private IDistrictRepository _districtRepository;
        private IWardRepository _wardRepository;
        private IEstate_InvestorRepository _estate_InvestorRepository;
        private ICertificate_TypeRepository _certificate_TypeRepository;
        private ICountryRepository _countryRepository;

        public ReportController(IEstateRepository _IEstateRepository,
       IEstate_GroupRepository _realestateGroupRepository,
        IEstate_ProjectRepository _realestateProjectRepository,
         IEstate_StatusRepository _realestateStatusRepository,
         ITownRepository _ITownRepository,
         ISaleUnitRepository _ISaleUnitRepository,
         IRentUnitRepository _IRentUnitRepository,
         IAccountRepository _accountRepository,
         ICommentRepository _commentRepository,
         IEstate_TypesRepository _Estate_TypesRepository,
         IEstate_GroupRepository _estate_GroupRepository,
         IEstate_StatusRepository _estate_StatusRepository,
         IProvinceRepository _provinceRepository,
         IDistrictRepository _districtRepository,
         IWardRepository _wardRepository,
         ICertificate_TypeRepository _certificate_TypeRepository,
         ICountryRepository countryRepository, IEstate_InvestorRepository estate_InvestorRepository)
        {
            this._ITownRepository = _ITownRepository;
            this._ISaleUnitRepository = _ISaleUnitRepository;
            this._IRentUnitRepository = _IRentUnitRepository;
            this._IEstateRepository = _IEstateRepository;
            this._commentRepository = _commentRepository;
            this._realestateGroupRepository = _realestateGroupRepository;
            this._realestateProjectRepository = _realestateProjectRepository;
            this._realestateStatusRepository = _realestateStatusRepository;
            this._accountRepository = _accountRepository;
            this._Estate_TypesRepository = _Estate_TypesRepository;
            this._Estate_GroupRepository = _estate_GroupRepository;
            this._estate_StatusRepository = _estate_StatusRepository;
            this._provinceRepository = _provinceRepository;
            this._districtRepository = _districtRepository;
            this._wardRepository = _wardRepository;
            this._certificate_TypeRepository = _certificate_TypeRepository;
            this._countryRepository = countryRepository;
            this._estate_InvestorRepository = estate_InvestorRepository;
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult ModificationReport(int? page, string kyHieu)
        {


            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            Account currentAccount = new Account();
            currentAccount = _accountRepository.GetById(manv);
            ViewBag.manv = manv;
            int pageNum = (page ?? 1);
            ViewBag.page = pageNum;
            ViewBag.currentAccountId = currentAccount;
         
            List<Estate> ListEstate = new List<Estate>();
            ListEstate = _IEstateRepository.GetAll().OrderByDescending(x => x.UpdateDate).ToList();
          
            if (string.IsNullOrWhiteSpace(kyHieu))
            {
                ListEstate = ListEstate.ToList();
            }
            else
            {
                ListEstate = ListEstate.Where(x => x.Code != null).ToList();
                ListEstate = ListEstate.Where(p => p.Code.ToLower().Contains(kyHieu.ToLower())).ToList();
            }
         
             ViewData["kyHieu"] = kyHieu;
           
            if (Request.IsAjaxRequest())
                return PartialView("AjaxEstate", ListEstate.ToPagedList(pageNum, pageSize));
            return View(ListEstate.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ExpiredProduct(int? page, int? accountId, string kyHieu)
        {
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            if (Convert.ToInt64(accountId) != manv)
            {
                string message = "Wrong account error, please try again.";
                return RedirectToAction("ErrorNotification", "Home", new { message = message });
            }
            Account acc = new Account();
            acc = _accountRepository.GetById(Convert.ToInt64(accountId));
          
            Account currentAccount = new Account();
            currentAccount = _accountRepository.GetById(manv);
            ViewBag.manv = manv;
            int pageNum = (page ?? 1);
            ViewBag.page = pageNum;
            ViewBag.currentAccountId = currentAccount;

            List<Estate> ListEstate = new List<Estate>();
            ListEstate = _IEstateRepository.GetAll().OrderByDescending(x => x.UpdateDate).ToList();

            if (string.IsNullOrWhiteSpace(kyHieu))
            {
                ListEstate = ListEstate.ToList();
            }
            else
            {
                ListEstate = ListEstate.Where(x => x.Code != null).ToList();
                ListEstate = ListEstate.Where(p => p.Code.ToLower().Contains(kyHieu.ToLower())).ToList();
            }
            if (acc.DepartmentId != 1)
            {
                ListEstate = ListEstate.Where(x => x.AccountId == acc.AccountId).ToList();
            }
            List<Estate> lstTemp = new List<Estate>();
            foreach(var item in ListEstate)
            { 
                    DateTime today = DateTime.Now;
                    TimeSpan ngay = today.Subtract(Convert.ToDateTime(item.UpdateDate));
                    if (ngay.Days < 183)
                {
                    lstTemp.Add(item);
                }
            }
            ListEstate = ListEstate.Except(lstTemp).ToList();

            ViewData["kyHieu"] = kyHieu;
            ViewData["accountId"] = accountId; ViewBag.accountId = accountId;
            if (Request.IsAjaxRequest())
                return PartialView("AjaxExpiredProduct", ListEstate.ToPagedList(pageNum, pageSize));
            return View(ListEstate.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ExpiredProductReport(int? page, int? accountId, string kyHieu)
        {
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Account acc = new Account();
            acc = _accountRepository.GetById(Convert.ToInt64(accountId));
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            Account currentAccount = new Account();
            currentAccount = _accountRepository.GetById(manv);
            ViewBag.manv = manv;
            int pageNum = (page ?? 1);
            ViewBag.page = pageNum;
            ViewBag.account = _accountRepository.GetAll();
            ViewBag.currentAccountId = currentAccount;
            List<Estate> ListEstate = new List<Estate>();
            if (acc != null)
            {
                ListEstate = _IEstateRepository.GetAllByAccountID(Convert.ToInt32(acc.AccountId))
                    .OrderByDescending(x => x.UpdateDate).ToList();
            }
            else
            {
                ListEstate = _IEstateRepository.GetAll().OrderByDescending(x => x.UpdateDate).ToList();
            }
            if (string.IsNullOrWhiteSpace(kyHieu))
            {
                ListEstate = ListEstate.ToList();
            }
            else
            {
                ListEstate = ListEstate.Where(x => x.Code != null).ToList();
                ListEstate = ListEstate.Where(p => p.Code.ToLower().Contains(kyHieu.ToLower())).ToList();
            }
            List<Estate> lstTemp = new List<Estate>();
            foreach (var item in ListEstate)
            {
                DateTime today = DateTime.Now;
                TimeSpan ngay = today.Subtract(Convert.ToDateTime(item.UpdateDate));
                if (ngay.Days < 183)
                {
                    lstTemp.Add(item);
                }
            }
            ListEstate = ListEstate.Except(lstTemp).ToList();

            ViewData["kyHieu"] = kyHieu;
            ViewData["accountId"] = accountId; ViewBag.accountId = accountId;
            if (Request.IsAjaxRequest())
                return PartialView("AjaxExpiredProductReport", ListEstate.ToPagedList(pageNum, pageSize));
            return View(ListEstate.ToPagedList(pageNum, pageSize));
        }

        public ActionResult MyEstate(int? page, string kyHieu, int khuPho = 0, int Estate_TypeId = 0, int loaiMucGia = 0, int loaiBanThue = 0)
        {
            ViewBag.Code = null;
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            Account currentAccount = new Account();
            currentAccount = _accountRepository.GetById(manv);
            ViewBag.manv = manv;
            int pageNum = (page ?? 1);
            ViewBag.page = pageNum;
            ViewBag.currentAccountId = currentAccount;
            ViewBag.khuPho = khuPho;
            ViewData["kyHieu"] = kyHieu;
            ViewData["khuPho"] = khuPho;
            List<Estate> ListEstate = new List<Estate>();
            // Chi Load cac san pham cua tai khoan dang nhap
            ListEstate = _IEstateRepository.GetAllByAccountID(Convert.ToInt32(manv)).OrderByDescending(x => x.UpdateDate).ToList();
            List<Town> khuPhoList = new List<Town>();
            khuPhoList = _ITownRepository.GetAll();
            ViewBag.Khupho = khuPhoList.Distinct().OrderBy(x => x.Name).ToList();

            List<Estate_Types> Estate_TypesList = new List<Estate_Types>();
            Estate_TypesList = _Estate_TypesRepository.GetAll();
            ViewBag.Estate_Types = Estate_TypesList.Distinct().OrderBy(x => x.Name).ToList();

            if (string.IsNullOrWhiteSpace(kyHieu))
            {
                ListEstate = ListEstate.ToList();
            }
            else
            {
                ViewBag.Code = kyHieu.ToLower();
                string kyhieu2 = kyHieu.Replace(".", "").Replace("-", "").Replace("_", "");
                ListEstate = ListEstate.Where(x => x.Code != null).ToList();
                ListEstate = ListEstate.Where(p => p.Code.ToLower().Replace(".", "").Replace("-", "").Replace("_", "").Contains(kyhieu2.ToLower())).ToList();
            }
            if (khuPho != 0)
            {
                ListEstate = ListEstate.Where(x => x.TownId == khuPho).ToList();
            }
            if (Estate_TypeId != 0)
            {
                ListEstate = ListEstate.Where(x => x.Estate_TypeId == Estate_TypeId).ToList();
            }
            if (loaiMucGia != 0)
            {
                ListEstate = ListEstate.Where(x => x.SalePrice != 0 && x.SalePrice != null).ToList();
                switch (loaiMucGia)
                {
                    case 1:
                        ListEstate = ListEstate.Where(x => x.SalePrice <= 1).ToList();
                        break;
                    case 2:
                        ListEstate = ListEstate.Where(x => x.SalePrice > 1 && x.SalePrice <= 2).ToList();
                        break;
                    case 3:
                        ListEstate = ListEstate.Where(x => x.SalePrice > 2 && x.SalePrice <= 5).ToList();
                        break;
                    case 4:
                        ListEstate = ListEstate.Where(x => x.SalePrice > 5 && x.SalePrice <= 7).ToList();
                        break;
                    case 5:
                        ListEstate = ListEstate.Where(x => x.SalePrice > 7 && x.SalePrice <= 10).ToList();
                        break;
                    case 6:
                        ListEstate = ListEstate.Where(x => x.SalePrice > 10 && x.SalePrice <= 20).ToList();
                        break;
                    case 7:
                        ListEstate = ListEstate.Where(x => x.SalePrice > 20 && x.SalePrice <= 30).ToList();
                        break;
                    case 8:
                        ListEstate = ListEstate.Where(x => x.SalePrice > 30 && x.SalePrice <= 50).ToList();
                        break;
                    case 9:
                        ListEstate = ListEstate.Where(x => x.SalePrice >= 50).ToList();
                        break;

                    default:                       
                        break;
                }
            }
            if (loaiBanThue != 0)
            {
                if (loaiBanThue == 1)
                    ListEstate = ListEstate.Where(x => x.SalePrice != 0).ToList();
                if (loaiBanThue == 2)
                    ListEstate = ListEstate.Where(x => x.RentPrice != 0).ToList();
            }
            // Đối với tài khoản thường thì sản phẩm quá 180 ngày ko cập nhật dù là thuê hay bán là ẩn       
            if (currentAccount.DepartmentId != 1)
            {
                foreach (var item in ListEstate)
                {
                    DateTime today = DateTime.Now;
                    TimeSpan ngay = today.Subtract(Convert.ToDateTime(item.UpdateDate));
                    if (ngay.Days >= 180)
                    {
                        ListEstate = ListEstate.Where(x => x.Id != item.Id).ToList();
                    }
                }
            }
            if (Request.IsAjaxRequest())
                return PartialView("AjaxMyEstate", ListEstate.ToPagedList(pageNum, pageSize));
            return View(ListEstate.ToPagedList(pageNum, pageSize));
        }


        public ActionResult Detail(long id)
        {
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            ViewBag.AccountId = manv;         


            EstateDetailViewModel model = new EstateDetailViewModel();


            model = _IEstateRepository.GetEstateDetailViewById(id);
            var myAccountId = Convert.ToInt32(model.AccountId);
            model.TotalEstates = _IEstateRepository.GetAllByAccountID(myAccountId).Count;

            model.lstComments = _commentRepository.GetByEstateId(id);
            return View(model);
        }

        private void loadData()
        {
            ViewBag.Khupho = new SelectList(_ITownRepository.GetAll(false).OrderBy(x => x.Name).ToList(), "TownId", "Name", null);
            ViewBag.Donviban = new SelectList(_ISaleUnitRepository.GetAll(false), "SaleUnitId", "Name", null);
            ViewBag.Donvithue = new SelectList(_IRentUnitRepository.GetAll(false), "RentUnitId", "Name", null);
        }
        private string CreateNewName(string str)
        {
            return DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString() + str;
        }
    }
}
