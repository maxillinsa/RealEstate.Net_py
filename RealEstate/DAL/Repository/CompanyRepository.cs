using RealEstate.DAL.IRepository;
using RealEstate.Models.ViewModels;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.DAL.Repository
{
    public class CompanyRepository : ICompanyRepository , IDisposable
    {
        private PerfectRealDataContext _data;       
        public CompanyRepository(PerfectRealDataContext dbContext)
        {
            _data = dbContext;
        }
        public async Task<List<CompanyViewModel>> GetAllAsync()
        {
           
            var expression = PredicateBuilder.True<Company>();
            expression = PredicateBuilder.And(expression, x => x.IsActive != null && x.IsActive == false);
            var model = await _data.Companies.Where(expression).OrderByDescending(x => x.Id).Select(x => new CompanyViewModel
            {
                Id = x.Id,
                Address = x.Address,
                CallCenter = x.CallCenter,
                CompanyName = x.CompanyName,
                Content = x.Content,
                EmailAddress = x.EmailAddress,
                NumberOfExpirationDates = x.NumberOfExpirationDates,
                ExchageRateUSD = x.ExchageRateUSD,
                FacebookPage = x.FacebookPage,
                GoogleAnalytics = x.GoogleAnalytics,
                Hotline = x.Hotline,
                ImageUrlIcon = x.ImageUrlIcon,
                ImageUrlLogo = x.ImageUrlLogo,
                IsActive = x.IsActive,
                IsMaintenance = x.IsMaintenance,
                TaxCode = x.TaxCode,
                TwitterPage = x.TwitterPage,
                YoutubeChanel = x.YoutubeChanel,              

            }).ToListAsync();
            return model;
        }
        public  List<CompanyViewModel> GetAll()
        {

            var expression = PredicateBuilder.True<Company>();
           // expression = PredicateBuilder.And(expression, x => x.IsActive != null && x.IsActive == false);
            var model = _data.Companies.Where(expression).OrderByDescending(x => x.Id).Select(x => new CompanyViewModel
            {
                Id = x.Id,
                Address = x.Address,
                CallCenter = x.CallCenter,
                CompanyName = x.CompanyName,
                Content = x.Content,
                EmailAddress = x.EmailAddress,
                NumberOfExpirationDates = x.NumberOfExpirationDates,
                ExchageRateUSD = x.ExchageRateUSD,
                FacebookPage = x.FacebookPage,
                GoogleAnalytics = x.GoogleAnalytics,
                Hotline = x.Hotline,
                ImageUrlIcon = x.ImageUrlIcon,
                ImageUrlLogo = x.ImageUrlLogo,
                IsActive = x.IsActive,
                IsMaintenance = x.IsMaintenance,
                TaxCode = x.TaxCode,
                TwitterPage = x.TwitterPage,
                YoutubeChanel = x.YoutubeChanel,             
                GoogleMapAPI = x.GoogleMapAPI,

            }).ToList();
            return model;
        }
        public async Task<CompanyViewModel> GetById (long? id)
        {
            var model = await _data.Companies.Where(x=> x.Id == id ).Select(x=> new CompanyViewModel {

                Id = x.Id,
                Address = x.Address,
                CallCenter = x.CallCenter,
                CompanyName = x.CompanyName,
                Content = x.Content,
                EmailAddress = x.EmailAddress,
                NumberOfExpirationDates = x.NumberOfExpirationDates,
                ExchageRateUSD = x.ExchageRateUSD,
                FacebookPage = x.FacebookPage,
                GoogleAnalytics = x.GoogleAnalytics,
                Hotline = x.Hotline,
                ImageUrlIcon = x.ImageUrlIcon,
                ImageUrlLogo = x.ImageUrlLogo,
                IsActive = x.IsActive,
                IsMaintenance = x.IsMaintenance,
                TaxCode = x.TaxCode,
                TwitterPage = x.TwitterPage,
                YoutubeChanel = x.YoutubeChanel,
                GoogleMapAPI = x.GoogleMapAPI,
                DefaulPageSize = x.DefaulPageSize,
            }).FirstOrDefaultAsync();
            return model;
        }

        public async Task<bool> Update(CompanyViewModel model)
        {
             var my = await _data.Companies.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (my == null)
                return false;
            my.Id = model.Id;
            my.Address = model.Address;
            my.CallCenter = model.CallCenter;
            my.CompanyName = model.CompanyName;
            my.Content = model.Content;
            my.EmailAddress = model.EmailAddress;
            my.NumberOfExpirationDates = model.NumberOfExpirationDates;
            my.ExchageRateUSD = model.ExchageRateUSD;
            my.FacebookPage = model.FacebookPage;
            my.GoogleAnalytics = model.GoogleAnalytics;
            my.Hotline = model.Hotline;
            my.ImageUrlIcon = model.ImageUrlIcon;
            my.ImageUrlLogo = model.ImageUrlLogo;
            my.IsActive = model.IsActive;
            my.IsMaintenance = model.IsMaintenance;
            my.TaxCode = model.TaxCode;
            my.TwitterPage = model.TwitterPage;
            my.YoutubeChanel = model.YoutubeChanel;
            my.GoogleMapAPI = model.GoogleMapAPI;
            my.DefaulPageSize = model.DefaulPageSize;
            await  _data.SaveChangesAsync();          
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
          
        }
        public async Task<bool> Create(CompanyViewModel model)
        {
            var my = new Company
            {

                Address = model.Address,
                CallCenter = model.CallCenter,
                CompanyName = model.CompanyName,
                Content = model.Content,
                EmailAddress = model.EmailAddress,
                NumberOfExpirationDates = model.NumberOfExpirationDates,
                ExchageRateUSD = model.ExchageRateUSD,
                FacebookPage = model.FacebookPage,
                GoogleAnalytics = model.GoogleAnalytics,
                Hotline = model.Hotline,
                ImageUrlIcon = model.ImageUrlIcon,
                ImageUrlLogo = model.ImageUrlLogo,
                IsActive = model.IsActive,
                IsMaintenance = model.IsMaintenance,
                TaxCode = model.TaxCode,
                TwitterPage = model.TwitterPage,
                YoutubeChanel = model.YoutubeChanel,
                DefaulPageSize = model.DefaulPageSize,
                GoogleMapAPI = model.GoogleMapAPI,
            };
            _data.Companies.Add(my);
            await _data.SaveChangesAsync();
          
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        #region IDisposable Support
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


        #endregion
    }
}