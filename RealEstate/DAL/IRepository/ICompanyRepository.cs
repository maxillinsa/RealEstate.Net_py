using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DAL.IRepository
{
    public interface ICompanyRepository
    {
        Task<List<CompanyViewModel>> GetAllAsync();
        List<CompanyViewModel> GetAll();
        Task<CompanyViewModel> GetById(long? id);
        Task<bool> Update(CompanyViewModel model);
        Task<bool> Create(CompanyViewModel model);
    }
}
