using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface ITownRepository
    {
        bool Edit(Town model);
        long Insert(Town model);
         List<Town> GetAll();
         Town GetById(long keyword);
         List<Town> GetAll(bool isDelete);
        List<Town> GetAllByEstateProjectId(bool isDelete, long estateProjectId);
         bool Delete(long id, bool IsDelete);
         bool CheckExit(string kyHieu);

        IEnumerable<TownViewModel> GetAllTownsByProject(long projectId, bool isDelete);

    }
}