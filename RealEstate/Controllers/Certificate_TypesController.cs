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
    public class Certificate_TypesController : Controller
    {
        private const int pageSize = 35;
        private IEstateRepository _IEstateRepository;
        private IEstate_InvestorRepository  _estate_InvestorRepository;
        private IEstate_ProjectRepository _realestateProjectRepository;
        private IEstate_StatusRepository _realestateStatusRepository;
        private ITownRepository _ITownRepository;
        private ISaleUnitRepository _ISaleUnitRepository;
        private IRentUnitRepository _IRentUnitRepository;
        private IAccountRepository _accountRepository;
        private ICommentRepository _commentRepository;
        private IEstate_TypesRepository _Estate_TypesRepository;
        private IEstate_GroupRepository _estate_GroupRepository;
        private IEstate_StatusRepository _estate_StatusRepository;
        private IProvinceRepository _provinceRepository;
        private IDistrictRepository _districtRepository;
        private IWardRepository _wardRepository;
        private ICertificate_TypeRepository _certificate_TypeRepository;


        public Certificate_TypesController(IEstate_InvestorRepository estate_InvestorRepository, IEstateRepository _IEstateRepository,
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
         ICertificate_TypeRepository _certificate_TypeRepository)
        {
            this._ITownRepository = _ITownRepository;
            this._ISaleUnitRepository = _ISaleUnitRepository;
            this._IRentUnitRepository = _IRentUnitRepository;
            this._IEstateRepository = _IEstateRepository;
            this._commentRepository = _commentRepository;
            this._estate_InvestorRepository = estate_InvestorRepository;
            this._realestateProjectRepository = _realestateProjectRepository;
            this._realestateStatusRepository = _realestateStatusRepository;
            this._accountRepository = _accountRepository;
            this._Estate_TypesRepository = _Estate_TypesRepository;
            this._estate_GroupRepository = _estate_GroupRepository;
            this._estate_StatusRepository = _estate_StatusRepository;
            this._provinceRepository = _provinceRepository;
            this._districtRepository = _districtRepository;
            this._wardRepository = _wardRepository;
            this._certificate_TypeRepository = _certificate_TypeRepository;
        }
        public async Task<ActionResult> Index(int? page, string name = null)
        {
            int pageNum = (page ?? 1);
            ViewBag.Title = "Manage Certificate_Types";
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

            List<Certificate_TypeViewModel> model = new List<Certificate_TypeViewModel>();
            model = await _certificate_TypeRepository.GetList();
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

        // GET: Certificate_Types/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate_TypeViewModel model = await _certificate_TypeRepository.GetById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Certificate_Types/Create
        public ActionResult Create()
        {
            LoadData();
            return View();
        }
        // POST: Certificate_Types/Create/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ItemId,Name,Content")] Certificate_TypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                model.Modified = now;
                model.IsDelete = false;
                await _certificate_TypeRepository.Create(model);
                return RedirectToAction("Index");
            }
            LoadData();
            return View(model);
        }
        // GET: Certificate_Types/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoadData();
            Certificate_TypeViewModel  model = await _certificate_TypeRepository.GetById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Certificate_Types/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ItemId,Name,Content")] Certificate_TypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                model.Modified = now;
                await _certificate_TypeRepository.Update(model);
                return RedirectToAction("Index");
            }
            LoadData();
            return View(model);
        }      

        [HttpPost, ValidateInput(false)]
        public async Task<JsonResult> UpdateIsDelete(long itemId)
        {
            string message = string.Empty;
            JsonModelReturnViewCertificate_Type json = new JsonModelReturnViewCertificate_Type();                    
            try
            {
                var task = await _certificate_TypeRepository.UpdateIsDelete(itemId, true);
                if (task == true)
                {
                    json.Certificate_Type = await _certificate_TypeRepository.GetById(itemId);
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
            JsonModelReturnViewCertificate_Type json = new JsonModelReturnViewCertificate_Type();
            try
            {
                var task = await _certificate_TypeRepository.UpdateIsDelete(itemId, false);
                if (task == true)
                {
                    json.Certificate_Type = await _certificate_TypeRepository.GetById(itemId);
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
        public async Task<ActionResult> CreateAjax( Certificate_TypeViewModel model)
        {
            try
            {

                JsonModelReturnViewCertificate_Type json = new JsonModelReturnViewCertificate_Type();
                model.IsDelete = false;
                var Certificate_TypeTask = await _certificate_TypeRepository.Create(model);
                if (Certificate_TypeTask)
                {
                    json.Certificate_Type = model;
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
            var my = await _certificate_TypeRepository.GetById(id);
            LoadData();
            return PartialView(my);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public async Task<JsonResult> EditAjax(Certificate_TypeViewModel model)
        {
            try
            {
                JsonModelReturnViewCertificate_Type json = new JsonModelReturnViewCertificate_Type();
               
                var Certificate_TypeTask = await _certificate_TypeRepository.Update(model);

                if (Certificate_TypeTask)
                {
                    json.Certificate_Type = await _certificate_TypeRepository.GetById(model.ItemId);
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
