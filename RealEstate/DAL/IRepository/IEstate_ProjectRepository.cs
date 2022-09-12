using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IEstate_ProjectRepository
    {
        Task<List<Estate_ProjectViewModel>> GetList();
        Task<Estate_ProjectViewModel> GetById(long? id);
        IEnumerable<Estate_ProjectViewModel> GetAllProjectsByInvestor(long investorId, bool isDelete);
        Task<bool> Update(Estate_ProjectViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(Estate_ProjectViewModel model);
        List<Estate_ProjectViewModel> GetAll(bool isDelete);
        IEnumerable<Estate_ProjectViewModel> GetAllProjectsByDistrict(long districtId);
    }
}