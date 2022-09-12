using System;
using System.Collections.Generic;
using System.Linq;
//using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
//using WebMatrix.WebData;
using RealEstate.Models;
using RealEstate.DAL.Repository;
using RealEstate.DAL.IRepository;
using Newtonsoft.Json;
using CustomRoles;
using RealEstate.Common;
using PagedList;
using RealEstate.Models.ViewModels;
using System.IO;

namespace RealEstate.Controllers
{

    public class AccountController : Controller
    {
       
        private const int pageSize = 20;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountRolesRepository _accountRolesRepository;
        private readonly IDepartmentsRepository _departmentRepository;
        private readonly ILoginLogRepository _loginLogRepository;
        private IEstateRepository _EstateRepository;
        public AccountController(IDepartmentsRepository departmentsRepository, IAccountRepository accountRepository, IAccountRolesRepository accountRolesRepository,ILoginLogRepository loginLogRepository, IEstateRepository estateRepository)
        {
            this._loginLogRepository = loginLogRepository;
            this._accountRepository = accountRepository;
            this._accountRolesRepository = accountRolesRepository;
            this._departmentRepository = departmentsRepository;
            this._EstateRepository = estateRepository;
        }
        private string ImageUploadsFolder
        {
            get
            {
                return Path.Combine(Server.MapPath("/Media/Images/account"));
            }
        }
        [CustomAuthorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            IList<Account> lstAccount = new List<Account>();
            lstAccount = _accountRepository.GetAll();
            return View(lstAccount.ToPagedList(pageNum, 100));
        }

