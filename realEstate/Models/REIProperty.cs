using realEstate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace realEstate.Models
{
    public class REIProperty
    {
        public int Id { get; set; }
        public int Zpid { get; set; }
        public AddressModel Address { get; set; }
        public int Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<string> Images { get; set; }

        public HomeDetails HomeDetails { get; set; }


    }
}