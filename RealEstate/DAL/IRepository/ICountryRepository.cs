using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface ICountryRepository
    {
        Task<List<CountryViewModel>> GetList();
        Task<CountryViewModel> GetById(long? id);
        Task<bool> Update(CountryViewModel model);
        Task<bool> UpdateIsDelete(long id, bool isDelete);
        Task<bool> Create(CountryViewModel model);
        List<CountryViewModel> GetAll(bool isDelete);
    }
}