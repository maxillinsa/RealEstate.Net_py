using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class DepartmentsRepository : IDepartmentsRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public DepartmentsRepository( PerfectRealDataContext dbContext)
        {
            _data = dbContext;
        }
        public bool Edit(Department Department)
        {
            try
            {
                Department rs = _data.Departments.Where(n => n.DepartmentId == Department.DepartmentId).FirstOrDefault();
                rs.DepartmentName = Department.DepartmentName;
                rs.CompanyId = Department.CompanyId;
                if(Department.IsDelete != null)
                rs.IsDelete = Department.IsDelete;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false; 
            }
            
        }
        public long Insert(Department Department)
        {
            try
            {
                Department.IsDelete = false;
                Department.CreatDate = DateTime.Now;
                _data.Departments.Add(Department);
                _data.SaveChanges();
                return Department.DepartmentId;
            }
            catch
            {
                return -1;
            }
        }
        public List<Department> GetAll()
        {
            return _data.Departments.ToList();
        }
        public List<Department> GetAll(bool isDelete)
        {
            try
            {
                return _data.Departments.Where(n => n.IsDelete == isDelete).ToList();
            }
            catch
            {
                return null;
            }
        }
        public Department GetById(long keyword)
        {
            try
            {
                return _data.Departments.Where(n => n.DepartmentId == keyword).FirstOrDefault();
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
                Department cg = _data.Departments.Find(id);
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