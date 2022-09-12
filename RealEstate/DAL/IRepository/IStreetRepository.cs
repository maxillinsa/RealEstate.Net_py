using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IStreetRepository
    {
        Task<List<StreetViewModel>> GetList();
        Task<StreetViewModel> GetById(long? id);
        Task<bool> Update(StreetViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(StreetViewModel model);
        List<StreetViewModel> GetAll(bool isDelete);
        IEnumerable<StreetViewModel> GetAllStreetsByDistrictId(long provinceid, bool isDelete);
    }
}