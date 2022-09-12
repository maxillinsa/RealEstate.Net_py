
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models.ViewModels
{
    public class Estate_ProjectViewModel
    {
        public long ? ItemId { get; set; }
        public long? Estate_InvestorId { get; set; }
        public Estate_InvestorViewModel Estate_Investor { get; set; }
        public string Name { get; set; }      
        public string Content { get; set; }
        public long? DistrictId { get; set; }
        public DistrictViewModel District { get; set; }
        public long? ProvinceId { get; set; }
        public ProvinceViewModel Province { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Modified { get; set; }
        public bool IsDelete { get; set; }
        
    }
    public class TownViewModel
    {
        public long? ItemId { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; }

    }
}