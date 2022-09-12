using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.IRepository
{
    public interface ICommentRepository
    {
        bool Edit(Comment model);
        long Insert(Comment model);
        List<Comment> GetAll();
        List<Comment> GetAllToDay();
        Comment GetById(long keyword);

        bool Delete(long id, bool isDelete);
         bool CheckExit(string tieuDe);
          List<Comment> GetBySanPhamId(long id);
          List<Comment> GetByThongBaoId(long id);
   

    }
}