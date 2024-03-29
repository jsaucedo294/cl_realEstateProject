﻿using Newtonsoft.Json;
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
public class Request
    {
        public string zpid { get; set; }
    }

    public class Message
    {
        public string text { get; set; }
        public string code { get; set; }
    }

    public class PageViewCount
    {
        public string currentMonth { get; set; }
        public string total { get; set; }
    }

    public class Address
    {
        public string street { get; set; }
        public string zipcode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class Links
    {
        public string homeDetails { get; set; }
        public string photoGallery { get; set; }
        public string homeInfo { get; set; }
    }

    public class Image
    {
        [JsonConverter(typeof(SingleValueArrayConverter<string>))]
        public List<string> url { get; set; }
    }

    public class Images
    {
        public string count { get; set; }
        public Image image { get; set; }
    }

    public class EditedFacts
    {
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
        public string numFloors { get; set; }
        public int price { get; set; }
        public string appliances { get; set; }

    }

    public class Response
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
        public Request request { get; set; }
        public Message message { get; set; }
        public Response response { get; set; }
    }


    public class SingleValueArrayConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object retVal = new Object();
            if (reader.TokenType == JsonToken.StartObject)
            {
                T instance = (T)serializer.Deserialize(reader, typeof(T));
                retVal = new List<T>() { instance };
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                retVal = serializer.Deserialize(reader, objectType);
            }
            return retVal;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }

}