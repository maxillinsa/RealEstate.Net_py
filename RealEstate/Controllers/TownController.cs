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
    public class TownController : Controller
    {
        //
        // GET: /Town/
        private ITownRepository _ITownRepository;
        private IEstate_ProjectRepository _realestateProjectRepository;
        private const int pageSize = 30;
        public TownController(ITownRepository khuPhoRepository, IEstate_ProjectRepository estate_ProjectRepository)
        {

            _ITownRepository = khuPhoRepository;
            _realestateProjectRepository = estate_ProjectRepository;
        }
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            List<Town> ListTown = new List<Town>();
            //ViewBag.Vendor = _IvendorRepository.GetAll();
            ListTown = _ITownRepository.GetAll();
            return View(ListTown.ToPagedList(pageNum, pageSize));
        }


        //
        // GET: /Town/Create
        public ActionResult Create()
        {
            loadData();
            return View();
        }

        //
        // POST: /Town/Create
        [HttpPost]
        public ActionResult Create(Town collection)
        {
            try
            {
                // TODO: Add insert logic here

                collection.CreateDate = DateTime.Now;
                collection.IsDelete = false;
                long rs = _ITownRepository.Insert(collection);
                return RedirectToAction("Index");

            }
            catch
            {
                loadData();
                return View();
            }
        }

        //
        // GET: /Town/Edit/5
        [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ActionResult Edit(int id)
        {
            loadData();
            Town model = new Town();
            model = _ITownRepository.GetById(id);
            
            return View(model);
        }

        //
        // POST: /Town/Edit/5
        [HttpPost]
        public ActionResult Edit(Town collection)
        {
            try
            {
                // TODO: Add update logic here
                _ITownRepository.Edit(collection);
                return RedirectToAction("Index");
            }
            catch
            {
                loadData();
                return View();
            }
        }

        //
        // GET: /Town/Delete/5
        public ActionResult Delete(int id)
        {
            _ITownRepository.Delete(id, true);
            return RedirectToAction("Index");
        }
        public ActionResult UnDelete(int id)
        {
            _ITownRepository.Delete(id, false);
            return RedirectToAction("Index");
        }
        //
        // POST: /Town/Delete/5
        private void loadData()
        {
            ViewBag.Projects = new SelectList(_realestateProjectRepository.GetAll(false).OrderBy(x => x.Name).ToList(), "ItemId", "Name", null);
        }
    }
}
