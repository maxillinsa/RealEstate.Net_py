using System;

namespace RealEstate.Models.ViewModels
{
    public class CompanyViewModel
    {
        public long Id { get; set; }
        public int? DefaulPageSize { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public double? ExchageRateUSD { get; set; }
        public bool? IsMaintenance { get; set; }
        public string Hotline { get; set; }
        public string CallCenter { get; set; }
        public string EmailAddress { get; set; }
        public string FacebookPage { get; set; }
        public string YoutubeChanel { get; set; }
        public string TwitterPage { get; set; }
        public string GoogleAnalytics { get; set; }
        public string ImageUrlLogo { get; set; }
        public string ImageUrlIcon { get; set; }
        public string TaxCode { get; set; }
        public int? NumberOfExpirationDates { get; set; }
        public string GoogleMapAPI { get; set; }
        public string Content { get; set; }
        public bool? IsActive { get; set; }
    }
}
