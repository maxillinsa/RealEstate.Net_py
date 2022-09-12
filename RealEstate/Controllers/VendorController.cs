using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Models;
using CustomRoles;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using PagedList;
namespace RealEstate.Controllers
{
     [CustomAuthorize(Roles = "SuperAdmin,Admin")]
    public class VendorController : Controller
    {
         private IVendorRepository _VendorRepsitory;
        private const int pageSize = 30;
         public VendorController(IVendorRepository vendorRepository)
         {
             _VendorRepsitory = vendorRepository;
         }

        //
        // GET: /Vendor/

         public ActionResult Index(int? page)
        {
            List<Vendor> ListVendor = _VendorRepsitory.GetAll();
            int pageNum = (page ?? 1);
            ViewBag.page = pageNum;
            return View(ListVendor.ToPagedList(pageNum, pageSize));
        }
        //
        // GET: /Vendor/Create

        public ActionResult Create(int? page)
        {
            ViewBag.page = page;
            return View();
        }

        //
        // POST: /Vendor/Create

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? page,Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                vendor.CreateDate = DateTime.Now;
                vendor.IsDelete = false;
                long rs = _VendorRepsitory.Insert(vendor);
                ViewBag.page = page;
                return RedirectToAction("Index", new {page= page });
            }

            return View(vendor);
        }

        //
        // GET: /Vendor/Edit/5

        public ActionResult Edit(int? page,long id)
        {
            ViewBag.page = (page ?? 1);
            Vendor model = _VendorRepsitory.GetById(id);
            return View(model);
        }

        //
        // POST: /Vendor/Edit/5

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? page, Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                int pageNum = (page ?? 1);
                ViewBag.page = pageNum;
                vendor.IsDelete = false;
                _VendorRepsitory.Edit(vendor);
                return RedirectToAction("Index", new { page = pageNum });
            }
            return View(vendor);
        }

        //
        // GET: /Vendor/Delete/5

        public ActionResult Delete(long id)
        {
            _VendorRepsitory.Delete(id, true);
            return RedirectToAction("Index");
        }
        public ActionResult UnDelete(long id)
        {
            _VendorRepsitory.Delete(id, false);
            return RedirectToAction("Index");
        }

    }
}