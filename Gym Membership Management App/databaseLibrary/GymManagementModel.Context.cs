﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace databaseLibrary
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class theGymDBEntities : DbContext
    {
        public theGymDBEntities(string strConnection)
            : base(strConnection)
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<tblClient> tblClients { get; set; }
        public virtual DbSet<tblClientMembership> tblClientMemberships { get; set; }
        public virtual DbSet<tblPlan> tblPlans { get; set; }
        public virtual DbSet<tblSecurityLevel> tblSecurityLevels { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
    }
}