using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface ISaleUnitRepository
    {
        bool Edit(SaleUnit model);
        long Insert(SaleUnit model);
        List<SaleUnit> GetAll();
        List<SaleUnit> GetAll(bool isDelete);
        SaleUnit GetById(long keyword);

        bool Delete(long id);
         bool CheckExit(string kyHieu);

   

    }
}