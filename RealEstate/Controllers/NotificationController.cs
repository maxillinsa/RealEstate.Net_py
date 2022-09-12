
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Models;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using PagedList;
using CustomRoles;
namespace RealEstate.Controllers
{
    [CustomAuthorize(Roles = "SuperAdmin,Admin,Staff")]
    public class NotificationController : Controller
    {
        //
        // GET: /DonVi/
        private INotificationRepository _INotificationRepository;
        private ICommentRepository _commentRepository;
        private const int pageSize = 30;
        public NotificationController(INotificationRepository thongBaoRepository,ICommentRepository commentRepository)
        {

            _INotificationRepository = thongBaoRepository;
            _commentRepository = commentRepository;
         
        }
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            List<Notification> ListNotification = new List<Notification>();
            ListNotification = _INotificationRepository.GetAll();
            return View(ListNotification.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Detail(long id)
        {
            Tb_Comment model = new Tb_Comment();

            Notification tb = new Notification();
            tb = _INotificationRepository.GetById(id);

            if (tb != null)
                model.thongBao = tb;
            model.lstComments = _commentRepository.GetByNotificationId(id);
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Detail(long thongBaoId, string content)
        {
            Comment cm = new Comment();
            cm.CreateDate = DateTime.Now;

            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            cm.CreateById = manv;
            cm.Contents = content;
            cm.IsDelete = false;
            cm.NotificationId = thongBaoId;
            _commentRepository.Insert(cm);
            return RedirectToAction("Detail", new { id = thongBaoId });
        }

        //
        // GET: /DonVi/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DonVi/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Notification collection)
        {
            try
            {
                string accountId;
                if (HttpContext.Session["bds_Acc_id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                accountId = HttpContext.Session["bds_Acc_id"].ToString();
                // TODO: Add insert logic here
                collection.CreateDate = DateTime.Now;
                collection.AccountId = Convert.ToInt64(accountId);
                    long rs = _INotificationRepository.Insert(collection);
                    return RedirectToAction("Index");
               
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /DonVi/Edit/5
        public ActionResult Edit(int id)
        {
            Notification model = new Notification();
            model = _INotificationRepository.GetById(id);

            return View(model);
        }

        //
        // POST: /DonVi/Edit/5
      [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Notification collection)
        {
            try
            {
                string accountId;
                if (HttpContext.Session["bds_Acc_id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                accountId = HttpContext.Session["bds_Acc_id"].ToString();
                // TODO: Add insert logic her
                collection.AccountId = Convert.ToInt64(accountId);
                // TODO: Add update logic here
                _INotificationRepository.Edit(collection);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /DonVi/Delete/5
        public ActionResult Delete(int id,bool isDelete)
        {
            _INotificationRepository.Delete(id, isDelete);
            return RedirectToAction("Index");
        }
       
        //
        // POST: /DonVi/Delete/5
      
    }
}
