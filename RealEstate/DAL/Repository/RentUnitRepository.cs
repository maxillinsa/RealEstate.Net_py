using RealEstate.DAL.IRepository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class RentUnitRepository : IRentUnitRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public RentUnitRepository(PerfectRealDataContext dbContext)
        {
            _data = dbContext;
        }
        public bool Edit(RentUnit model)
        {
            try
            {
                RentUnit rs = _data.RentUnits.Where(n => n.RentUnitId == model.RentUnitId).FirstOrDefault();
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
        public bool CheckExit(string tenRentUnit)
        {
            RentUnit m = null;
            m = _data.RentUnits.Where(x => x.Name == tenRentUnit).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(RentUnit model)
        {
            try
            {
                model.IsDelete = false;
                _data.RentUnits.Add(model);
                _data.SaveChanges();
                return model.RentUnitId;
            }
            catch
            {
                return -1;
            }
        }
        public List<RentUnit> GetAll()
        {
            List<RentUnit> lstRentUnitBan = new List<RentUnit>();
            lstRentUnitBan = _data.RentUnits.ToList();
            return lstRentUnitBan;
        }
        public List<RentUnit> GetAll(bool isDelete)
        {
            List<RentUnit> lstRentUnitBan = new List<RentUnit>();
            lstRentUnitBan = _data.RentUnits.Where(x=> x.IsDelete == isDelete).ToList();
            return lstRentUnitBan;
        }

        public RentUnit GetById(long keyword)
        {
            try
            {
                return _data.RentUnits.Where(n => n.RentUnitId == keyword).FirstOrDefault();
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
                RentUnit pd = _data.RentUnits.Find(id);
                if (pd.IsDelete == null || pd.IsDelete == true)
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