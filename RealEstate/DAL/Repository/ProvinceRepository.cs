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
    public class ProvinceRepository : IProvinceRepository, IDisposable
    {
        private readonly PerfectRealDataContext _data;
        public ProvinceRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public async Task<List<ProvinceViewModel>> GetList()
        {
            var model = await _data.Provinces.OrderByDescending(x => x.Created).Select(x => new ProvinceViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                CountryId = x.CountryId,
                CountryName = x.Country.Name
            }).ToListAsync();
            return model;
        }
        public List<ProvinceViewModel> GetAll(bool isDelete)
        {
            var model =  _data.Provinces.Where(x=> x.IsDelete == isDelete).OrderByDescending(x => x.Created).Select(x => new ProvinceViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                CountryId = x.CountryId,
                CountryName = x.Country.Name
            }).ToList();
            return model;
        }
        public async Task<ProvinceViewModel> GetById(long ? id)
        {
            var model = await _data.Provinces.Where(x => x.ItemId == id).Select(x => new ProvinceViewModel
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
        public async Task<bool> Create(ProvinceViewModel model)
        {
            try
            {
                var now = DateTime.Now;
                var my = new Province();                  
                    my.Created = now;
                    my.Modified = now;
                    my.Content = model.Content;
                    my.Name = model.Name;
                    my.IsDelete = false;
                my.CountryId = model.CountryId;
                    my.IsPublished = true;             
               
                 _data.Provinces.Add(my);
                await _data.SaveChangesAsync();
                return true;
            }
            catch (Exception E)
            {
                return false;
            }

        }
        public async Task<bool> Update(ProvinceViewModel model)
        {
            try
            {
                var my = await _data.Provinces.Where(x => x.ItemId == model.ItemId).FirstOrDefaultAsync();
                if (model.Name != my.Name)
                    my.Name = model.Name;
                if (model.Content != my.Content)
                    my.Content = model.Content;
                if (model.IsDelete != my.IsDelete)
                    my.IsDelete = model.IsDelete;
                if (model.CountryId != my.CountryId && model.CountryId != null)
                    my.CountryId = model.CountryId;
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
                var my = await _data.Provinces.Where(x => x.ItemId == id).FirstOrDefaultAsync();
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