//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealEstate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        public long CustomerId { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string LicensePlates { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> CustomerCategoryId { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        public virtual CustomerCategory CustomerCategory { get; set; }
    }
}
