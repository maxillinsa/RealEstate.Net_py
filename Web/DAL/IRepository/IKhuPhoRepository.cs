using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.IRepository
{
    public interface IKhuPhoRepository
    {
        bool Edit(KhuPho model);
        long Insert(KhuPho model);
         List<KhuPho> GetAll();
         KhuPho GetById(long keyword);
         List<KhuPho> GetAll(bool isDelete);
         bool Delete(long id, bool IsDelete);
         bool CheckExit(string kyHieu);

   

    }
}