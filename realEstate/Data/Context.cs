using realEstate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace realEstate
{
    public class Context : DbContext
    {
        public DbSet<REIProperty> REIProperties { get; set; }
    }
}