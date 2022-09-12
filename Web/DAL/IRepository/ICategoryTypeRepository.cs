using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.IRepository
{
    public interface ICategoryTypeRepository
    {
        bool Edit(CategoryType model);
        long Insert(CategoryType model);
        List<CategoryType> GetAll();
        CategoryType GetById(long keyword);

      
        List<CategoryType> GetAll(bool isDelete);
    }
}