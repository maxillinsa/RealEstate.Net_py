using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IGroupCategoryRepository
    {
         bool Edit(GroupCategory groupCategory);
         long Insert(GroupCategory groupCategory);
         List<GroupCategory> GetAll();
         List<GroupCategory> GetAll(bool isDelete);
         GroupCategory GetById(long keyword);
         bool Delete(long keyword, bool IsDelete);
    }
}