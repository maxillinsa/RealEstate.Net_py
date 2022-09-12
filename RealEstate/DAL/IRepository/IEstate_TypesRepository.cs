using RealEstate.Models;
using System.Collections.Generic;

namespace RealEstate.DAL.IRepository
{
    public interface IEstate_TypesRepository
    {
        bool Create(Estate_Types model);
        bool Edit(Estate_Types model);
        long Insert(Estate_Types model);
         List<Estate_Types> GetAll();
        Estate_Types GetById(long keyword);
         List<Estate_Types> GetAll(bool isDelete);
         bool Delete(long id, bool IsDelete);
         bool CheckExit(string kyHieu);

   

    }
}