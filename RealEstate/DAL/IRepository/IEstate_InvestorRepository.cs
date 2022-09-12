using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IEstate_InvestorRepository
    {
        Task<List<Estate_InvestorViewModel>> GetList();
        Task<Estate_InvestorViewModel> GetById(long? id);
        Task<bool> Update(Estate_InvestorViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(Estate_InvestorViewModel model);
        List<Estate_InvestorViewModel> GetAll(bool isDelete);
    }
}