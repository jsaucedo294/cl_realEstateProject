using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace realEstate.Data
{
    public class AddressModel
    {
        public string Street { get; set; }
        public int Zipcode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public int Latitude { get; set; }
        public int Longitude { get; set; }

    }
}