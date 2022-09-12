using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class Estate_TypesRepository : IEstate_TypesRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public Estate_TypesRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public bool Edit(Estate_Types model)
        {
            try
            {
                Estate_Types rs = _data.Estate_Types.FirstOrDefault(n => n.ItemId == model.ItemId);
                if (model.Name != null)
                    rs.Name = model.Name;
                if (model.EditDate != null)
                    rs.EditDate = model.EditDate;
                if (model.IsDelete != null)
                    rs.IsDelete = model.IsDelete;
                if (model.CreateDate != null)
                    rs.CreateDate = model.CreateDate;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool CheckExit(string name)
        {
            Estate_Types m = null;
            m = _data.Estate_Types.FirstOrDefault(x => x.Name == name);
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(Estate_Types model)
        {
            try
            {
                _data.Estate_Types.Add(model);
                _data.SaveChanges();
                return model.ItemId;
            }
            catch
            {
                return -1;
            }
        }
        public bool Create(Estate_Types model)
        {
            try
            {
                _data.Estate_Types.Add(model);
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Estate_Types> GetAll(bool isDelete)
        {
            List<Estate_Types> lstEstate_Types = new List<Estate_Types>();
            lstEstate_Types = _data.Estate_Types.Where(x => x.IsDelete == isDelete).ToList();
            return lstEstate_Types;
        }

        public List<Estate_Types> GetAll()
        {
            return _data.Estate_Types.ToList();
        }
        public Estate_Types GetById(long keyword)
        {
            try
            {
                return _data.Estate_Types.FirstOrDefault(n => n.ItemId == keyword);
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
                Estate_Types pd = _data.Estate_Types.Find(id);
                pd.IsDelete = IsDelete;
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