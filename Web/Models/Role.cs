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
    
    public partial class Role
    {
        public Role()
        {
            this.AccountRoles = new HashSet<AccountRole>();
        }
    
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
    }
}