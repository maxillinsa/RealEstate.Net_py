using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface ICommentRepository
    {
        bool Edit(Comment model);
        long Insert(Comment model);
        List<Comment> GetAll();
        List<Comment> GetAllToDay();
        Comment GetById(long keyword);

        bool Delete(long id, bool isDelete);
         bool CheckExit(string Title);
          List<Comment> GetByEstateId(long id);
          List<Comment> GetByNotificationId(long id);
   

    }
}