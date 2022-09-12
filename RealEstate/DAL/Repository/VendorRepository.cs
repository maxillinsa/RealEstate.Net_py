using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace RealEstate.DAL.Repository
{
    public class VendorRepository : IVendorRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public VendorRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public List<Vendor> GetAll()
        {
            try
            {
                return _data.Vendors.ToList();
            }
            catch
            {
                return null;
            }
        }
        public List<Vendor> GetAll(bool IsDelete)
        {
            try
            {
                return _data.Vendors.Where(n => n.IsDelete == IsDelete).ToList();
            }
            catch
            {
                return null;
            }
        }
        public long Insert(Vendor vendor)
        {
            try
            {
                _data.Vendors.Add(vendor);
                _data.SaveChanges();
                return vendor.VendorId;
            }
            catch
            {
                return -1;
            }
        }

        public Vendor GetById(long id)
        {
            try
            {
                return _data.Vendors.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public bool Edit(Vendor vendor)
        {
            try
            {
                Vendor rs = _data.Vendors.Find(vendor.VendorId);
                if (vendor.IsDelete == null)
                {
                    rs.IsDelete = vendor.IsDelete;
                }
                rs.Address = vendor.Address;
                rs.Description = vendor.Description;
                rs.Email = vendor.Email;
                rs.Mobile = vendor.Mobile;
                rs.VendorName = vendor.VendorName;
                vendor.CreateDate = rs.CreateDate;
                //_data.Entry(vendor).State = EntityState.Modified;
                    
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(long id, bool IsDelete)
        {
            try
            {
                var rs = _data.Vendors.Find(id);
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