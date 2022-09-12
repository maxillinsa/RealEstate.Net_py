using MvcPaging;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public class StreetsController : Controller
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
        private IEstate_GroupRepository _estate_GroupRepository;
        private IEstate_StatusRepository _estate_StatusRepository;
        private IProvinceRepository _provinceRepository;
        private IStreetRepository _StreetRepository;
        private IWardRepository _wardRepository;
        private ICertificate_TypeRepository _certificate_TypeRepository;
        private ICountryRepository _countryRepository;
        private IDistrictRepository districtRepository;
        public StreetsController(IEstateRepository _IEstateRepository,
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
         IStreetRepository _StreetRepository,
         IWardRepository _wardRepository,
         ICertificate_TypeRepository _certificate_TypeRepository,
         ICountryRepository countryRepository,
         IDistrictRepository districtRepository)
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
            this._estate_GroupRepository = _estate_GroupRepository;
            this._estate_StatusRepository = _estate_StatusRepository;
            this._provinceRepository = _provinceRepository;
            this._StreetRepository = _StreetRepository;
            this._wardRepository = _wardRepository;
            this._certificate_TypeRepository = _certificate_TypeRepository;
            this._countryRepository = countryRepository;
            this.districtRepository = districtRepository;
        }
        #endregion
        public async Task<ActionResult> Index(int? page, long districtId = 0, string name = null)
        {
            LoadData();
            int pageNum = (page ?? 1);
            ViewBag.Title = "Manage Streets";
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

            List<StreetViewModel> model = new List<StreetViewModel>();
            model = await _StreetRepository.GetList();
            if (string.IsNullOrWhiteSpace(name))
            {
                model = model.ToList();
            }
            else
            {
                model = model.Where(x => x.Name != null).ToList();
                model = model.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
            }
            if(districtId != 0)
            {
              model =  model.Where(x => x.DistrictId == districtId).ToList();
            }
            ViewData["name"] = name;
            if (Request.IsAjaxRequest())
                return PartialView("AjaxList", model.ToPagedList(pageNum, pageSize));
            return View(model.ToPagedList(pageNum, pageSize));

        }
        [HttpPost]
        public ActionResult GetStreetByDistrictId(long districtId)
        {
            var model =  _StreetRepository.GetAllStreetsByDistrictId(districtId, false);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        // GET: Streets/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StreetViewModel model = await _StreetRepository.GetById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }      
        
        [HttpPost, ValidateInput(false)]
        public async Task<JsonResult> UpdateIsDelete(long itemId)
        {
            string message = string.Empty;
            JsonModelReturnViewStreet json = new JsonModelReturnViewStreet();
            try
            {
                var task = await _StreetRepository.UpdateIsDelete(itemId, true);
                if (task == true)
                {
                    json.Street = await _StreetRepository.GetById(itemId);
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
            JsonModelReturnViewStreet json = new JsonModelReturnViewStreet();
            try
            {
                var task = await _StreetRepository.UpdateIsDelete(itemId, false);
                if (task == true)
                {
                    json.Street = await _StreetRepository.GetById(itemId);
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
        public async Task<ActionResult> CreateAjax(StreetViewModel model)
        {
            try
            {

                JsonModelReturnViewStreet json = new JsonModelReturnViewStreet();
                var StreetTask = await _StreetRepository.Create(model);
                if (StreetTask)
                {
                    json.Street = model;
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
            var my = await _StreetRepository.GetById(id);
            LoadData();
            return PartialView(my);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public async Task<JsonResult> EditAjax(StreetViewModel model)
        {
            try
            {
                JsonModelReturnViewStreet json = new JsonModelReturnViewStreet();

                var StreetTask = await _StreetRepository.Update(model);

                if (StreetTask)
                {
                    json.Street = await _StreetRepository.GetById(model.StreetId);
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
            ViewBag.Provinces = new SelectList(_provinceRepository.GetAll(false), "ItemId", "Name", null);
            ViewBag.Districts = new SelectList(districtRepository.GetAll(false), "ItemId", "Name", null);

        }

    }
}
