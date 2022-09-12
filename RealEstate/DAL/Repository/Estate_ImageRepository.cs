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
    public class Estate_ImageRepository : IEstate_ImageRepository, IDisposable
    {
        private readonly PerfectRealDataContext _data;
        public Estate_ImageRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public async Task<List<Estate_ImageViewModel>> GetList()
        {
            var model = await _data.Estate_Images.OrderByDescending(x => x.Created).Select(x => new Estate_ImageViewModel
            {
                Title = x.Title,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Type = x.Type,
                EstateId = x.EstateId,
                Url = x.Url

            }).ToListAsync();
            return model;
        }
        public List<Estate_ImageViewModel> GetAll(bool isDelete)
        {
            var model = _data.Estate_Images.Where(x => x.IsDelete == isDelete).OrderByDescending(x => x.Created).Select(x => new Estate_ImageViewModel
            {
                Title = x.Title,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Type = x.Type,
                EstateId = x.EstateId,
                Url = x.Url
            }).ToList();
            return model;
        }
        public List<Estate_ImageViewModel> GetAllByEstateId(long estateId,bool isDelete)
        {
            var model = _data.Estate_Images.Where(x => x.IsDelete == isDelete && x.EstateId == estateId).OrderByDescending(x => x.Created).Select(x => new Estate_ImageViewModel
            {
                Title = x.Title,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                Type = x.Type,
                EstateId = x.EstateId,
                IsDelete = x.IsDelete,
                Url = x.Url
            }).ToList();
            return model;
        }
        public async Task<Estate_ImageViewModel> GetById(long ? id)
        {
            var model = await _data.Estate_Images.Where(x => x.ItemId == id).Select(x => new Estate_ImageViewModel
            {
                Title = x.Title,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Url = x.Url,
                Type = x.Type,
                EstateId = x.EstateId,
            }).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> Create(Estate_ImageViewModel model)
        {
            try
            {
                var now = DateTime.Now;
                var my = new Estate_Images();                  
                    my.Created = now;
                    my.Modified = now;
                    my.Url = model.Url;
                    my.Title = model.Title;
                    my.IsDelete = false;
                   my.Type = Convert.ToInt32(model.Type);
                   my.EstateId = model.EstateId;
                 _data.Estate_Images.Add(my);
                await _data.SaveChangesAsync();
                return true;
            }
            catch (Exception E)
            {
                return false;
            }

        }
        public async Task<bool> Update(Estate_ImageViewModel model)
        {
            try
            {
                var my = await _data.Estate_Images.Where(x => x.ItemId == model.ItemId).FirstOrDefaultAsync();
                if (model.Title != my.Title)
                    my.Title = model.Title;
                if (model.Url != my.Url)
                    my.Url = model.Url;
                if (model.IsDelete != my.IsDelete)
                    my.IsDelete = model.IsDelete;
                if (model.Type != my.Type)
                    my.Type = Convert.ToInt32(model.Type);
                if (model.EstateId != my.EstateId)
                    my.EstateId = model.EstateId;
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
                var my = await _data.Estate_Images.Where(x => x.ItemId == id).FirstOrDefaultAsync();
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