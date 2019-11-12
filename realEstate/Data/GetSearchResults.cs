using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace realEstate.Data
{
    public class GetSearchResults
    {
        [JsonProperty("SearchResults:searchresults")]
        public SearchResultsSearchresults SearchResults { get; set; }
    }
    public class Request
    {
        public string address { get; set; }
        public string citystatezip { get; set; }
    }

    public class Message
    {
        public string text { get; set; }
        public string code { get; set; }
    }

    public class Links
    {
        public string homedetails { get; set; }
        public string graphsanddata { get; set; }
        public string mapthishome { get; set; }
        public string comparables { get; set; }
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

    public class Amount
    {
        [JsonProperty("@currency")]
        public string currency { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }


    public class Low
    {
        [JsonProperty("@currency")]
        public string currency { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class High
    {
        [JsonProperty("@currency")]
        public string currency { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class ValuationRange
    {
        public Low low { get; set; }
        public High high { get; set; }
    }

    public class Zestimate
    {
        public Amount amount { get; set; }
        public string lastUpdated { get; set; }
        public object valueChange { get; set; }
        public ValuationRange valuationRange { get; set; }
        public string percentile { get; set; }
    }

    public class Result
    {
        public string zpid { get; set; }
        public Links links { get; set; }
        public Address address { get; set; }
        public Zestimate zestimate { get; set; }
    }

    public class Results
    {
        [JsonConverter(typeof(SingleValueArrayConverter<Result>))]
        public List<Result> result { get; set; }
    }

    public class Response
    {
        public Results results { get; set; }
    }

    public class SearchResultsSearchresults
    {
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