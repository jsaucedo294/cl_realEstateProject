using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace realEstate.Data
{
    public class HomeDetailsModel
    {
        public string PropertyType { get; set; }
        public byte Bedrooms { get; set; }
        public byte Bathrooms { get; set; }
        public int FinishedSqFt { get; set; }
        public int LotSizeSqFt { get; set; }
        public int YearBuilt { get; set; }
        public byte NumFloors { get; set; }

        public string BasementCondition { get; set; }
        public string RoofType { get; set; }
        public string ExteriorMaterial { get; set; }

        public string HeatingSystem { get; set; }
        public string CoolingSystem { get; set; }
        public string Appliances { get; set; }

        public string HomeDescription { get; set; }

        public string WhatOwnerLoves { get; set; }


    }
}