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

namespace RealEstate.Controllers
{
     [CustomAuthorize(Roles = "SuperAdmin,Admin")]
    public class RolesController : Controller
    {
         private readonly IRolesRepository _rolesRepository;

        //
        // GET: /Roles/
         public RolesController(IRolesRepository rolesRepository)
         {
             this._rolesRepository = rolesRepository;
         }
        public ActionResult Index()
        {
            return View(_rolesRepository.GetAll());
        }

        //
        // GET: /Roles/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                role.CreateDate = DateTime.Now;
                _rolesRepository.Insert(role);
                return RedirectToAction("Index");
            }

            return View(role);
        }

        //
        // GET: /Roles/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Role role = _rolesRepository.GetById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Roles/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {

                _rolesRepository.Edit(role);
                return RedirectToAction("Index");
            }
            return View(role);
        }

        //
        // GET: /Roles/Delete/5

     

        //
        // POST: /Roles/Delete/5


      
    }
}