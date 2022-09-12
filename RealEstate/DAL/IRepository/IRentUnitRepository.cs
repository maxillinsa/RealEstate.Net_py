using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IRentUnitRepository
    {
        bool Edit(RentUnit model);
        long Insert(RentUnit model);
        List<RentUnit> GetAll();
        List<RentUnit> GetAll(bool isDelete);
        RentUnit GetById(long keyword);
       
         bool Delete(long id);
         bool CheckExit(string kyHieu);

   

    }
}