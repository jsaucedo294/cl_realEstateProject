using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RealEstatePropertyShared.Models;

namespace RealEstatePropertyShared.Data
{
    public class REIPropertiesRepository : BaseRepository<REIProperty>
    {
        public REIPropertiesRepository(Context context)
            : base(context)
        {
        }

        public override IList<REIProperty> GetList()
        {
            return Context.REIProperties
                .OrderBy(p => p.UpdatedPropertyDetails.response.editedFacts.price)
                .ToList();
        }
     

        public override REIProperty Get(int id)
        {
            var reiProperty = Context.REIProperties.AsQueryable();
            

            return reiProperty
                .Where(p => p.UpdatedPropertyDetails.response.zpid == id.ToString())
                .SingleOrDefault();
        }

    }
}
