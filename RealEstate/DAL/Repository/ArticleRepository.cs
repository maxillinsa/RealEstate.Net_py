using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class ArticleRepository : IArticleRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public ArticleRepository(PerfectRealDataContext dbContext)
        {
            _data = dbContext;
        }
        public List<Article> TakeRecentTopBy(long id, int take)
        {
            return _data.Articles.Where(x => x.IsDelete == false && x.ArticleId != id).OrderByDescending(x => x.CreateDate).Take(take).ToList();
        }
        public List<Article> TakeByCategoryId(long id, long catType,int take)
        {
            return _data.Articles.Where(x => x.Category.CategoryTypeId == catType && x.CategoryId == id).OrderByDescending(x => x.CreateDate).Take(take).ToList();
        }
        public   List<Article> GetAllByCategoryId(long id, long catType)
            {
                return _data.Articles.Where(x => x.Category.CategoryTypeId == catType && x.CategoryId == id).OrderByDescending(x => x.CreateDate).ToList();
            }
        
        public bool Edit(Article model)
        {
            try
            {
                Article rs = _data.Articles.Where(n => n.ArticleId == model.ArticleId).FirstOrDefault();
                rs.Title = model.Title;
                if (model.Image != null)
                rs.Image = model.Image;
                rs.MetaTitle = model.MetaTitle;
                rs.MetaKeywords = model.MetaKeywords;
                rs.MetaDescription = model.MetaDescription;
                rs.Tags = model.Tags;
                rs.Short = model.Short;
                rs.Contents = model.Contents;
                rs.AllowComments = model.AllowComments;
                rs.Alias = model.Alias;
                if (model.CategoryId != null)
                    rs.CreateDate = model.CreateDate;
                else
                    rs.CreateDate = DateTime.Now;
                if (model.CategoryId != null)
                    rs.CategoryId = model.CategoryId;
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
        public long Insert(Article model)
        {
            try
            {
                model.IsDelete = false;
                model.CreateDate = DateTime.Now;
                _data.Articles.Add(model);
                _data.SaveChanges();
                return model.ArticleId;
            }
            catch
            {
                return -1;
            }
        }
        public List<Article> TakeTopArticle(bool isDelete, int take)
        {
            return _data.Articles.Where(x => x.IsDelete == isDelete).OrderByDescending(x=> x.CreateDate).Take(take).ToList();
        }
        public List<Article> GetAll(bool isDelete)
        {
            return _data.Articles.Where(x => x.IsDelete == isDelete).ToList();
        }
        public List<Article> GetAll()
        {
            return _data.Articles.ToList();
        }
        public Article GetById(long keyword)
        {
            try
            {
                return _data.Articles.Where(n => n.ArticleId == keyword).FirstOrDefault();
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
                Article cg = _data.Articles.Find(id);
                cg.IsDelete = IsDelete;
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