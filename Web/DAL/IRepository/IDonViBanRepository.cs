using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.IRepository
{
    public interface IDonViBanRepository
    {
        bool Edit(DonViBan model);
        long Insert(DonViBan model);
        List<DonViBan> GetAll();
        DonViBan GetById(long keyword);
       
         bool Delete(long id);
         bool CheckExit(string kyHieu);

   

    }
}