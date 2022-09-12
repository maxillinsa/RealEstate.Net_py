using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IEstate_ImageRepository
    {
        Task<List<Estate_ImageViewModel>> GetList();
        Task<Estate_ImageViewModel> GetById(long? id);
        Task<bool> Update(Estate_ImageViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(Estate_ImageViewModel model);
        List<Estate_ImageViewModel> GetAll(bool isDelete);
        List<Estate_ImageViewModel> GetAllByEstateId(long estateId, bool isDelete);
    }
}