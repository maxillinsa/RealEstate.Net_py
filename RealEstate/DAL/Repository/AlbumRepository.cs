using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class AlbumRepository : IAlbumRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public AlbumRepository(PerfectRealDataContext dbContext)
        {
            _data = dbContext;
        }
        public bool Edit(Album model)
        {
            try
            {
                Album rs = _data.Albums.Where(n => n.AlbumId == model.AlbumId).FirstOrDefault();
               if(model.Name != null)
                rs.Name = model.Name;
               if (model.Image != null)
                rs.Image = model.Image;
                rs.MetaTitle = model.MetaTitle;
                rs.MetaKeywords = model.MetaKeywords;
                rs.MetaDescription = model.MetaDescription;
                rs.Tags = model.Tags;
                rs.Alias = model.Alias;
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
        public long Insert(Album model)
        {
            try
            {
                model.IsDelete = false;
                model.CreateDate = DateTime.Now;
                _data.Albums.Add(model);
                _data.SaveChanges();
                return model.AlbumId;
            }
            catch
            {
                return -1;
            }
        }
        public List<Album> GetAll(bool isDelete)
        {
            return _data.Albums.Where(x => x.IsDelete == isDelete).ToList();
        }
        public List<Album> GetAll()
        {
            return _data.Albums.ToList();
        }
        public Album GetById(long keyword)
        {
            try
            {
                return _data.Albums.Where(n => n.AlbumId == keyword).FirstOrDefault();
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
                Album cg = _data.Albums.Find(id);
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