using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IEstate_GroupRepository
    {
        Task<List<Estate_GroupViewModel>> GetList();
        Task<Estate_GroupViewModel> GetById(long? id);
        Task<bool> Update(Estate_GroupViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(Estate_GroupViewModel model);
        List<Estate_GroupViewModel> GetAll(bool isDelete);
    }
}