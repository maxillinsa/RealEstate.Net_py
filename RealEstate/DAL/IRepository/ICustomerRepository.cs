using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface ICustomerRepository
    {
        bool CheckExitNameAndMobileCustomer(Customer model);
        bool Edit(Customer customer);
        long Insert(Customer customer);
         List<Customer> GetAll();
         Customer GetById(long keyword);
         List<Customer> GetAll(bool isDelete);
         bool Delete(long id, bool IsDelete);

         bool Remove(long keyword);

         long GetLastId();
    }
}