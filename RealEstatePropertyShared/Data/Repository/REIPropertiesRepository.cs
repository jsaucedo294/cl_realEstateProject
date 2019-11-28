using System;
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

        public override RealEstateProperty Get(int zpid)
        {
            var reiProperty = Context.RealEstateProperties.AsQueryable();
            


            return reiProperty
                .Where(p => p.Zpid == zpid)
                .SingleOrDefault();
        }

      
        public override IList<RealEstateProperty> GetList(int zipcode)
        {
            return Context.RealEstateProperties.Where(p => p.Zipcode == zipcode).ToList();
        }
        public override IList<RealEstateProperty> GetList()
        {
            return Context.RealEstateProperties.OrderBy(p => p.Zipcode).ToList();
        }

        public bool doesExist(int zipcode)
        {
            return Context.RealEstateProperties.Any(p => p.Zipcode == zipcode);
        }

    }
}
