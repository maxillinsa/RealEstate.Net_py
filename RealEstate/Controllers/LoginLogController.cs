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
using PagedList;

namespace RealEstate.Controllers
{
     [CustomAuthorize(Roles = "SuperAdmin,Admin")]
    public class LoginLogController : Controller
    {
         private const int pageSize = 50;
         private readonly ILoginLogRepository _loginLogRepository;
        //
        // GET: /LoginLog/
         public LoginLogController(ILoginLogRepository loginLogRepository)
         {
             this._loginLogRepository = loginLogRepository;
         }
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            IList<LoginLog> lst = new List<LoginLog>();
            lst = _loginLogRepository.GetAll();
            return View(lst.ToPagedList(pageNum,pageSize));
        }

      
    }
}