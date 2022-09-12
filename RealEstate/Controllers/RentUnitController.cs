
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
    [CustomAuthorize(Roles = "SuperAdmin,Admin")]
    public class RentUnitController : Controller
    {
        //
        // GET: /DonVi/
        private IRentUnitRepository _IRentUnitRepository;
        private const int pageSize = 30;
        public RentUnitController(IRentUnitRepository donViThueRepository)
        {

            _IRentUnitRepository = donViThueRepository;
         
        }
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            List<RentUnit> ListDonVi = new List<RentUnit>();
            //ViewBag.Vendor = _IvendorRepository.GetAll();
            ListDonVi = _IRentUnitRepository.GetAll();

            return View(ListDonVi.ToPagedList(pageNum, pageSize));
        }


        //
        // GET: /DonVi/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DonVi/Create
        [HttpPost]
        public ActionResult Create(RentUnit collection)
        {
            try
            {
                // TODO: Add insert logic here
                    long rs = _IRentUnitRepository.Insert(collection);
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
            RentUnit model = new RentUnit();
            model = _IRentUnitRepository.GetById(id);

            return View(model);
        }

        //
        // POST: /DonVi/Edit/5
        [HttpPost]
        public ActionResult Edit(RentUnit collection)
        {
            try
            {
                // TODO: Add update logic here
                _IRentUnitRepository.Edit(collection);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /DonVi/Delete/5
        public ActionResult Delete(int id)
        {
            _IRentUnitRepository.Delete(id);
            return RedirectToAction("Index");
        }
       
        //
        // POST: /DonVi/Delete/5
      
    }
}
