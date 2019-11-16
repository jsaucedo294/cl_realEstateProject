using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstatePropertyShared.Models
{
    public class RealEstateProperty
    {
        [Key]
        public int? Zpid { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int? Price { get; set; }
        public string PropertyType { get; set; }
        public int? Bedrooms { get; set; }
        public double? Bathrooms { get; set; }
        public int? FinishedSqFt { get; set; }
        public int? LotSizeSqFt { get; set; }
        public int? NumRooms { get; set; }
        public string Roof { get; set; }
        public string ExterialMaterial { get; set; }
        public string ParkingType { get; set; }
        public string HeatingSystem { get; set; }
        public string CoolingSystem { get; set; }
        public string FloorCovering { get; set; }
        public string Architecture { get; set; }
        public string Basement { get; set; }
        public string Appliances { get; set; }
        public int? NumFloors { get; set; }
        public List<string> Images { get; set; }
        public string HomeDescription { get; set; }



    }
}
