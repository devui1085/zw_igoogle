﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZigmaWeb
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AppDataModelContainer : DbContext
    {
        public AppDataModelContainer()
            : base("name=AppDataModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Gadget> Gadgets { get; set; }
        public virtual DbSet<GadgetInstance> GadgetInstances { get; set; }
        public virtual DbSet<GadgetInstanceSetting> GadgetInstanceSettings { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }
        public virtual DbSet<UsernameBlackList> UsernameBlackLists { get; set; }
        public virtual DbSet<GadgetSetting> GadgetSettings { get; set; }
    }
}