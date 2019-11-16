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
        public DbSet<RealEstateProperty> REIProperties { get; set; }
    }
}