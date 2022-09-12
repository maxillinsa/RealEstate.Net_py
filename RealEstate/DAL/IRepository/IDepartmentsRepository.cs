using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IDepartmentsRepository
    {
        bool Edit(Department Department);
        long Insert(Department Department);
        List<Department> GetAll();
        Department GetById(long keyword);
        List<Department> GetAll(bool isDelete);
        bool Delete(long id, bool IsDelete);

       
    }
}