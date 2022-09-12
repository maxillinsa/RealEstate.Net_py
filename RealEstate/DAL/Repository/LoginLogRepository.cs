using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.Models;
using RealEstate.DAL.IRepository;

namespace RealEstate.DAL.Repository
{
    public class LoginLogRepository : ILoginLogRepository, IDisposable
    {

        private PerfectRealDataContext _data;
        public LoginLogRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
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