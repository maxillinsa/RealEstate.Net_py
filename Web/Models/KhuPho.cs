//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class KhuPho
    {
        public KhuPho()
        {
            this.SanPhams = new HashSet<SanPham>();
        }
    
        public long KhuPhoId { get; set; }
        public string Ten { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
