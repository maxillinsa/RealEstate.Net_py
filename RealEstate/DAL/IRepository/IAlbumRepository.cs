using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IAlbumRepository
    {
        bool Edit(Album model);
        long Insert(Album model);
        List<Album> GetAll();
        Album GetById(long keyword);

        bool Delete(long id, bool IsDelete); 
        List<Album> GetAll(bool isDelete);
    }
}