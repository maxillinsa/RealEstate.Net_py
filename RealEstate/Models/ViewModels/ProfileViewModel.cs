
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models.ViewModels
{
    public class ProfileViewModel
    {
        public long ? AccountId { get; set; }       
        public string UserName { get; set; }      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Detail { get; set; }
        public System.DateTime ? Created { get; set; }       
        public bool IsDelete { get; set; }
        public string Mobile { get; set; }
        public DepartmentViewModel Department { get; set; }
        public int TotalEstates { get; set; }



    }
}