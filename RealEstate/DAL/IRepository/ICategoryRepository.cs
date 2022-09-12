using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface ICategoryRepository
    {
        bool Edit(Category category);
        long Insert(Category category);
        List<Category> GetAll();
        Category GetById(long keyword);

        bool Delete(long id, bool IsDelete); 
        List<Category> GetAll(bool isDelete);
    }
}