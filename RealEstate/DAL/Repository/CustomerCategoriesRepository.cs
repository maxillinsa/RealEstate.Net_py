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
    public class CustomerCategoriesRepository : ICustomerCategoriesRepository, IDisposable
    {
        private readonly PerfectRealDataContext _data;
        public CustomerCategoriesRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public async Task<List<CustomerCategoryViewModel>> GetList()
        {
            var model = await _data.CustomerCategories.OrderByDescending(x => x.Created).Select(x => new CustomerCategoryViewModel
            {
                Name = x.Name,
                CustomerCategoryId = x.CustomerCategoryId,
                Created = x.Created,                
                IsDelete = x.IsDelete,              
            }).ToListAsync();
            return model;
        }
        public List<CustomerCategoryViewModel> GetAll(bool isDelete)
        {
            var model = _data.CustomerCategories.Where(x => x.IsDelete == isDelete).OrderByDescending(x => x.Created).Select(x => new CustomerCategoryViewModel
            {
                Name = x.Name,
                CustomerCategoryId = x.CustomerCategoryId,
                Created = x.Created,
                IsDelete = x.IsDelete,

            }).ToList();
            return model;
        }
        public async Task<CustomerCategoryViewModel> GetById(long ? id)
        {
            var model = await _data.CustomerCategories.Where(x => x.CustomerCategoryId == id).Select(x => new CustomerCategoryViewModel
            {
                Name = x.Name,
                CustomerCategoryId = x.CustomerCategoryId,
                Created = x.Created,
                IsDelete = x.IsDelete,

            }).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> Create(CustomerCategoryViewModel model)
        {
            try
            {
                var now = DateTime.Now;
                var my = new CustomerCategory();                  
                    my.Created = now;                   
                    my.Name = model.Name;
                    my.IsDelete = false;
                       
                 _data.CustomerCategories.Add(my);
                await _data.SaveChangesAsync();
                return true;
            }
            catch (Exception E)
            {
                return false;
            }

        }
        public async Task<bool> Update(CustomerCategoryViewModel model)
        {
            try
            {
                var my = await _data.CustomerCategories.Where(x => x.CustomerCategoryId == model.CustomerCategoryId).FirstOrDefaultAsync();
                if (model.Name != my.Name)
                    my.Name = model.Name;
                my.Created = DateTime.Now;
                if (model.IsDelete != my.IsDelete)
                    my.IsDelete = model.IsDelete;
               
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
                var my = await _data.CustomerCategories.Where(x => x.CustomerCategoryId == id).FirstOrDefaultAsync();
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