using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clRealEstateProject.Models
{
    public class Property
    {
        public string Street { get; set; }
        public int Zipcode { get; set; }
        public int Price { get; set; }
        public byte Bedrooms { get; set; }
        public byte Bathrooms { get; set; }
    }
}
