using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IEstate_StatusRepository
    {
        Task<List<Estate_StatusViewModel>> GetList();
        Task<Estate_StatusViewModel> GetById(long? id);
        Task<bool> Update(Estate_StatusViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(Estate_StatusViewModel model);
        List<Estate_StatusViewModel> GetAll(bool isDelete);
    }
}