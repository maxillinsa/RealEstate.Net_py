using Web.DAL.IRepository;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.Repository
{
    public class CommentRepository : ICommentRepository, IDisposable
    {
        private db_bdsEntities _data;
        public CommentRepository()
        {
            _data = new db_bdsEntities();
        }

        public List<Comment> GetByThongBaoId (long id)
        {
            return _data.Comments.Where(x => x.ThongBaoId == id).ToList();
        }
        public List<Comment> GetBySanPhamId(long id)
        {
            return _data.Comments.Where(x => x.SanPhamId == id).ToList();
        }
        public bool Edit(Comment model)
        {
            try
            {
                Comment rs = _data.Comments.Where(n => n.Id == model.Id).FirstOrDefault();
                if (model.Contents != null)
                    rs.Contents = model.Contents;
                if (model.SanPhamId != null)
                    rs.SanPhamId = model.SanPhamId;
                if (model.IsDelete != null)
                    rs.IsDelete = model.IsDelete;
                if (model.CreateDate != null)
                    rs.CreateDate = model.CreateDate;
                if (model.ThongBaoId != null)
                    rs.ThongBaoId = model.ThongBaoId;
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
        public bool CheckExit(string tieuDe)
        {
            Comment m = null;
            m = _data.Comments.Where(x => x.Contents == tieuDe).FirstOrDefault();
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
            lstCommentBan = _data.Comments.ToList();
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