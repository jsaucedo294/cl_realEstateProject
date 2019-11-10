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
        public AddressModel Address { get; set; }

    }
}