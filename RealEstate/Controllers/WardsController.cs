using MvcPaging;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public class WardsController : Controller
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

        public WardsController(IEstateRepository _IEstateRepository,
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
        public async Task<ActionResult> Index(int? page, long districtId = 0, string name = null)
        {
            LoadData();
            int pageNum = (page ?? 1);
            ViewBag.Title = "Manage Wards";
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            Account currentAccount = new Account();
            currentAccount = _accountRepository.GetById(manv);
            ViewBag.manv = manv;
            ViewBag.page = pageNum;
            ViewBag.currentAccountId = currentAccount;

            List<WardViewModel> model = new List<WardViewModel>();
            model = await _wardRepository.GetList();
            if (string.IsNullOrWhiteSpace(name))
            {
                model = model.ToList();
            }
            else
            {
                model = model.Where(x => x.Name != null).ToList();
                model = model.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
            }
            if (districtId != 0)
            {
                model = model.Where(x => x.DistrictId == districtId).ToList();
            }
            ViewData["name"] = name;
            if (Request.IsAjaxRequest())
                return PartialView("AjaxList", model.ToPagedList(pageNum, pageSize));
            return View(model.ToPagedList(pageNum, pageSize));

        }

        // GET: Wards/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WardViewModel model = await _wardRepository.GetById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Wards/Create
        public ActionResult Create()
        {
            LoadData();
            return View();
        }
        // POST: Wards/Create/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ItemId,IsDelete,Name,Content")] WardViewModel model)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                model.Modified = now;
                await _wardRepository.Create(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        // GET: Wards/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WardViewModel model = await _wardRepository.GetById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            LoadData();
            return View(model);
        }

        // POST: Wards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ItemId,IsDelete,Name,Content")] WardViewModel model)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                model.Modified = now;
                await _wardRepository.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public async Task<JsonResult> UpdateIsDelete(long itemId)
        {
            string message = string.Empty;
            JsonModelReturnViewWard json = new JsonModelReturnViewWard();
            try
            {
                var task = await _wardRepository.UpdateIsDelete(itemId, true);
                if (task == true)
                {
                    json.Ward = await _wardRepository.GetById(itemId);
                    json.messages = "Update successfully.";
                    json.isError = false;
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [HttpPost, ValidateInput(false)]
        public async Task<JsonResult> UnUpdateIsDelete(long itemId)
        {
            string message = string.Empty;
            JsonModelReturnViewWard json = new JsonModelReturnViewWard();
            try
            {
                var task = await _wardRepository.UpdateIsDelete(itemId, false);
                if (task == true)
                {
                    json.Ward = await _wardRepository.GetById(itemId);
                    json.messages = "Update successfully.";
                    json.isError = false;
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        // GET: Admin/Create
        public ActionResult CreateAjax()
        {
            LoadData();
            return PartialView();
        }

        // POST: Admin/Create
        [HttpPost]
        public async Task<ActionResult> CreateAjax(WardViewModel model)
        {
            try
            {

                JsonModelReturnViewWard json = new JsonModelReturnViewWard();
                var WardTask = await _wardRepository.Create(model);
                if (WardTask)
                {
                    json.Ward = model;
                    json.isError = false;
                    json.isExit = false;
                    return Json(json);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        // GET: Admin/Edit/5
        public async Task<ActionResult> EditAjax(long id)
        {
            var my = await _wardRepository.GetById(id);
            LoadData();
            return PartialView(my);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public async Task<JsonResult> EditAjax(WardViewModel model)
        {
            try
            {
                JsonModelReturnViewWard json = new JsonModelReturnViewWard();

                var WardTask = await _wardRepository.Update(model);

                if (WardTask)
                {
                    json.Ward = await _wardRepository.GetById(model.ItemId);
                    json.isExit = false;
                    json.isError = false;
                    return Json(json);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult GetAllDistrictByProvinceId(long provinceId)
        {
            IEnumerable<DistrictViewModel> model = new List<DistrictViewModel>();
            model = _districtRepository.GetAllDistrictsByProvinceId(provinceId, false).OrderBy(x => x.Name);            
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetAllWardByDistrictId(long districtId)
        {

            IEnumerable<WardViewModel> model = new List<WardViewModel>();
            model = _wardRepository.GetAllWardsByDistrictId(districtId, false).OrderBy(x => x.Name);
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        private void LoadData()
        {
            ViewBag.Countries = new SelectList(_countryRepository.GetAll(false), "ItemId", "Name", null);
            ViewBag.Provinces = new SelectList(_provinceRepository.GetAll(false), "ItemId", "Name", null);
            ViewBag.Districts = new SelectList(_districtRepository.GetAll(false), "ItemId", "Name", null);
        }

      
    }
}
