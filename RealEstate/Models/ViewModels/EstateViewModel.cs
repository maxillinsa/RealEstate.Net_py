using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models.ViewModels
{
    public class EstateViewModel: Estate
    {
        public HttpPostedFileBase[] Files2D { get; set; }
        public HttpPostedFileBase[] Files3D { get; set; }
        public HttpPostedFileBase[] CertificateImages { get; set; }
        public HttpPostedFileBase[] OtherImages { get; set; }
        public double? FinalSalePrice { get; set; }
        public double? FinalRentPrice { get; set; }
        public string FinalSalePriceStr => FormatFinalPrice(FinalSalePrice);
        public string FinalRentPriceStr => FormatFinalPrice(FinalRentPrice);
        public string RentPriceInUSD
        {
            get
            {
                if (RentUnitId != 7)
                    return string.Empty;
                var config = (CompanyViewModel)HttpContext.Current.Cache.Get("MyConfig");
                var exchangeRateInUSD = config != null ? config.ExchageRateUSD : 1;
                var usd = FinalRentPrice / exchangeRateInUSD;
                return (usd ?? 0).ToString("#,##0") + "/month";
            }
        }
        private string FormatFinalPrice(double? price)
        {
            if (!price.HasValue || price.Value == 0)
                return "";
            if (price / 1000000000 >= 1)
            {
                return (price / 1000000000) + " B";
            }
            else if (price / 1000000 >= 1)
            {
                return (price / 1000000) + " M";
            }
            else if (price / 100000 >= 1)
            {
                if (price / 100000 == 1)
                    return "100 thousands";
                else
                    return (price / 100000) + " Hundred thousands";
            }
            else if (price / 1000 >= 1)
            {
                return (price / 1000) + " Thousands";
            }
            else
            {
                return $"{price}";
            }
        }
        public bool ShowSaleUnit
        {
            get
            {
                return SaleUnitId.HasValue && (SaleUnitId == 4 || SaleUnitId == 5);
            }
        }
        public bool ShowRentUnit
        {
            get
            {
                return RentUnitId.HasValue && (RentUnitId == 4 || RentUnitId == 5 || RentUnitId == 6);
            }
        }
        public long? InvestorId { get; set; }
    }
    public class EstateDetailViewModel :Estate
    {
        public virtual Account Account { get; set; }
        public virtual SaleUnit SaleUnit { get; set; }
        public virtual RentUnit RentUnit { get; set; }
        public virtual Estate_Groups Estate_Groups { get; set; }
        public virtual ICollection<Estate_ImageViewModel> Estate_Images { get; set; }
        public virtual Estate_Projects Estate_Projects { get; set; }
        public virtual Estate_Statuses Estate_Statuses { get; set; }
        public virtual Estate_Types Estate_Types { get; set; }
        public virtual Town Town { get; set; }
        public virtual List<Comment> lstComments { get; set; }
        public int? TotalEstates { get; set; }
    }
}