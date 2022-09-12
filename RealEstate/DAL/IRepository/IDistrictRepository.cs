using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IDistrictRepository
    {
        Task<List<DistrictViewModel>> GetList();
        Task<DistrictViewModel> GetById(long? id);
        Task<bool> Update(DistrictViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(DistrictViewModel model);
        List<DistrictViewModel> GetAll(bool isDelete);
        IEnumerable<DistrictViewModel> GetAllDistrictsByProvinceId(long provinceid, bool isDelete);
    }
}