using RealEstatePropertyShared.Data;
using RealEstatePropertyShared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace RealEstatePropertyShared
{
    public class Context : DbContext
    {
        public Context()
        {
            //Database.SetInitializer(new PropertiesInitializer());
            
        }
        public DbSet<RealEstateProperty> RealEstateProperties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}