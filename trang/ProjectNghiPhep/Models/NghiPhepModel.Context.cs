﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectNghiPhep.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NghiphepEntities : DbContext
    {
        public NghiphepEntities()
            : base("name=NghiphepEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ContractType> ContractTypes { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Title> Titles { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
