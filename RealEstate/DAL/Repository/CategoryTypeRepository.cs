using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class CategoryTypeRepository : ICategoryTypeRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public CategoryTypeRepository(PerfectRealDataContext dbContext)
        {
            _data = dbContext;
        }
        public bool Edit(CategoryType model)
        {
            try
            {
                CategoryType rs = _data.CategoryTypes.Where(n => n.CategoryTypeId == model.CategoryTypeId).FirstOrDefault();
                if(model.Name != null)
                rs.Name = model.Name;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public long Insert(CategoryType model)
        {
            try
            {
                _data.CategoryTypes.Add(model);
                _data.SaveChanges();
                return model.CategoryTypeId;
            }
            catch
            {
                return -1;
            }
        }
        public List<CategoryType> GetAll(bool isDelete)
        {
            return _data.CategoryTypes.ToList();
        }
        public List<CategoryType> GetAll()
        {
            return _data.CategoryTypes.ToList();
        }
        public CategoryType GetById(long keyword)
        {
            try
            {
                return _data.CategoryTypes.Where(n => n.CategoryTypeId == keyword).FirstOrDefault();
            }
            catch
            {
                return null;
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