using CustomRoles;
using MvcPaging;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace RealEstate.Controllers
{
    [CustomAuthorize(Roles = "Admin,Staff,Editor")]
    public class Estate_TypesController : Controller
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
        private IProvinceRepository _provinceRepository;
        private IDistrictRepository _districtRepository;
        private IWardRepository _wardRepository;
        private IEstate_InvestorRepository _estate_InvestorRepository;
        private ICertificate_TypeRepository _certificate_TypeRepository;
        private ICountryRepository _countryRepository;

        public Estate_TypesController(IEstateRepository _IEstateRepository,
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
         IEstate_StatusRepository realestateStatusRepository,
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
            this._realestateStatusRepository = realestateStatusRepository;
            this._accountRepository = _accountRepository;
            this._Estate_TypesRepository = _Estate_TypesRepository;
            this._Estate_GroupRepository = _estate_GroupRepository;
            this._provinceRepository = _provinceRepository;
            this._districtRepository = _districtRepository;
            this._wardRepository = _wardRepository;
            this._certificate_TypeRepository = _certificate_TypeRepository;
            this._countryRepository = countryRepository;
            this._estate_InvestorRepository = estate_InvestorRepository;
        }
        #endregion
        public ActionResult Index(int? page)
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

            List<Estate_Types> ListEstate_Types = new List<Estate_Types>();
            ListEstate_Types = _Estate_TypesRepository.GetAll().OrderByDescending(x => x.Name).ToList();
            
            if (Request.IsAjaxRequest())
                return PartialView("AjaxEstate_Type", ListEstate_Types.ToPagedList(pageNum, pageSize));
            return View(ListEstate_Types.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Create()
        {
            //var lstLoaiHangHoa = new List<LoaiHangHoa>();
            //lstLoaiHangHoa = adminRepository.GetAllLoaiHangHoa().Where(x => x.IsDelete != true).ToList();
            //ViewBag.model = lstLoaiHangHoa;
            return PartialView();
        }

        [HttpPost]
        public JsonResult Create(Estate_Types model)
        {
            model.CreateDate = DateTime.Now;
            model.EditDate = DateTime.Now;
            if (_Estate_TypesRepository.Create(model))
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
            Estate_Types m = new Estate_Types();
            m = _Estate_TypesRepository.GetById(id);
            if (m == null)
            {
                return HttpNotFound();
            }
           // ViewBag.LoaiHangHoaId = new SelectList(adminRepository.GetAllLoaiHangHoa().Where(x => x.IsDelete != true).ToList(), "LoaiHangHoaId", "Name", m.LoaiHangHoaId);
            return PartialView(m);
        }
        [HttpPost]
        public JsonResult Edit(Estate_Types model)
        {
            model.EditDate = DateTime.Now;
            if (_Estate_TypesRepository.Edit(model))
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(long id)
        {
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            ViewBag.AccountId = manv;
            Estate_Types model = new Estate_Types();
            model = _Estate_TypesRepository.GetById(id);
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
