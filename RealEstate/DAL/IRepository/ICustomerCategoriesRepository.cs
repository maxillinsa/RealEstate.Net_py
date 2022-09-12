using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface ICustomerCategoriesRepository
    {
        Task<List<CustomerCategoryViewModel>> GetList();
        Task<CustomerCategoryViewModel> GetById(long? id);
        Task<bool> Update(CustomerCategoryViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(CustomerCategoryViewModel model);
        List<CustomerCategoryViewModel> GetAll(bool isDelete);
    }
}