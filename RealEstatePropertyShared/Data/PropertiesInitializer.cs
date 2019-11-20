using RealEstatePropertyShared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstatePropertyShared.Data
{
    internal class PropertiesInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            var propertiesFromAPI = ProcessData.GetPropertiesDetailsForSale();
            
            propertiesFromAPI.ForEach( p => context.RealEstateProperties.Add(p));

            context.SaveChanges();

        }

    }
}