        [CustomAuthorize(Roles = "SuperAdmin,Admin")]
        public ActionResult ListingTransfer()
        {
            ViewBag.flag = 0;
            IList<Account> lstAcc = new List<Account>();
            lstAcc = _accountRepository.GetAll();
            ViewBag.account = lstAcc;
            ViewBag.total = lstAcc.Count;
            List<Estate> lstEstates = new List<Estate>();
            lstEstates = _EstateRepository.GetAll();

            List<Account> lstAccounts = new List<Account>();

            foreach (var item in lstEstates)
            {
                if (item.AccountId != null)
                {
                    if (lstAccounts.Count == 0)
                    {
                        lstAccounts.Add(item.Account);
                    }
                    else
                    {
                        var acc = lstAccounts.FirstOrDefault(x => x.AccountId == item.AccountId);
                        if (acc == null)
                        {
                            lstAccounts.Add(item.Account);
                        }
                    }
                }

            }
            ViewBag.total1 = lstAccounts.Count;
            ViewBag.account1 = lstAccounts;
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ListingTransfer(int accountIdChuyen, int accountIdNhan)
        {

            if (accountIdChuyen != 0 && accountIdNhan != 0)
            {
                if (_accountRepository.ListingTransferAccountToAccount(accountIdChuyen, accountIdNhan))
                {
                    ViewBag.flag = 1;
                    return View();
                }
            }
            ViewBag.flag = 0;
            return View();
        }
        [CustomAuthorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Create()
        {
            LoadData();
            return View();
        }
        [CustomAuthorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Account account)
        {
            account.IsDelete = false;
            account.CreateDate = DateTime.Now;
            account.Password = Security.MD5Encrypt(account.Password);
            account.UserName = account.Email;
            if (_accountRepository.CheckExit(account))
            {
                LoadData();
                ViewBag.error = "This email already existed";
                return View(account);
            }
            else if (_accountRepository.Create(account))
            {
                var ac = new AccountRole
                {
                    AccountId = account.AccountId,
                    RoleId = 2,

                };
                _accountRolesRepository.CreateRoleDefault(ac);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Create failed, please try again.";
                LoadData();
                return View(account);
            }
        }
        [CustomAuthorize(Roles = "SuperAdmin,Admin,Staff")]
        public ActionResult Edit(long id)
        {

            Account account = new Account();
            account = _accountRepository.GetById(id);
            LoadData();
            return View(account);
        }


        [CustomAuthorize(Roles = "SuperAdmin,Admin,Staff")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Account account)
        {
            if (_accountRepository.CheckExit(account))
            {
                LoadData();
                ViewBag.error = "This email already existed";
                return View(account);
            }
            if (_accountRepository.Edit(account))
            {
                return RedirectToAction("Index");
            }
            else
            {
                LoadData();
                ViewBag.error = "Create failed, please try again.";
                return View(account);
            }
        }
        public CaptchaImageResult ShowCaptchaImage()
        {
            return new CaptchaImageResult();
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ShowCaptchaText = 0;
            ViewBag.ReturnUrl = returnUrl;
            FormsAuthentication.SignOut();
            HttpContext.Session["captchastring"] = null;
            if (HttpContext.Session["bds_Acc_id"] != null)
            {
                HttpContext.Session["bds_Acc_id"] = null;
            }
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost, ValidateInput(false)]
        public ActionResult Login(LoginModel colection, string captchaText, int showCaptchaText)
        {


            ViewBag.ShowCaptchaText = showCaptchaText;
            if (HttpContext.Session["captchastring"] != null)
            {
                string sessionCapt = HttpContext.Session["captchastring"].ToString();
                if (captchaText.ToUpper() != sessionCapt)
                {
                    ViewBag.Error = "Wrong capcha. ";
                    ViewBag.CaptchaText = "Input capcha above.";
                    HttpContext.Session["captchastring"] = null;
                    return View(colection);
                }
            }
            LoginModel lg = new LoginModel();
            lg = _accountRepository.GetLoginByEmail(colection.Email);
            if (lg.AccountId == 0)
            {
                ViewBag.Error = "Wrong email, please try again. ";
                ViewBag.ShowCaptchaText = 1;
                HttpContext.Session["captchastring"] = null;
                return View();
            }
            else
            {

                string password = RealEstate.Common.Security.MD5Decrypt(lg.Password);
                if (colection.Password != password)
                {
                    ViewBag.Error = "Wrong password, please try again.";
                    ViewBag.ShowCaptchaText = 1;
                    HttpContext.Session["captchastring"] = null;
                    return View();
                }
                else
                {
                    List<string> roles = new List<string>();
                    roles = _accountRolesRepository.GetRoleNameByAccountId(lg.AccountId);
                    if (roles != null)
                    {
                        CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                        serializeModel.AccountId = lg.AccountId;
                        serializeModel.FirstName = lg.FirstName;
                        serializeModel.LastName = lg.LastName;
                        serializeModel.roles = roles;
                        string userData = JsonConvert.SerializeObject(serializeModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                 1,
                                lg.Email,
                                 DateTime.Now,
                                 DateTime.Now.AddMinutes(15),
                                 false,
                                 userData);

                        string encTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        Response.Cookies.Add(faCookie);
                        HttpContext.Session["bds_Acc_id"] = lg.AccountId;

                        string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        if (string.IsNullOrEmpty(ip))
                        {
                            ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        }
                        string browserName = System.Web.HttpContext.Current.Request.Browser.Platform + System.Web.HttpContext.Current.Request.Browser.Type + System.Web.HttpContext.Current.Request.Browser.Version;
                        _loginLogRepository.Insert(new LoginLog
                        {
                            AccountId = lg.AccountId,
                            CreateDate = DateTime.Now,
                            IPAddress = ip + browserName,
                            IsDelete = false
                        });
                        _accountRepository.UpdateLastSignIn(lg.AccountId);
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("Index", "Home");
                        }

                        if (roles.Contains("Staff"))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        if (roles.Contains("Editor")) // Editor là quyền quản trị Website.
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        return RedirectToAction("LogOut", "Account");
                    }
                    return RedirectToAction("LogOut", "Account");
                }
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {

            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ForgotPassword(ForgotPasswordModel colection)
        {

            string senderAddress = "admin@www.tinraonhanh.com";
            string reciever = "quanghoahcm@gmail.com";
            return View(colection);
        }
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            if (HttpContext.Session["bds_Acc_id"] != null)
            {
                HttpContext.Session["bds_Acc_id"] = null;
            }
            return RedirectToAction("Login", "Account", null);
        }

        [ChildActionOnly]
        public ActionResult UserAccountPartial()
        {
            IList<Estate> lsEstates = new List<Estate>();
            string accId;
            Account account = new Account();
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                    accId = (serializeModel.AccountId).ToString();
                    account.FirstName = serializeModel.FirstName;
                    account.LastName = serializeModel.LastName;
                    account.Email = serializeModel.Email;
                    HttpContext.Session["bds_Acc_id"] = accId;
                    return PartialView(account);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                accId = HttpContext.Session["bds_Acc_id"].ToString();
                account = _accountRepository.GetById(Int64.Parse(accId));
                lsEstates = _EstateRepository.GetAllByAccountID(Int32.Parse(accId));
                ViewBag.totalByAccount = lsEstates.Count;
                return PartialView(account);
            }

        }
        [CustomAuthorize(Roles = "SuperAdmin,Admin,Staff")]
        public ActionResult ChangePassword(int? id)
        {
            ViewBag.id = null;
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id != null)
                ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(LocalPasswordModel collection, int? id)
        {
            if (id != null)
            {
                Account account = new Account();
                account = _accountRepository.GetById((int)id);
                account.Password = Security.MD5Encrypt(collection.NewPassword);
                _accountRepository.ChangePassword(account);
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                long accountId = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
                Account account = new Account();
                account = _accountRepository.GetById(accountId);
                if (collection.OldPassword != Security.MD5Decrypt(account.Password))
                {
                    ModelState.AddModelError("OldPassword", "Current password is not correct!");
                    return View();
                }
                account.Password = Security.MD5Encrypt(collection.NewPassword);
                _accountRepository.ChangePassword(account);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        [CustomAuthorize(Roles = "SuperAdmin,Admin,Staff")]
        public ActionResult Profile(long id)
        {
            ViewBag.id = null;
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            LoadData();
            ViewBag.id = id;
            ProfileViewModel model = new ProfileViewModel();
            model = _accountRepository.GetProfileViewById(id);
            model.TotalEstates = _EstateRepository.GetAllByAccountID(id).Count;

            return View(model);
        }
        [HttpPost]
        public ActionResult Profile(ProfileViewModel collection, HttpPostedFileBase txtFile)
        {
         
            if (ModelState.IsValid)
            {

                long accountId = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
                Account account = new Account();
                account = _accountRepository.GetById(accountId);


                if (txtFile != null)
                {
                    if (txtFile.ContentLength / 1024 > 4500)
                    {
                        ModelState.AddModelError("txtFile", "Image maximum 4.5MB");
                    }
                    string server = string.Empty;
                    server = ImageUploadsFolder;
                    String nameImage = CreateNewName(Path.GetExtension(txtFile.FileName));
                    var fullName = Path.Combine(server, nameImage);
                    ImageHelper.UploadImage(txtFile, fullName);
                    collection.Image = nameImage;
                }
                else
                {
                    ModelState.AddModelError("txtFile", "Image 1 is empty");
                    LoadData();
                    return View(collection);
                }

                var model = new Account {
                    AccountId = Convert.ToInt32(collection.AccountId),
                    Detail = collection.Detail,
                    FirstName = collection.FirstName,
                    UserName = collection.UserName,
                    Email = collection.Email,
                    Image = collection.Image,
                    Mobile = collection.Mobile                    
                };
                _accountRepository.Edit(model);
                LoadData();
                return RedirectToAction("Profile");
            }
            else
            {
                return View();
            }
        }

        private string CreateNewName(string str)
        {
            return DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString() + str;
        }
        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Delete(long id, bool isDelete)
        {

            _accountRepository.Delete(id, isDelete);
            return RedirectToAction("Index");
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);

            }
        }
        public void LoadData(object obj = null)
        {
            var lst = _departmentRepository.GetAll(false);
            ViewBag.DepartmentName = new SelectList(lst, "DepartmentId", "DepartmentName", obj);
        }

        #endregion
    }


}
