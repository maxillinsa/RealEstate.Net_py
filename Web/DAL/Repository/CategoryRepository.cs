using Web.DAL.IRepository;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository, IDisposable
    {
        private db_bdsEntities _data;
        public CategoryRepository()
        {
            _data = new db_bdsEntities();
        }
        public bool Edit(Category category)
        {
            try
            {
                Category rs = _data.Categories.Where(n => n.CategoryId == category.CategoryId).FirstOrDefault();
                rs.CategoryName = category.CategoryName;
                if (category.Image != null)
                rs.Image = category.Image;

                if (category.ParentCategoryId != null)
                    rs.ParentCategoryId = category.ParentCategoryId;
                else
                    {
                        rs.ParentCategoryId = 0;
                    }

                if (category.CateLevel != null)
                rs.CateLevel = category.CateLevel;
                rs.CreatedBy = category.CreatedBy;
                rs.CreatedDate = category.CreatedDate;
                if (category.ModifiedBy != null)
                    rs.ModifiedBy = category.ModifiedBy;
                if (category.OrderNumber != null)
                    rs.OrderNumber = category.OrderNumber;
                else 
                    {
                    Random random = new Random();
                        rs.OrderNumber = random.Next(1000, 2000);
                    }

                if (category.ParentCategoryId != null)
                    rs.ParentCategoryId = category.ParentCategoryId;

                if (category.CategoryTypeId != null)
                rs.CategoryTypeId = category.CategoryTypeId;

                rs.GroupCategoryId = category.GroupCategoryId;

                rs.Description = category.Description;
                rs.Alias = category.Alias;
                rs.MetaKeywords = category.MetaKeywords;
                rs.MetaDescription = category.MetaDescription;
                if (category.MetaTitle != null)
                rs.MetaTitle = category.MetaTitle;
                else
                {
                    rs.MetaTitle = category.CategoryName;
                }

                if (category.IsDelete != null)
                    rs.IsDelete = category.IsDelete;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public long Insert(Category category)
        {
            try
            {
                if (category.OrderNumber != null)
                    category.OrderNumber = category.OrderNumber;
                else
                {
                    Random random = new Random();
                    category.OrderNumber = random.Next(2000, 3000);
                }

                if (category.ParentCategoryId != null)
                    category.ParentCategoryId = category.ParentCategoryId;
                else
                {
                    category.ParentCategoryId = 0;
                }
                category.IsDelete = false;
                category.CreatedDate = DateTime.Now;
                _data.Categories.Add(category);
                _data.SaveChanges();
                return category.CategoryId;
            }
            catch
            {
                return -1;
            }
        }
        public List<Category> GetAll(bool isDelete)
        {
            return _data.Categories.Where(x => x.IsDelete == isDelete).ToList();
        }
        public List<Category> GetAll()
        {
            return _data.Categories.ToList();
        }
        public Category GetById(long keyword)
        {
            try
            {
                return _data.Categories.Where(n => n.CategoryId == keyword).FirstOrDefault();
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
                Category cg = _data.Categories.Find(id);
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