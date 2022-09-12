using RealEstate.DAL.IRepository;
using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System;

namespace RealEstate.DAL.Repository
{
    public class StreetRepository : IStreetRepository, IDisposable
    {
        private readonly PerfectRealDataContext _data;
        public StreetRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public async Task<List<StreetViewModel>> GetList()
        {
            var model = await _data.Streets.OrderByDescending(x => x.CreatedDate).Select(x => new StreetViewModel
            {
                Name = x.Name,
                StreetId = x.StreetId,
                CreatedDate = x.CreatedDate,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                IsDelete = x.IsDelete,               
                DistrictId = x.DistrictId,
                DistrictName = x.District.Name
            }).ToListAsync();
            return model;
        }
        public IEnumerable<StreetViewModel> GetAllStreetsByDistrictId(long districtId, bool isDelete)
        {
            var model =  _data.Streets.OrderByDescending(x => x.CreatedDate).Select(x => new StreetViewModel
            {
                Name = x.Name,
                StreetId = x.StreetId,
                CreatedDate = x.CreatedDate,
                IsDelete = x.IsDelete,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                DistrictId = x.DistrictId,
                DistrictName = x.District.Name
            }).Where(x=> x.DistrictId == districtId && x.IsDelete == isDelete).ToList();
            return model;
         }
        public List<StreetViewModel> GetAll(bool isDelete)
        {
            var model =  _data.Streets.Where(x=> x.IsDelete == isDelete).OrderByDescending(x => x.CreatedDate).Select(x => new StreetViewModel
            {
                Name = x.Name,
                StreetId = x.StreetId,
                CreatedDate = x.CreatedDate,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                IsDelete = x.IsDelete,
                DistrictId = x.DistrictId,
                DistrictName = x.District.Name
            }).ToList();
            return model;
        }
        public async Task<StreetViewModel> GetById(long ? id)
        {
            var model = await _data.Streets.Where(x => x.StreetId == id).Select(x => new StreetViewModel
            {
                Name = x.Name,
                StreetId = x.StreetId,
                CreatedDate = x.CreatedDate,
                IsDelete = x.IsDelete,
                DistrictId = x.DistrictId,
                DistrictName = x.District.Name
            }).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> Create(StreetViewModel model)
        {
            try
            {
                var now = DateTime.Now;
                var my = new Street();                  
                    my.CreatedDate = now;
                    my.Name = model.Name;
                    my.IsDelete = false;
                    my.DistrictId = model.DistrictId;
                   my.Latitude = model.Latitude;
                  my.Longitude = model.Longitude;
                 _data.Streets.Add(my);
                await _data.SaveChangesAsync();
                return true;
            }
            catch (Exception E)
            {
                return false;
            }

        }
        public async Task<bool> Update(StreetViewModel model)
        {
            try
            {
                var my = await _data.Streets.Where(x => x.StreetId == model.StreetId).FirstOrDefaultAsync();
                if (model.Name != my.Name)
                    my.Name = model.Name;
                if (model.Latitude != my.Latitude)
                    my.Latitude = model.Latitude;
                if (model.Longitude != my.Longitude)
                    my.Longitude = model.Longitude;
                if (model.IsDelete != my.IsDelete)
                    my.IsDelete = model.IsDelete;
                if (model.DistrictId != my.DistrictId && model.DistrictName != null)
                    my.DistrictId = model.DistrictId;
                my.CreatedDate = DateTime.Now;
                await _data.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }         

        }
        public async Task<bool> UpdateIsDelete(long id, bool isDelete)
        {
            try
            {
                var my = await _data.Streets.Where(x => x.StreetId == id).FirstOrDefaultAsync();
                if (my != null)
                {
                    my.IsDelete = isDelete;
                    await _data.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
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