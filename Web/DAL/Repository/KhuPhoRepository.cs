using Web.DAL.IRepository;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.Repository
{
    public class KhuPhoRepository : IKhuPhoRepository, IDisposable
    {
        private db_bdsEntities _data;
        public KhuPhoRepository()
        {
            _data = new db_bdsEntities();
        }
        public bool Edit(KhuPho model)
        {
            try
            {
                KhuPho rs = _data.KhuPhoes.Where(n => n.KhuPhoId == model.KhuPhoId).FirstOrDefault();
                if (model.Ten != null)
                    rs.Ten = model.Ten;
                if (model.Address != null)
                    rs.Address = model.Address;
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
        public bool CheckExit(string tenKhuPho)
        {
            KhuPho m = null;
            m = _data.KhuPhoes.Where(x => x.Ten == tenKhuPho).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(KhuPho model)
        {
            try
            {
                _data.KhuPhoes.Add(model);
                _data.SaveChanges();
                return model.KhuPhoId;
            }
            catch
            {
                return -1;
            }
        }
        public List<KhuPho> GetAll(bool isDelete)
        {
            List<KhuPho> lstKhuPho = new List<KhuPho>();
            lstKhuPho = _data.KhuPhoes.Where(x => x.IsDelete == isDelete).ToList();
            return lstKhuPho;
        }

        public List<KhuPho> GetAll()
        {
            return _data.KhuPhoes.ToList();
        }
        public KhuPho GetById(long keyword)
        {
            try
            {
                return _data.KhuPhoes.Where(n => n.KhuPhoId == keyword).FirstOrDefault();
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
                KhuPho pd = _data.KhuPhoes.Find(id);
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