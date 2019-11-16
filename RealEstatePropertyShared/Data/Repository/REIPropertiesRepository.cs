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

        public void Add(List<RealEstateProperty> properties)
        {
            foreach (var property in properties)
            {
                Context.Set<RealEstateProperty>().Add(property);
                Context.SaveChanges();
            }
        }
        

        public override RealEstateProperty Get(int id)
        {
            var reiProperty = Context.REIProperties.AsQueryable();
            

            return reiProperty
                .Where(p => p.Zpid == id)
                .SingleOrDefault();
        }

        public override IList<RealEstateProperty> GetList()
        {
            return Context.REIProperties.OrderBy(p => p.Zpid).ToList();
        }
    }
}
