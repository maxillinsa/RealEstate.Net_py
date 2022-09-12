
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models.ViewModels
{
    public class CustomerCategoryViewModel
    {
        public long ? CustomerCategoryId { get; set; }       
        public string Name { get; set; }     
        
        public System.DateTime ? Created { get; set; }
       
        public bool? IsDelete { get; set; }

    }   
}