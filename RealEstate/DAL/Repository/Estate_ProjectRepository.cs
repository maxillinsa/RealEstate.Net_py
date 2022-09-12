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
    public class Estate_ProjectRepository : IEstate_ProjectRepository, IDisposable
    {
        private readonly PerfectRealDataContext _data;
        public Estate_ProjectRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }
        public async Task<List<Estate_ProjectViewModel>> GetList()
        {
            var model = await _data.Estate_Projects.OrderByDescending(x => x.Created).Select(x => new Estate_ProjectViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                Estate_InvestorId = x.Estate_InvestorId,
                Estate_Investor = new Estate_InvestorViewModel
                {
                    ItemId = x.Estate_InvestorId,
                    Name = x.Estate_Investors.Name
                },
                DistrictId = x.DistrictId,
                District = new DistrictViewModel
                {
                    ItemId = x.DistrictId,
                    Name = x.District.Name
                },
                Province = new ProvinceViewModel
                {
                    ItemId = x.District.ProvinceId,
                    Name = x.District.Province.Name
                }
            }).ToListAsync();
            return model;
        }
        public IEnumerable<Estate_ProjectViewModel> GetAllProjectsByInvestor(long investorId, bool isDelete)
        {
            var model = _data.Estate_Projects.Where(x => x.Estate_InvestorId == investorId && x.IsDelete == isDelete)
                .OrderByDescending(x => x.Created).Select(x => new Estate_ProjectViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                    DistrictId = x.DistrictId,
                    District = new DistrictViewModel
                    {
                        ItemId = x.DistrictId,
                        Name = x.District.Name
                    }
                }).ToList();
            return model;
        }
        public IEnumerable<Estate_ProjectViewModel> GetAllProjectsByDistrict(long districtId)
        {
            var model = _data.Estate_Projects.Where(x => x.DistrictId == districtId && x.IsDelete == false)
                .OrderByDescending(x => x.Created).Select(x => new Estate_ProjectViewModel
                {
                    Name = x.Name,
                    ItemId = x.ItemId,
                    Created = x.Created,
                    Modified = x.Modified,
                    IsDelete = x.IsDelete,
                    Content = x.Content,
                    DistrictId = x.DistrictId,
                    District = new DistrictViewModel
                    {
                        ItemId = x.DistrictId,
                        Name = x.District.Name
                    }
                }).ToList();
            return model;
        }
        public List<Estate_ProjectViewModel> GetAll(bool isDelete)
        {
            var model = _data.Estate_Projects.Where(x => x.IsDelete == isDelete).OrderByDescending(x => x.Created).Select(x => new Estate_ProjectViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                Estate_InvestorId = x.Estate_InvestorId,
                Estate_Investor = new Estate_InvestorViewModel
                {
                    ItemId = x.Estate_InvestorId,
                    Name = x.Estate_Investors.Name
                },
                  DistrictId = x.DistrictId,
                District = new DistrictViewModel
                {
                    ItemId = x.DistrictId,
                    Name = x.District.Name
                },
                Province = new ProvinceViewModel
                {
                    ItemId = x.District.ProvinceId,
                    Name = x.District.Province.Name
                }
            }).ToList();
            return model;
        }
        public async Task<Estate_ProjectViewModel> GetById(long ? id)
        {
            var model = await _data.Estate_Projects.Where(x => x.ItemId == id).Select(x => new Estate_ProjectViewModel
            {
                Name = x.Name,
                ItemId = x.ItemId,
                Created = x.Created,
                Modified = x.Modified,
                IsDelete = x.IsDelete,
                Content = x.Content,
                Estate_InvestorId = x.Estate_InvestorId,
                Estate_Investor = new Estate_InvestorViewModel
                {
                    ItemId = x.Estate_InvestorId,
                    Name = x.Estate_Investors.Name
                },
                DistrictId = x.DistrictId,
                District = new DistrictViewModel
                {
                    ItemId = x.DistrictId,
                    Name = x.District.Name
                },
                ProvinceId = x.District.ProvinceId,
                Province = new ProvinceViewModel
                {
                    ItemId = x.District.ProvinceId,
                    Name = x.District.Province.Name
                }
            }).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> Create(Estate_ProjectViewModel model)
        {
            try
            {
                var now = DateTime.Now;
                var my = new Estate_Projects();                  
                    my.Created = now;
                    my.Modified = now;
                    my.Content = model.Content;
                    my.Name = model.Name;
                    my.IsDelete = false;
                if (model.DistrictId != null)
                {
                    my.DistrictId = model.DistrictId;
                }
                if(model.Estate_InvestorId!= null)
                {
                    my.Estate_InvestorId = model.Estate_InvestorId;
                }                   
                 _data.Estate_Projects.Add(my);
                await _data.SaveChangesAsync();
                return true;
            }
            catch (Exception E)
            {
                return false;
            }

        }
        public async Task<bool> Update(Estate_ProjectViewModel model)
        {
            try
            {
                var my = await _data.Estate_Projects.Where(x => x.ItemId == model.ItemId).FirstOrDefaultAsync();
                if (model.Name != my.Name)
                    my.Name = model.Name;
                if (model.Content != my.Content)
                    my.Content = model.Content;
                if (model.IsDelete != my.IsDelete)
                    my.IsDelete = model.IsDelete;
                if(model.DistrictId != null)
                    my.DistrictId = model.DistrictId;
                if (model.Estate_InvestorId != null)
                    my.Estate_InvestorId = model.Estate_InvestorId;
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
                var my = await _data.Estate_Projects.Where(x => x.ItemId == id).FirstOrDefaultAsync();
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