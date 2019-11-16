using Newtonsoft.Json;
using RealEstatePropertyShared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstatePropertyShared.Models
{
    public class REIProperty
    {
        [JsonProperty("UpdatedPropertyDetails:updatedPropertyDetails")]
        public UpdatedPropertyDetailsUpdatedPropertyDetails UpdatedPropertyDetails { get; set; }
    }

    public class RequestThird
    {
        public string zpid { get; set; }
    }

    public class MessageThird
    {
        public string text { get; set; }
        public string code { get; set; }
    }

    public class PageViewCount
    {
        public string currentMonth { get; set; }
        public string total { get; set; }
    }

    public class AddressThird
    {
        public string street { get; set; }
        public string zipcode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class LinksThird
    {
        public string homeDetails { get; set; }
        public string photoGallery { get; set; }
        public string homeInfo { get; set; }
    }

    public class Image
    {
        public List<string> url { get; set; }
    }

    public class Images
    {
        public string count { get; set; }
        public Image image { get; set; }
    }

    public class EditedFacts
    {
        public int price { get; set; }
        public string useCode { get; set; }
        public string bedrooms { get; set; }
        public string bathrooms { get; set; }
        public string finishedSqFt { get; set; }
        public string lotSizeSqFt { get; set; }
        public string yearBuilt { get; set; }
        public string numRooms { get; set; }
        public string roof { get; set; }
        public string exteriorMaterial { get; set; }
        public string parkingType { get; set; }
        public string heatingSources { get; set; }
        public string heatingSystem { get; set; }
        public string coolingSystem { get; set; }
        public string floorCovering { get; set; }
        public string architecture { get; set; }

        public string basement { get; set; }
        public string appliances { get; set; }
        public string numFloors { get; set; }
    }

    public class ResponseThird
    {
        public string zpid { get; set; }
        public PageViewCount pageViewCount { get; set; }
        public Address address { get; set; }
        public Links links { get; set; }
        public Images images { get; set; }
        public EditedFacts editedFacts { get; set; }
        public string homeDescription { get; set; }
        public string schoolDistrict { get; set; }
    }

    public class UpdatedPropertyDetailsUpdatedPropertyDetails
    {
        public RequestThird request { get; set; }
        public MessageThird message { get; set; }
        public ResponseThird response { get; set; }
    }
}