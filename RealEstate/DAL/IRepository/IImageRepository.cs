using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface IImageRepository
    {
        bool DeleteByAblumId(long id);
        List<Image> GetAllByAlbumId(long id);
        bool Edit(Image model);
        long Insert(Image model);
        List<Image> GetAll();
        Image GetById(long keyword);

        bool Delete(long id, bool IsDelete); 
        List<Image> GetAll(bool isDelete);
    }
}