using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IProvinceRepository
    {
        Task<List<ProvinceViewModel>> GetList();
        Task<ProvinceViewModel> GetById(long? id);
        Task<bool> Update(ProvinceViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(ProvinceViewModel model);
        List<ProvinceViewModel> GetAll(bool isDelete);
    }
}