using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class CommentRepository : ICommentRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public CommentRepository(PerfectRealDataContext dbContext)
        {
            _data = dbContext;
        }

        public List<Comment> GetByNotificationId (long id)
        {
            return _data.Comments.Where(x => x.NotificationId == id).ToList();
        }
        public List<Comment> GetByEstateId(long id)
        {
            return _data.Comments.Where(x => x.EstateId == id).ToList();
        }
        public bool Edit(Comment model)
        {
            try
            {
                Comment rs = _data.Comments.Where(n => n.Id == model.Id).FirstOrDefault();
                if (model.Contents != null)
                    rs.Contents = model.Contents;
                if (model.EstateId != null)
                    rs.EstateId = model.EstateId;
                if (model.IsDelete != null)
                    rs.IsDelete = model.IsDelete;
                if (model.CreateDate != null)
                    rs.CreateDate = model.CreateDate;
                if (model.NotificationId != null)
                    rs.NotificationId = model.NotificationId;
                if(model.CreateById != null)
                {
                    rs.CreateById = model.CreateById;
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
            Comment m = null;
            m = _data.Comments.Where(x => x.Contents == Title).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(Comment model)
        {
            try
            {
                _data.Comments.Add(model);
                _data.SaveChanges();
                return model.Id;
            }
            catch
            {
                return -1;
            }
        }
        public List<Comment> GetAll()
        {
            List<Comment> lstCommentBan = new List<Comment>();
            lstCommentBan = _data.Comments.Where(x=> x.IsDelete == false).OrderByDescending(x=> x.CreateDate).Take(500).ToList();
            return lstCommentBan;
        }

        public List<Comment> GetAllToDay()
        {
            DateTime td = DateTime.Now;
            List<Comment> lstCommentBan = new List<Comment>();
            lstCommentBan = _data.Comments.Where(x => x.CreateDate.Value.Day == td.Day && x.CreateDate.Value.Month == td.Month && x.CreateDate.Value.Year == td.Year).OrderByDescending(x => x.CreateDate).ToList();
            return lstCommentBan;
        }
        public Comment GetById(long keyword)
        {
            try
            {
                return _data.Comments.Where(n => n.Id == keyword).FirstOrDefault();
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
                Comment pd = _data.Comments.Find(id);
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