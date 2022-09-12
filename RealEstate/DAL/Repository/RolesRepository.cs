using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.Models;
using RealEstate.DAL.IRepository;
using System.Data;

namespace RealEstate.DAL.Repository
{
    public class RolesRepository : IRolesRepository, IDisposable
    {

        private PerfectRealDataContext _data;
        public RolesRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public IList<Role> GetAll()
        {
            return _data.Roles.ToList();
        }
        public IList<Role> GetAll(bool isDelete)
        {
            return _data.Roles.Where(x => x.IsDelete == isDelete).ToList();
        }
        public bool Edit(Role role)
        {
            try
            {
                Role rs = _data.Roles.Where(n => n.RoleId == role.RoleId).FirstOrDefault();
                rs.RoleName = role.RoleName;
                rs.Description = role.Description;
                if (role.IsDelete != null)
                    rs.IsDelete = role.IsDelete;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public long Insert(Role role)
        {
            try
            {
                _data.Roles.Add(role);
                _data.SaveChanges();
                return role.RoleId;
            }
            catch
            {
                return -1;
            }
        }
        public Role GetById(long keyword)
        {
            try
            {
                return _data.Roles.Where(n => n.RoleId == keyword).FirstOrDefault();
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