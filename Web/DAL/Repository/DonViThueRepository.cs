using Web.DAL.IRepository;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.Repository
{
    public class DonViThueRepository : IDonViThueRepository, IDisposable
    {
        private db_bdsEntities _data;
        public DonViThueRepository()
        {
            _data = new db_bdsEntities();
        }
        public bool Edit(DonViThue model)
        {
            try
            {
                DonViThue rs = _data.DonViThues.Where(n => n.DonViThueId == model.DonViThueId).FirstOrDefault();
                if (model.Ten != null)
                    rs.Ten = model.Ten;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool CheckExit(string tenDonViThue)
        {
            DonViThue m = null;
            m = _data.DonViThues.Where(x => x.Ten == tenDonViThue).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(DonViThue model)
        {
            try
            {
                _data.DonViThues.Add(model);
                _data.SaveChanges();
                return model.DonViThueId;
            }
            catch
            {
                return -1;
            }
        }
        public List<DonViThue> GetAll()
        {
            List<DonViThue> lstDonViThueBan = new List<DonViThue>();
            lstDonViThueBan = _data.DonViThues.ToList();
            return lstDonViThueBan;
        }

       
        public DonViThue GetById(long keyword)
        {
            try
            {
                return _data.DonViThues.Where(n => n.DonViThueId == keyword).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public bool Delete(long id)
        {
            try
            {
                DonViThue pd = _data.DonViThues.Find(id);
                _data.DonViThues.Remove(pd);
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