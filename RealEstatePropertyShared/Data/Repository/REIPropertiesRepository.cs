﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RealEstatePropertyShared.Models;

namespace RealEstatePropertyShared.Data
{
    public class REIPropertiesRepository : BaseRepository<RealEstateProperty>
    {
        public REIPropertiesRepository(Context context)
            : base(context)
        {
        }

        public override RealEstateProperty Get(int id)
        {
            var reiProperty = Context.RealEstateProperties.AsQueryable();
            

            return reiProperty
                .Where(p => p.Zpid == id)
                .SingleOrDefault();
        }

        public override IList<RealEstateProperty> GetList()
        {
            return Context.RealEstateProperties.OrderBy(p => p.Zpid).ToList();
        }
    }
}
