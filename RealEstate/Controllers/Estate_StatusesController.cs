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
    public class Estate_StatusesController : Controller
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

        public Estate_StatusesController(IEstateRepository _IEstateRepository,
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
        public async Task<ActionResult> Index(int? page, string name = null)
        {
            int pageNum = (page ?? 1);
            ViewBag.Title = "Manage Estate_Statuses";
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

            List<Estate_StatusViewModel> model = new List<Estate_StatusViewModel>();
            model = await _realestateStatusRepository.GetList();
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

        // GET: Estate_Statuses/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estate_StatusViewModel model = await _realestateStatusRepository.GetById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Estate_Statuses/Create
        public ActionResult Create()
        {
            LoadData();
            return View();
        }
        // POST: Estate_Statuses/Create/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ItemId,Name,Content")] Estate_StatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                model.Modified = now;
                model.IsDelete = false;
                await _realestateStatusRepository.Create(model);
                return RedirectToAction("Index");
            }
            LoadData();
            return View(model);
        }
        // GET: Estate_Statuses/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoadData();
            Estate_StatusViewModel  model = await _realestateStatusRepository.GetById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Estate_Statuses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ItemId,Name,Content")] Estate_StatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                model.Modified = now;
                await _realestateStatusRepository.Update(model);
                return RedirectToAction("Index");
            }
            LoadData();
            return View(model);
        }      

        [HttpPost, ValidateInput(false)]
        public async Task<JsonResult> UpdateIsDelete(long itemId)
        {
            string message = string.Empty;
            JsonModelReturnViewEstate_Status json = new JsonModelReturnViewEstate_Status();                    
            try
            {
                var task = await _realestateStatusRepository.UpdateIsDelete(itemId, true);
                if (task == true)
                {
                    json.Estate_Status = await _realestateStatusRepository.GetById(itemId);
                    json.messages = "Update successfully.";
                    json.isError = false;
                    return Json(json,JsonRequestBehavior.AllowGet);
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
            JsonModelReturnViewEstate_Status json = new JsonModelReturnViewEstate_Status();
            try
            {
                var task = await _realestateStatusRepository.UpdateIsDelete(itemId, false);
                if (task == true)
                {
                    json.Estate_Status = await _realestateStatusRepository.GetById(itemId);
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
        public  ActionResult CreateAjax()
        {
            LoadData();
            return  PartialView();
        }

        // POST: Admin/Create
        [HttpPost]
        public async Task<ActionResult> CreateAjax( Estate_StatusViewModel model)
        {
            try
            {

                JsonModelReturnViewEstate_Status json = new JsonModelReturnViewEstate_Status();
                model.IsDelete = false;
                var Estate_StatusTask = await _realestateStatusRepository.Create(model);
                if (Estate_StatusTask)
                {
                    json.Estate_Status = model;
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
            var my = await _realestateStatusRepository.GetById(id);
            LoadData();
            return PartialView(my);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public async Task<JsonResult> EditAjax(Estate_StatusViewModel model)
        {
            try
            {
                JsonModelReturnViewEstate_Status json = new JsonModelReturnViewEstate_Status();
               
                var Estate_StatusTask = await _realestateStatusRepository.Update(model);

                if (Estate_StatusTask)
                {
                    json.Estate_Status = await _realestateStatusRepository.GetById(model.ItemId);
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
            ViewBag.Estate_Investors = new SelectList(_estate_InvestorRepository.GetAll(false), "ItemId", "Name", null);
        }

    }
}
