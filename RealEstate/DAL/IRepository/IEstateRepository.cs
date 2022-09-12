using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IEstateRepository
    {
        EstateDetailViewModel GetEstateDetailViewById(long id);
        EstateViewModel Create(Estate model);
        EstateViewModel GetBySetMapLocation(long keyword);
        Task<List<EstateViewModel>> GetEstatesAsync(EstateArguments estateArguments);
        List<Estate> GetAllByAccountID(long accountId);    
        List<Estate> GetAllThisDay(bool isDelete);          
        bool Edit(Estate model, bool hasChanged3D = false);       
        List<Estate> GetAll();
        int CountTotalEstates();
        int CountTotalEstatesHot();
        Estate GetById(long keyword);     
        bool Delete(long id, bool IsDelete);
        string DeleteMedia(int mediaId);
        bool IsEstateExisting(Estate model);

        bool UpdateGhiChu(Estate model);
        bool UpdateIsHot(long id, bool isHot);
      

    }
}