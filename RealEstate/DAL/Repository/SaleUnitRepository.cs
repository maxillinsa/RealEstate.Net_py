using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class SaleUnitRepository : ISaleUnitRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public SaleUnitRepository(PerfectRealDataContext dbContext)
        {
            _data = dbContext;
        }
        public bool Edit(SaleUnit model)
        {
            try
            {
                SaleUnit rs = _data.SaleUnits.Where(n => n.SaleUnitId == model.SaleUnitId).FirstOrDefault();
                if (model.Name != null)
                    rs.Name = model.Name;
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
        public bool CheckExit(string tenSaleUnit)
        {
            SaleUnit m = null;
            m = _data.SaleUnits.Where(x => x.Name == tenSaleUnit).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(SaleUnit model)
        {
            try
            {
                model.IsDelete = false;
                _data.SaleUnits.Add(model);
                _data.SaveChanges();
                return model.SaleUnitId;
            }
            catch
            {
                return -1;
            }
        }
        public List<SaleUnit> GetAll()
        {
            List<SaleUnit> lstSaleUnitBan = new List<SaleUnit>();
            lstSaleUnitBan = _data.SaleUnits.ToList();
            return lstSaleUnitBan;
        }

        public List<SaleUnit> GetAll(bool isDelete)
        {
            List<SaleUnit> lstSaleUnitBan = new List<SaleUnit>();
            lstSaleUnitBan = _data.SaleUnits.Where(x=> x.IsDelete == isDelete).ToList();
            return lstSaleUnitBan;
        }

        public SaleUnit GetById(long keyword)
        {
            try
            {
                return _data.SaleUnits.Where(n => n.SaleUnitId == keyword).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public bool Delete(long id)
        {
            try
            {
                SaleUnit pd = _data.SaleUnits.Find(id);
                //  _data.SaleUnits.Remove(pd);
                if(pd.IsDelete == null || pd.IsDelete == true)
                {
                    pd.IsDelete = false;
                }
                else
                pd.IsDelete = true;
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