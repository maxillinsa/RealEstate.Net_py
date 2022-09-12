using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface ICertificate_TypeRepository
    {
        Task<List<Certificate_TypeViewModel>> GetList();
        Task<Certificate_TypeViewModel> GetById(long? id);
        Task<bool> Update(Certificate_TypeViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(Certificate_TypeViewModel model);
        List<Certificate_TypeViewModel> GetAll(bool isDelete);
    }
}