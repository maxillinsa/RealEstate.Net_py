using Web.DAL.IRepository;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.Repository
{
    public class ThongBaoRepository : IThongBaoRepository, IDisposable
    {
        private db_bdsEntities _data;
        public ThongBaoRepository()
        {
            _data = new db_bdsEntities();
        }
        public bool Edit(ThongBao model)
        {
            try
            {
                ThongBao rs = _data.ThongBaos.Where(n => n.Id == model.Id).FirstOrDefault();
                if (model.TieuDe != null)
                    rs.TieuDe = model.TieuDe;
                if (model.NoiDung != null)
                    rs.NoiDung = model.NoiDung;
                if (model.IsDelete != null)
                    rs.IsDelete = model.IsDelete;
                if (model.CreateDate != null)
                    rs.CreateDate = model.CreateDate;
                if (model.AccountId != null)
                    rs.AccountId = model.AccountId;
                if(model.AllowComment != null)
                {
                    rs.AllowComment = model.AllowComment;
                }
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool CheckExit(string tieuDe)
        {
            ThongBao m = null;
            m = _data.ThongBaos.Where(x => x.TieuDe == tieuDe).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(ThongBao model)
        {
            try
            {
                _data.ThongBaos.Add(model);
                _data.SaveChanges();
                return model.Id;
            }
            catch
            {
                return -1;
            }
        }
        public List<ThongBao> GetAll()
        {
            List<ThongBao> lstThongBaoBan = new List<ThongBao>();
            lstThongBaoBan = _data.ThongBaos.ToList();
            return lstThongBaoBan;
        }

       
        public ThongBao GetById(long keyword)
        {
            try
            {
                return _data.ThongBaos.Where(n => n.Id == keyword).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public bool Delete(long id, bool isDelete)
        {
            try
            {
                ThongBao pd = _data.ThongBaos.Find(id);
                pd.IsDelete = isDelete;
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