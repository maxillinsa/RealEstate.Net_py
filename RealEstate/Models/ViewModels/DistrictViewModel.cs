
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models.ViewModels
{
    public class DistrictViewModel
    {
        public long ? ItemId { get; set; }       
        public string Name { get; set; }      
        public string Content { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Modified { get; set; }
        public bool IsDelete { get; set; }

        public long? ProvinceId { get; set; }
        public string ProvinceName { get; set; }

    } 
    
}