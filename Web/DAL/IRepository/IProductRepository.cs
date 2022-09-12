using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.IRepository
{
    public interface IProductRepository
    {
        List<Product> TakeByCategoryId2(long? id, long? catType, int take);
        List<Product> TakeOrtherTopByCateId(long id, int take);
        List<Product> TakeRecentTopBy(long id, int take);
        List<Product> TakeTopProducts(bool isDelete, int take);
         bool Edit(Product product);
         long Insert(Product product);
         List<Product> GetAll();
         Product GetById(long keyword);
         List<Product> GetAll(bool isDelete);
         List<Product> GetAllByCateId(long id);
         bool Delete(long id, bool IsDelete);
         bool CheckExit(string productName);
    }
}