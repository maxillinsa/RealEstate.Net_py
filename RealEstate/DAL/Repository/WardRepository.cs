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
    public class WardRepository : IWardRepository, IDisposable
    {
        private readonly PerfectRealDataContext _data;
        public WardRepository()
        {
            this._data = new PerfectRealDataContext();
        }
        public async Task<List<WardViewModel>> GetList()
        {
            var model = await _data.Wards.OrderByDescending(x => x.Created).Select(x => new WardViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                DistrictId = x.DistrictId,
                DistrictName = x.District.Name
            }).ToListAsync();
            return model;
        }
        public List<WardViewModel> GetAll(bool isDelete)
        {
            var model =  _data.Wards.Where(x=> x.IsDelete == isDelete && x.District.IsDelete == false && x.District.Province.IsDelete == false).OrderByDescending(x => x.Created).Select(x => new WardViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                DistrictId = x.DistrictId,
                DistrictName = x.District.Name
            }).ToList();
            return model;
        }
        public IEnumerable<WardViewModel> GetAllWardsByDistrictId(long districtId, bool isDelete)
        {
            var model = _data.Wards.Where(x => x.IsDelete == isDelete).OrderByDescending(x => x.Created).Select(x => new WardViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                DistrictId = x.DistrictId,
                DistrictName = x.District.Name
            }).Where(x => x.IsDelete == isDelete && x.DistrictId == districtId).ToList();
            return model;
       }
        public async Task<WardViewModel> GetById(long ? id)
        {
            var model = await _data.Wards.Where(x => x.ItemId == id).Select(x => new WardViewModel
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
        public async Task<bool> Create(WardViewModel model)
        {
            try
            {
                var now = DateTime.Now;
                var my = new Ward();                  
                    my.Created = now;
                    my.Modified = now;
                    my.Content = model.Content;
                    my.Name = model.Name;
                    my.IsDelete = false;
                my.DistrictId = model.DistrictId;
                    my.IsPublished = true;             
               
                 _data.Wards.Add(my);
                await _data.SaveChangesAsync();
                return true;
            }
            catch (Exception E)
            {
                return false;
            }

        }
        public async Task<bool> Update(WardViewModel model)
        {
            try
            {
                var my = await _data.Wards.Where(x => x.ItemId == model.ItemId).FirstOrDefaultAsync();
                if (model.Name != my.Name)
                    my.Name = model.Name;
                if (model.Content != my.Content)
                    my.Content = model.Content;
                if (model.IsDelete != my.IsDelete)
                    my.IsDelete = model.IsDelete;
                if (model.DistrictId != my.DistrictId && model.DistrictId != null)
                    my.DistrictId = model.DistrictId;
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
                var my = await _data.Wards.Where(x => x.ItemId == id).FirstOrDefaultAsync();
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