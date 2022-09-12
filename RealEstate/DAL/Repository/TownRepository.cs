using RealEstate.DAL.IRepository;
using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.Repository
{
    public class TownRepository : ITownRepository, IDisposable
    {
        private PerfectRealDataContext _data;
        public TownRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public bool Edit(Town model)
        {
            try
            {
                Town rs = _data.Towns.Where(n => n.TownId == model.TownId).FirstOrDefault();
                if (model.Name != null)
                    rs.Name = model.Name;
                if (model.Address != null)
                    rs.Address = model.Address;
                if (model.EstateProjectId != null)
                    rs.EstateProjectId = model.EstateProjectId;
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
        public bool CheckExit(string tenTown)
        {
            Town m = null;
            m = _data.Towns.Where(x => x.Name == tenTown).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(Town model)
        {
            try
            {
                _data.Towns.Add(model);
                _data.SaveChanges();
                return model.TownId;
            }
            catch
            {
                return -1;
            }
        }
        public List<Town> GetAll(bool isDelete)
        {
            List<Town> lstTown = new List<Town>();
            lstTown = _data.Towns.Where(x => x.IsDelete == isDelete).ToList();
            return lstTown;
        }
        public List<Town> GetAllByEstateProjectId(bool isDelete, long estateProjectId)
        {
            List<Town> lstTown = new List<Town>();
            lstTown = _data.Towns.Where(x => x.IsDelete == isDelete && x.EstateProjectId == estateProjectId).ToList();
            return lstTown;
        }
        public List<Town> GetAll()
        {
            return _data.Towns.ToList();
        }
        public Town GetById(long keyword)
        {
            try
            {
                return _data.Towns.Where(n => n.TownId == keyword).FirstOrDefault();
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
                Town pd = _data.Towns.Find(id);
                pd.IsDelete = IsDelete;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<TownViewModel> GetAllTownsByProject(long projectId, bool isDelete)
        {
            var model = _data.Towns.Where(x => x.EstateProjectId == projectId && x.IsDelete == isDelete)
                .OrderByDescending(x => x.CreateDate).Select(x => new TownViewModel
                {
                    Name = x.Name,
                    ItemId = x.TownId
                }).ToList();
            return model;
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