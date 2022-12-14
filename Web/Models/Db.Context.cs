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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_bdsEntities : DbContext
    {
        public db_bdsEntities()
            : base("name=db_bdsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountRole> AccountRoles { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryType> CategoryTypes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<DonViBan> DonViBans { get; set; }
        public virtual DbSet<DonViThue> DonViThues { get; set; }
        public virtual DbSet<GroupCategory> GroupCategories { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<KhuPho> KhuPhoes { get; set; }
        public virtual DbSet<LoginLog> LoginLogs { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<ThongBao> ThongBaos { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
    }
}
