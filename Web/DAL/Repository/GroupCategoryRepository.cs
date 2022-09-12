using Web.DAL.IRepository;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.Repository
{
    public class GroupCategoryRepository : IGroupCategoryRepository, IDisposable
    {

        private db_bdsEntities _data;
        public GroupCategoryRepository()
        {
            _data = new db_bdsEntities();
        }
        public bool Edit(GroupCategory groupCategory)
        {
            try
            {
                GroupCategory rs = _data.GroupCategories.Where(n => n.GroupCategoryId == groupCategory.GroupCategoryId).FirstOrDefault();
                rs.GroupCategoryName = groupCategory.GroupCategoryName;
                if (groupCategory.IsDelete != null)
                    rs.IsDelete = groupCategory.IsDelete;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public long Insert(GroupCategory groupCategory)
        {
            try
            {
                _data.GroupCategories.Add(groupCategory);
                _data.SaveChanges();
                return groupCategory.GroupCategoryId;
            }
            catch
            {
                return -1;
            }
        }
        public List<GroupCategory> GetAll()
        {
            return _data.GroupCategories.ToList();
        }
        public List<GroupCategory> GetAll(bool isDelete)
        {
            return _data.GroupCategories.Where(x => x.IsDelete == isDelete).ToList();
        }
        public GroupCategory GetById(long keyword)
        {
            try
            {
                return _data.GroupCategories.Where(n => n.GroupCategoryId == keyword).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public bool Delete(long keyword, bool IsDelete)
        {
            try
            {
                var rs = _data.GroupCategories.Find(keyword);
                rs.IsDelete = IsDelete;
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