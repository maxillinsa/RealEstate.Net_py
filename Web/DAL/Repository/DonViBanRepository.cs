using Web.DAL.IRepository;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.Repository
{
    public class DonViBanRepository : IDonViBanRepository, IDisposable
    {
        private db_bdsEntities _data;
        public DonViBanRepository()
        {
            _data = new db_bdsEntities();
        }
        public bool Edit(DonViBan model)
        {
            try
            {
                DonViBan rs = _data.DonViBans.Where(n => n.DonViBanId == model.DonViBanId).FirstOrDefault();
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
        public bool CheckExit(string tenDonViBan)
        {
            DonViBan m = null;
            m = _data.DonViBans.Where(x => x.Ten == tenDonViBan).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(DonViBan model)
        {
            try
            {
                _data.DonViBans.Add(model);
                _data.SaveChanges();
                return model.DonViBanId;
            }
            catch
            {
                return -1;
            }
        }
        public List<DonViBan> GetAll()
        {
            List<DonViBan> lstDonViBanBan = new List<DonViBan>();
            lstDonViBanBan = _data.DonViBans.ToList();
            return lstDonViBanBan;
        }

       
        public DonViBan GetById(long keyword)
        {
            try
            {
                return _data.DonViBans.Where(n => n.DonViBanId == keyword).FirstOrDefault();
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
                DonViBan pd = _data.DonViBans.Find(id);
                _data.DonViBans.Remove(pd);
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