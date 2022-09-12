using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.IRepository
{
    public interface IThongBaoRepository
    {
        bool Edit(ThongBao model);
        long Insert(ThongBao model);
        List<ThongBao> GetAll();
        ThongBao GetById(long keyword);

        bool Delete(long id, bool isDelete);
         bool CheckExit(string tieuDe);

   

    }
}