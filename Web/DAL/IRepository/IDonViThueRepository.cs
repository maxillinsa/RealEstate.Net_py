using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.IRepository
{
    public interface IDonViThueRepository
    {
        bool Edit(DonViThue model);
        long Insert(DonViThue model);
        List<DonViThue> GetAll();
        DonViThue GetById(long keyword);
       
         bool Delete(long id);
         bool CheckExit(string kyHieu);

   

    }
}