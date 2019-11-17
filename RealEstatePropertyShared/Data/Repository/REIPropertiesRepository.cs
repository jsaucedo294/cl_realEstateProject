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
            /* 
             TODO: Fix error
             System.Data.SqlClient.SqlException: 'CREATE FILE encountered operating system error 5(Access is denied.) while attempting to open or create the physical file 
             'C:\Users\Jorge SaucedoRealEstatePropertyWebApp.mdf'. CREATE DATABASE failed. Some file names listed could not be created. Check related errors.'
             */
            properties.ForEach(p => Context.Set<RealEstateProperty>().Add(p));
                Context.SaveChanges();
            
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
