using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateProject.Models
{
    public class Property
    {
        public string StreetAddress { get; set; }
        public byte ZipCode { get; set; }
        public int Price { get; set; }
        public byte Bedrooms { get; set; }
        public byte Bathrooms { get; set; }
    }
}
