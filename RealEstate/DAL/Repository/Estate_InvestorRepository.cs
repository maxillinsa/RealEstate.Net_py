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
    public class Estate_InvestorRepository : IEstate_InvestorRepository, IDisposable
    {
        private readonly PerfectRealDataContext _data;
        public Estate_InvestorRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public async Task<List<Estate_InvestorViewModel>> GetList()
        {
            var model = await _data.Estate_Investors.OrderByDescending(x => x.Created).Select(x => new Estate_InvestorViewModel
            {
                Name = x.Name,
                ItemId = x.Estate_InvestorId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content
            }).ToListAsync();
            return model;
        }
        public List<Estate_InvestorViewModel> GetAll(bool isDelete)
        {
            var model = _data.Estate_Investors.Where(x => x.IsDelete == isDelete).OrderByDescending(x => x.Created).Select(x => new Estate_InvestorViewModel
            {
                Name = x.Name,
                ItemId = x.Estate_InvestorId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content
            }).ToList();
            return model;
        }
        public async Task<Estate_InvestorViewModel> GetById(long ? id)
        {
            var model = await _data.Estate_Investors.Where(x => x.Estate_InvestorId == id).Select(x => new Estate_InvestorViewModel
            {
                Name = x.Name,
                ItemId = x.Estate_InvestorId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content
            }).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> Create(Estate_InvestorViewModel model)
        {
            try
            {
                var now = DateTime.Now;
                var my = new Estate_Investors();                  
                    my.Created = now;
                    my.Modified = now;
                    my.Content = model.Content;
                    my.Name = model.Name;
                    my.IsDelete = false;                              
               
                 _data.Estate_Investors.Add(my);
                await _data.SaveChangesAsync();
                return true;
            }
            catch (Exception E)
            {
                return false;
            }

        }
        public async Task<bool> Update(Estate_InvestorViewModel model)
        {
            try
            {
                var my = await _data.Estate_Investors.Where(x => x.Estate_InvestorId == model.ItemId).FirstOrDefaultAsync();
                if (model.Name != my.Name)
                    my.Name = model.Name;
                if (model.Content != my.Content)
                    my.Content = model.Content;
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
                var my = await _data.Estate_Investors.Where(x => x.Estate_InvestorId == id).FirstOrDefaultAsync();
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