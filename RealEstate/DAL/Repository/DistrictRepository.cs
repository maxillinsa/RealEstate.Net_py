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
    public class DistrictRepository : IDistrictRepository, IDisposable
    {
        private readonly PerfectRealDataContext _data;
        public DistrictRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public async Task<List<DistrictViewModel>> GetList()
        {
            var model = await _data.Districts.OrderByDescending(x => x.Created).Select(x => new DistrictViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                ProvinceId = x.ProvinceId,
                ProvinceName = x.Province.Name
            }).ToListAsync();
            return model;
        }
        public IEnumerable<DistrictViewModel> GetAllDistrictsByProvinceId(long provinceid, bool isDelete)
        {
            var model =  _data.Districts.OrderByDescending(x => x.Created).Select(x => new DistrictViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                ProvinceId = x.ProvinceId,
                ProvinceName = x.Province.Name
            }).Where(x=> x.ProvinceId == provinceid && x.IsDelete == isDelete).ToList();
            return model;
         }
        public List<DistrictViewModel> GetAll(bool isDelete)
        {
            var model =  _data.Districts.Where(x=> x.IsDelete == isDelete && x.Province.IsDelete == false).OrderByDescending(x => x.Created).Select(x => new DistrictViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                ProvinceId = x.ProvinceId,
                ProvinceName = x.Province.Name
            }).ToList();
            return model;
        }
        public async Task<DistrictViewModel> GetById(long ? id)
        {
            var model = await _data.Districts.Where(x => x.ItemId == id).Select(x => new DistrictViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content
            }).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> Create(DistrictViewModel model)
        {
            try
            {
                var now = DateTime.Now;
                var my = new District();                  
                    my.Created = now;
                    my.Modified = now;
                    my.Content = model.Content;
                    my.Name = model.Name;
                    my.IsDelete = false;
                    my.ProvinceId = model.ProvinceId;
                    my.IsPublished = true;             
               
                 _data.Districts.Add(my);
                await _data.SaveChangesAsync();
                return true;
            }
            catch (Exception E)
            {
                return false;
            }

        }
        public async Task<bool> Update(DistrictViewModel model)
        {
            try
            {
                var my = await _data.Districts.Where(x => x.ItemId == model.ItemId).FirstOrDefaultAsync();
                if (model.Name != my.Name)
                    my.Name = model.Name;
                if (model.Content != my.Content)
                    my.Content = model.Content;
                if (model.IsDelete != my.IsDelete)
                    my.IsDelete = model.IsDelete;
                if (model.ProvinceId != my.ProvinceId && model.ProvinceId != null)
                    my.ProvinceId = model.ProvinceId;
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
                var my = await _data.Districts.Where(x => x.ItemId == id).FirstOrDefaultAsync();
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