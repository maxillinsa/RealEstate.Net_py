
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models.ViewModels
{
    public class Estate_ImageViewModel
    {
        public long ? ItemId { get; set; }       
        public string Title { get; set; }      
        public string Url { get; set; }
        public System.DateTime? Created { get; set; }
        public System.DateTime? Modified { get; set; }
        public bool IsDelete { get; set; }
        public long? EstateId { get; set; }
        public long? Type { get; set; }
    }
}