
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
    public class SaleUnitController : Controller
    {
        //
        // GET: /DonVi/
        private ISaleUnitRepository _ISaleUnitRepository;
        private const int pageSize = 30;
        public SaleUnitController(ISaleUnitRepository donViBanRepository)
        {

            _ISaleUnitRepository = donViBanRepository;
         
        }
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            List<SaleUnit> ListDonVi = new List<SaleUnit>();
            //ViewBag.Vendor = _IvendorRepository.GetAll();
            ListDonVi = _ISaleUnitRepository.GetAll();

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
        public ActionResult Create(SaleUnit collection)
        {
            try
            {
                // TODO: Add insert logic here
                    long rs = _ISaleUnitRepository.Insert(collection);
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
            SaleUnit model = new SaleUnit();
            model = _ISaleUnitRepository.GetById(id);

            return View(model);
        }

        //
        // POST: /DonVi/Edit/5
        [HttpPost]
        public ActionResult Edit(SaleUnit collection)
        {
            try
            {
                // TODO: Add update logic here
                _ISaleUnitRepository.Edit(collection);
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
            _ISaleUnitRepository.Delete(id);
            return RedirectToAction("Index");
        }
       
        //
        // POST: /DonVi/Delete/5
      
    }
}
