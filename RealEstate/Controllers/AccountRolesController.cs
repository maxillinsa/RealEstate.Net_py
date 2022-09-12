using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Models;
using CustomRoles;
using RealEstate.DAL.Repository;
using RealEstate.DAL.IRepository;
using RealEstate.Common;

namespace RealEstate.Controllers
{
    [CustomAuthorize(Roles = "SuperAdmin,Admin")]
    public class AccountRolesController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountRolesRepository _accountRolesRepository;
        private readonly IDepartmentsRepository _departmentRepository;
        private readonly ILoginLogRepository _loginLogRepository;
        private IEstateRepository _EstateRepository;
        private IRolesRepository _rolesRepository;
        private const int pageSize = 20;
        public AccountRolesController(IRolesRepository rolesRepository,IDepartmentsRepository departmentsRepository, IAccountRepository accountRepository, IAccountRolesRepository accountRolesRepository, ILoginLogRepository loginLogRepository, IEstateRepository estateRepository)
        {
            this._loginLogRepository = loginLogRepository;
            this._accountRepository = accountRepository;
            this._accountRolesRepository = accountRolesRepository;
            this._departmentRepository = departmentsRepository;
            this._EstateRepository = estateRepository;
            _rolesRepository = rolesRepository;
        }       
        //
        // GET: /AccountRoles/

        public ActionResult Index()
        {
            IList<AccountRole> accountroles = new List<AccountRole>();
            accountroles = _accountRolesRepository.GetAll().OrderBy(x=> x.RoleId).ToList();
            return View(accountroles);
        }

        //
        // GET: /AccountRoles/Details/5

      
        //
        // GET: /AccountRoles/Create

        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(_accountRepository.GetAll(false), "AccountId", "UserName");
            ViewBag.RoleId = new SelectList(_rolesRepository.GetAll(false), "RoleId", "RoleName");
            return View();
        }

        //
        // POST: /AccountRoles/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountRole accountrole)
        {
            ViewBag.error = null;
            if (ModelState.IsValid)
            {
               
                if(_accountRolesRepository.CheckExit(accountrole))
                {
                    ViewBag.AccountId = new SelectList(_accountRepository.GetAll(false), "AccountId", "UserName");
                    ViewBag.RoleId = new SelectList(_rolesRepository.GetAll(false), "RoleId", "RoleName");
                    ViewBag.error = "Role existed for selected account";
                    return View(accountrole);
                }
                else
                {
                    accountrole.CreateDate = DateTime.Now;
                    accountrole.IsDelete = false;
                    
                    _accountRolesRepository.Create(accountrole);
                return RedirectToAction("Index");
                }
            }
            ViewBag.AccountId = new SelectList(_accountRepository.GetAll(false), "AccountId", "UserName");
            ViewBag.RoleId = new SelectList(_rolesRepository.GetAll(false), "RoleId", "RoleName");
            ViewBag.error = "Create failed, please try again.";
            return View(accountrole);
        }
        public ActionResult CreateByAccount(long id)
        {
            ViewBag.id = id;
            ViewBag.RoleId = new SelectList(_rolesRepository.GetAll(false), "RoleId", "RoleName");
            ViewBag.lstAccountRoleByAccountId = _accountRolesRepository.GetAll().Where(x => x.AccountId == id).ToList();
            return View();
        }

        //
        // POST: /AccountRoles/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateByAccount(AccountRole accountrole)
        {
            ViewBag.error = null;
            if (ModelState.IsValid)
            {
                if (_accountRolesRepository.CheckExit(accountrole))
                {
                    ViewBag.RoleId = new SelectList(_rolesRepository.GetAll(false), "RoleId", "RoleName");
                    ViewBag.error = "Role existed for selected account";
                    ViewBag.id = accountrole.AccountId;
                    ViewBag.lstAccountRoleByAccountId = _accountRolesRepository.GetAll().Where(x => x.AccountId == accountrole.AccountId).ToList();
                    return View(accountrole);
                }
                else
                {
                    _accountRolesRepository.Create(accountrole);
                    return RedirectToAction("CreateByAccount", new { id  = accountrole.AccountId});
                }
            }

            ViewBag.AccountId = new SelectList(_accountRepository.GetAll(false), "AccountId", "UserName");
            ViewBag.RoleId = new SelectList(_rolesRepository.GetAll(false), "RoleId", "RoleName");
            ViewBag.id = accountrole.AccountId;
            return View(accountrole);
        }
        //
        // GET: /AccountRoles/Edit/5

        public ActionResult Edit(long id = 0)
        {
            AccountRole accountrole = _accountRolesRepository.GetById(id);
            if (accountrole == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(_accountRepository.GetAll(false), "AccountId", "UserName");
            ViewBag.RoleId = new SelectList(_rolesRepository.GetAll(false), "RoleId", "RoleName");
            return View(accountrole);
        }

        //
        // POST: /AccountRoles/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountRole accountrole)
        {
            ViewBag.error = null;
            if (ModelState.IsValid)
            {
                if (_accountRolesRepository.CheckExit(accountrole))
                {
                    ViewBag.AccountId = new SelectList(_accountRepository.GetAll(false), "AccountId", "UserName");
                    ViewBag.RoleId = new SelectList(_rolesRepository.GetAll(false), "RoleId", "RoleName");
                    ViewBag.error = "Role existed for selected account";
                    return View(accountrole);
                }
                else
                {
                    _accountRolesRepository.Edit(accountrole);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AccountId = new SelectList(_accountRepository.GetAll(false), "AccountId", "UserName");
            ViewBag.RoleId = new SelectList(_rolesRepository.GetAll(false), "RoleId", "RoleName");
            return View(accountrole);
        }

        public ActionResult Delete(long id)
        {
            _accountRolesRepository.Delete(id, true);
            return RedirectToAction("Index");
        }
        public ActionResult UnDelete(long id)
        {
            _accountRolesRepository.Delete(id, false);
            return RedirectToAction("Index");
        }
    }
}