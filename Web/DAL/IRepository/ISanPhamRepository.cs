using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.IRepository
{
    public interface ISanPhamRepository
    {
        bool EditByUser(SanPham model);
        List<SanPham> GetAllThisDay(bool isDelete);
        bool Edit(SanPham model);
        long Insert(SanPham model);
         List<SanPham> GetAll();
         SanPham GetById(long keyword);
         List<SanPham> GetAll(bool isDelete);
         bool Delete(long id, bool IsDelete);
         bool CheckExit(string kyHieu);
         bool UpdateIsHot(long id, bool isHot);
         List<SanPham> GetAllThisMonth(bool isDelete);
         List<SanPham> LayDanhSachSPTheoKyHieu(string kyHieu);

    }
}