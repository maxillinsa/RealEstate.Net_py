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
    public class Estate_ProjectsController : Controller
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

        public Estate_ProjectsController(IEstateRepository _IEstateRepository,
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
        public async Task<ActionResult> Index(int? page, string name = null)
        {
            int pageNum = (page ?? 1);
            ViewBag.Title = "Manage Estate_Projects";
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

            List<Estate_ProjectViewModel> model = new List<Estate_ProjectViewModel>();
            model = await _realestateProjectRepository.GetList();
            if (string.IsNullOrWhiteSpace(name))
            {
                model = model.ToList();
            }
            else
            {
                model = model.Where(x => x.Name != null).ToList();
                model = model.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
            }

            ViewData["name"] = name;
            if (Request.IsAjaxRequest())
                return PartialView("AjaxList", model.ToPagedList(pageNum, pageSize));
            return View(model.ToPagedList(pageNum, pageSize));

        }

        // GET: Estate_Projects/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estate_ProjectViewModel model = await _realestateProjectRepository.GetById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Estate_Projects/Create
        public ActionResult Create()
        {
            LoadData();
            return View();
        }
        // POST: Estate_Projects/Create/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ItemId,Estate_InvestorId,Name,Content")] Estate_ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                model.Modified = now;
                model.IsDelete = false;
                await _realestateProjectRepository.Create(model);
                return RedirectToAction("Index");
            }
            LoadData();
            return View(model);
        }
        // GET: Estate_Projects/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoadData();
            Estate_ProjectViewModel model = await _realestateProjectRepository.GetById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Estate_Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ItemId,Estate_InvestorId,Name,Content")] Estate_ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                model.Modified = now;
                await _realestateProjectRepository.Update(model);
                return RedirectToAction("Index");
            }
            LoadData();
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public async Task<JsonResult> UpdateIsDelete(long itemId)
        {
            string message = string.Empty;
            JsonModelReturnViewEstate_Project json = new JsonModelReturnViewEstate_Project();
            try
            {
                var task = await _realestateProjectRepository.UpdateIsDelete(itemId, true);
                if (task == true)
                {
                    json.Estate_Project = await _realestateProjectRepository.GetById(itemId);
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
            JsonModelReturnViewEstate_Project json = new JsonModelReturnViewEstate_Project();
            try
            {
                var task = await _realestateProjectRepository.UpdateIsDelete(itemId, false);
                if (task == true)
                {
                    json.Estate_Project = await _realestateProjectRepository.GetById(itemId);
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
        public async Task<ActionResult> CreateAjax(Estate_ProjectViewModel model)
        {
            try
            {

                JsonModelReturnViewEstate_Project json = new JsonModelReturnViewEstate_Project();
                model.IsDelete = false;
                var Estate_ProjectTask = await _realestateProjectRepository.Create(model);
                if (Estate_ProjectTask)
                {
                    json.Estate_Project = model;
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
            var my = await _realestateProjectRepository.GetById(id);
            LoadData();
            return PartialView(my);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public async Task<JsonResult> EditAjax(Estate_ProjectViewModel model)
        {
            try
            {
                JsonModelReturnViewEstate_Project json = new JsonModelReturnViewEstate_Project();

                var Estate_ProjectTask = await _realestateProjectRepository.Update(model);

                if (Estate_ProjectTask)
                {
                    json.Estate_Project = await _realestateProjectRepository.GetById(model.ItemId);
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
        private void LoadData()
        {
            ViewBag.Provinces = new SelectList(_provinceRepository.GetAll(false).OrderBy(x => x.Name), "ItemId", "Name", null);
            ViewBag.Districts = new SelectList(_districtRepository.GetAll(false).OrderBy(x => x.Name), "ItemId", "Name", null);
            ViewBag.Estate_Investors = new SelectList(_estate_InvestorRepository.GetAll(false), "ItemId", "Name", null);
        }

        [HttpPost]
        public ActionResult GetAllProjectByDistrictId(long districtId)
        {
            var model = _realestateProjectRepository.GetAllProjectsByDistrict(districtId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
