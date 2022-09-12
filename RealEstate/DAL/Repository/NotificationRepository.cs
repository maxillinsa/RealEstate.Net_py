using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class NotificationRepository : INotificationRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public NotificationRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public bool Edit(Notification model)
        {
            try
            {
                Notification rs = _data.Notifications.Where(n => n.Id == model.Id).FirstOrDefault();
                if (model.Title != null)
                    rs.Title = model.Title;
                if (model.Content != null)
                    rs.Content = model.Content;
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
        public bool CheckExit(string Title)
        {
            Notification m = null;
            m = _data.Notifications.Where(x => x.Title == Title).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(Notification model)
        {
            try
            {
                _data.Notifications.Add(model);
                _data.SaveChanges();
                return model.Id;
            }
            catch
            {
                return -1;
            }
        }
        public List<Notification> GetAll()
        {
            List<Notification> lstNotificationBan = new List<Notification>();
            lstNotificationBan = _data.Notifications.ToList();
            return lstNotificationBan;
        }
        
       
        public Notification GetById(long keyword)
        {
            try
            {
                return _data.Notifications.Where(n => n.Id == keyword).FirstOrDefault();
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
                Notification pd = _data.Notifications.Find(id);
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