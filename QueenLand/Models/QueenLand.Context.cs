﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QueenLand.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class queenlandEntities : DbContext
    {
        public queenlandEntities()
            : base("name=queenlandEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<about> abouts { get; set; }
        public DbSet<address> addresses { get; set; }
        public DbSet<contact> contacts { get; set; }
        public DbSet<homecat> homecats { get; set; }
        public DbSet<homecatitem> homecatitems { get; set; }
        public DbSet<job> jobs { get; set; }
        public DbSet<slide> slides { get; set; }
        public DbSet<user> users { get; set; }
        public DbSet<video> videos { get; set; }
        public DbSet<news> news { get; set; }
        public DbSet<projectcontent> projectcontents { get; set; }
        public DbSet<projectitem> projectitems { get; set; }
        public DbSet<project> projects { get; set; }
    }
}
