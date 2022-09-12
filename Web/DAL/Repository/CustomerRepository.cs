using Web.DAL.IRepository;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.Repository
{
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private db_bdsEntities _data;
        public CustomerRepository()
        {
            _data = new db_bdsEntities();
        }
        public List<Customer> GetAll(bool isDelete)
        {
            try
            {
                return _data.Customers.Where(x=> x.IsDelete == isDelete).ToList();
            }
            catch
            {
                return null;
            }
        }
        public List<Customer> GetAll()
        {
            try
            {
                return _data.Customers.ToList();
            }
            catch
            {
                return null;
            }
        }
        public bool Edit(Customer customer)
        {
            try
            {
                Customer rs = _data.Customers.Where(n => n.CustomerId == customer.CustomerId).FirstOrDefault();
                rs.CustomerName = customer.CustomerName;
                rs.Address = customer.Address;
                rs.Mobile = customer.Mobile;
                rs.IsDelete = customer.IsDelete;
                if (rs.Code == null)
                {
                    rs.Code = "KH" + rs.CreateDate.Value.Year + string.Format("{0:00000}", rs.CustomerId);
                }
                //rs.Code = customer.Code;
                rs.LicensePlates = customer.LicensePlates;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public long Insert(Customer customer)
        {
            try
            {
                customer.CreateDate = DateTime.Now;
                customer.IsDelete = false;
                _data.Customers.Add(customer);
                _data.SaveChanges();
                var item = _data.Customers.Find(customer.CustomerId);
                customer.Code = "KH" + customer.CreateDate.Value.Year + string.Format("{0:00000}", item.CustomerId);
                _data.SaveChanges();
                return customer.CustomerId;
            }
            catch
            {
                return -1;
            }
        }
        public Customer GetById(long keyword)
        {
            try
            {
                return _data.Customers.Where(n => n.CustomerId == keyword).FirstOrDefault();
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
                Customer pd = _data.Customers.Find(id);
                pd.IsDelete = IsDelete;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Remove(long keyword)
        {
            try
            {
                var or = _data.Customers.Find(keyword);
                _data.Customers.Remove(or);
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public long GetLastId()
        {

            try
            {
                var lst = _data.Customers.ToList();
                if (lst.Count > 1)
                {
                    var item = lst[lst.Count - 1];
                    return item.CustomerId;
                }
                else
                {
                    return lst[0].CustomerId;
                }
            }
            catch
            {
                return -1;
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