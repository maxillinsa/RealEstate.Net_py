using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IVendorRepository
    {
        List<Vendor> GetAll();
        List<Vendor> GetAll(bool IsDelete);

        long Insert(Vendor vendor);

        Vendor GetById(long id);

        bool Edit(Vendor vendor);

        bool Delete(long id, bool IsDelete);
    }
}