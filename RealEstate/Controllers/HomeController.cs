using CustomRoles;
using PagedList;
using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace RealEstate.Controllers
{
    [CustomAuthorize(Roles = "Admin,Staff,Editor")] // Staff là quyền nhan viên Sale  - Editor: Quản trị Website
    public class HomeController : Controller
    {
      
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
        private IProvinceRepository _provinceRepository;
        private IDistrictRepository _districtRepository;
        private IWardRepository _wardRepository;
        private IEstate_InvestorRepository _estate_InvestorRepository;
        private ICertificate_TypeRepository _certificate_TypeRepository;
        private ICountryRepository _countryRepository;
        private INotificationRepository _thongBaoRepository;
        public HomeController(IEstateRepository _IEstateRepository,
       IEstate_GroupRepository _realestateGroupRepository,
        IEstate_ProjectRepository _realestateProjectRepository,
         IEstate_StatusRepository _realestateStatusRepository,
         ITownRepository _ITownRepository,
         ISaleUnitRepository _ISaleUnitRepository,
         IRentUnitRepository _IRentUnitRepository,
         IAccountRepository _accountRepository,
         ICommentRepository _commentRepository,
         IEstate_TypesRepository _Estate_TypesRepository,
         IEstate_GroupRepository estate_GroupRepository,
         IEstate_StatusRepository realestateStatusRepository,
         IProvinceRepository _provinceRepository,
         IDistrictRepository _districtRepository,
         IWardRepository _wardRepository,
         ICertificate_TypeRepository _certificate_TypeRepository,
         ICountryRepository countryRepository, INotificationRepository thongBaoRepository, IEstate_InvestorRepository estate_InvestorRepository)
        {
            this._ITownRepository = _ITownRepository;
            this._ISaleUnitRepository = _ISaleUnitRepository;
            this._IRentUnitRepository = _IRentUnitRepository;
            this._IEstateRepository = _IEstateRepository;
            this._commentRepository = _commentRepository;
            this._realestateGroupRepository = _realestateGroupRepository;
            this._realestateProjectRepository = _realestateProjectRepository;
            this._realestateStatusRepository = realestateStatusRepository;
            this._accountRepository = _accountRepository;
            this._Estate_TypesRepository = _Estate_TypesRepository;
            _Estate_GroupRepository = estate_GroupRepository;
            this._provinceRepository = _provinceRepository;
            this._districtRepository = _districtRepository;
            this._wardRepository = _wardRepository;
            this._certificate_TypeRepository = _certificate_TypeRepository;
            _countryRepository = countryRepository;
            _estate_InvestorRepository = estate_InvestorRepository;
            _thongBaoRepository = thongBaoRepository;
        }
        #endregion


        #region All
        public ActionResult Index(int? page)
        {
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int pageNum = (page ?? 1);

            ViewBag.tongSoSP = _IEstateRepository.CountTotalEstates();

            List<Estate> allSPHot = _IEstateRepository.GetAll().Where(x => x.IsHot == true && x.IsDelete == false).OrderByDescending(x => x.CreateDate).ToList();
            ViewBag.tongSoSPHot = _IEstateRepository.CountTotalEstatesHot();

            List<Account> allAccount = new List<Account>();
            allAccount = _accountRepository.GetAll().Where(x => x.IsDelete == false).ToList();
            ViewBag.allAccount = allAccount;

            List<Notification> allNotification = new List<Notification>();
            allNotification = _thongBaoRepository.GetAll().Where(x => x.IsDelete == false).ToList();

            ViewBag.tongSoNotification = allNotification.Count();

            ViewBag.allNotification = allNotification.Where(x => x.IsDelete == false).ToList();

            List<Town> khuPhoList = new List<Town>();
            khuPhoList = _ITownRepository.GetAll().Where(x => x.IsDelete == false).ToList();
            ViewBag.Khupho = khuPhoList.Distinct().OrderBy(x => x.Name).ToList();

            List<Estate_Types> Estate_TypeList = new List<Estate_Types>();
            Estate_TypeList = _Estate_TypesRepository.GetAll().Where(x => x.IsDelete == false).ToList();
            ViewBag.Estate_Types = Estate_TypeList.Distinct().OrderBy(x => x.Name).ToList();


            List<Estate> EstateMoi = new List<Estate>();
            EstateMoi = _IEstateRepository.GetAllThisDay(false).OrderByDescending(x => x.CreateDate).Take(20).ToList();
            ViewBag.EstateMoi = EstateMoi;
            return View(allSPHot.ToPagedList(pageNum, 6));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        public ActionResult CommentPartial()
        {
            List<Comment> model = new List<Comment>();
            model = _commentRepository.GetAllToDay().OrderByDescending(x => x.CreateDate).Take(20).ToList();
            return PartialView(model);

        }

        [ChildActionOnly]
        public ActionResult EstatesNewPartial()
        {
            List<Estate> model = new List<Estate>();
            model = _IEstateRepository.GetAllThisDay(false).ToList();
            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult SidebarPartial()
        {
            ViewBag.admin = 0;
            string accountId;
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            accountId = HttpContext.Session["bds_Acc_id"].ToString();
            Account currentAccount = new Account();
            currentAccount = _accountRepository.GetById(Convert.ToInt64(accountId));
            if (currentAccount.DepartmentId == 1)
            {
                ViewBag.admin = 1;
            }
            ViewBag.currentAccountId = currentAccount;
            return PartialView(currentAccount);

        }
        public ActionResult Comments()
        {
            List<Comment> model = new List<Comment>();
            model = _commentRepository.GetAll();
            return View(model);
        }
        public ActionResult ErrorNotification(string message)
        {
            ViewBag.message = message;
            return View();
        }
        private void loadData()
        {
            ViewBag.Khupho = new SelectList(_ITownRepository.GetAll(false).OrderBy(x => x.Name).ToList(), "TownId", "Name", null);
            ViewBag.Donviban = new SelectList(_ISaleUnitRepository.GetAll(false), "SaleUnitId", "Name", null);
            ViewBag.Donvithue = new SelectList(_IRentUnitRepository.GetAll(false), "RentUnitId", "Name", null);
            ViewBag.Estate_Types = new SelectList(_Estate_TypesRepository.GetAll(), "ItemId", "Name", null);
        }
        #endregion
    }
}