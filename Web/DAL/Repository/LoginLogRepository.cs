using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;
using Web.DAL.IRepository;

namespace Web.DAL.Repository
{
    public class LoginLogRepository : ILoginLogRepository, IDisposable
    {

        private db_bdsEntities _data;
        public LoginLogRepository()
        {
            this._data = new db_bdsEntities();
        }
        public IList<LoginLog> GetAll()
        {
            return _data.LoginLogs.OrderByDescending(x=> x.CreateDate).ToList();
        }
        public IList<LoginLog> GetAllByAccId(long AccountId)
        {
            return _data.LoginLogs.Where(x => x.AccountId == AccountId).ToList();
        }
        public bool Insert(LoginLog LoginLog)
        {
            try
            {
                LoginLog.CreateDate = DateTime.Now;
                _data.LoginLogs.Add(LoginLog);
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public LoginLog GetById(long keyword)
        {
            try
            {
                return _data.LoginLogs.Where(n => n.LoginLogId == keyword).FirstOrDefault();
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