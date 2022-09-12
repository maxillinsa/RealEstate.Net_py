
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models.ViewModels
{
    public class DepartmentViewModel
    {
        public long ? DepartmentId { get; set; }       
        public string DepartmentName { get; set; }      
        public string Description { get; set; }       
        public bool IsDelete { get; set; }     
        public CompanyViewModel Company { get; set; }
    }   
}