using RealEstatePropertyShared.Data;
using RealEstatePropertyShared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RealEstatePropertyShared
{
    public class Context : DbContext
    {
        public Context() : base("ContextDb")
        {
            Database.SetInitializer<Context>(new PropertiesInitializer());
        }
        public DbSet<RealEstateProperty> RealEstateProperties { get; set; }
    }
}