using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class ImageRepository : IImageRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public ImageRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public bool DeleteByAblumId(long id)
        {
            try{
            Image img = _data.Images.Where(x => x.ImageId == id).FirstOrDefault();
             _data.Images.Remove(img);
             return true;
            }
            catch
                {
                    return false;
                }
        }
        public List<Image> GetAllByAlbumId (long id)
            {
                return _data.Images.Where(x => x.AlbumId == id && x.IsDelete == false).ToList();
            }
        public bool Edit(Image model)
        {
            try
            {
                Image rs = _data.Images.Where(n => n.ImageId == model.ImageId).FirstOrDefault();
                rs.Name = model.Name;
                if(model.AlbumId != null)
                rs.AlbumId = model.AlbumId;
                if (model.IsDelete != null)
                rs.IsDelete = model.IsDelete;
                rs.Link = model.Link;
                rs.Title = model.Title;
                if (model.CreateDate != null)
                    rs.CreateDate = model.CreateDate;
                else
                    rs.CreateDate = DateTime.Now;
                if (model.IsDelete != null)
                    rs.IsDelete = model.IsDelete;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public long Insert(Image model)
        {
            try
            {
                model.IsDelete = false;
                model.CreateDate = DateTime.Now;
                _data.Images.Add(model);
                _data.SaveChanges();
                return model.ImageId;
            }
            catch
            {
                return -1;
            }
        }
        public List<Image> GetAll(bool isDelete)
        {
            return _data.Images.Where(x => x.IsDelete == isDelete).ToList();
        }
        public List<Image> GetAll()
        {
            return _data.Images.ToList();
        }
        public Image GetById(long keyword)
        {
            try
            {
                return _data.Images.Where(n => n.ImageId == keyword).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public bool Delete(long id, bool IsDelete)
        {
            try
            {
                Image cg = _data.Images.Find(id);
                cg.IsDelete = IsDelete;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region disposed
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _data.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}