
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models.ViewModels
{
    public class StreetViewModel
    {
        public long ? StreetId { get; set; }       
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public System.DateTime? CreatedDate { get; set; }       
        public bool? IsDelete { get; set; }
        public long? DistrictId { get; set; }
        public string DistrictName { get; set; }

    } 
    
}