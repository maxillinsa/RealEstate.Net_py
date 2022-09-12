using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IWardRepository
    {
        IEnumerable<WardViewModel> GetAllWardsByDistrictId(long districtId, bool isDelete);
        Task<List<WardViewModel>> GetList();
        Task<WardViewModel> GetById(long? id);
        Task<bool> Update(WardViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(WardViewModel model);
        List<WardViewModel> GetAll(bool isDelete);
    }
}