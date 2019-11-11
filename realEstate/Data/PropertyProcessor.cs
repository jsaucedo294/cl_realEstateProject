using System;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using realEstate.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;

namespace realEstate.Data
{
    public static class PropertyProcessor
    {
        public static List<Dictionary<string, string>> GetPropertiesForSale()
        {
            /*
           *&city=louisville&childtype=neighborhood
           */
            List<Dictionary<string, string>> addressesForSale = new List<Dictionary<string, string>>();

            var builder = new UriBuilder("https://api.gateway.attomdata.com/propertyapi/v1.0.0/property/address");

            var query = HttpUtility.ParseQueryString(builder.Query);
            query["address1"] = "320 IDLEWYLDE DR";
            query["address2"] = "2825 LEXINGTON RD";
            query["radius"] = "20";
            query["propertytype"] = "DUPLEX";
            query["orderby"] = "publisheddate";
            query["page"] = "1";
            query["pagesize"] = "5";
            builder.Query = query.ToString();
            string url = builder.ToString();

            ApiHelper.InitializeClient("json");
            using (var response = ApiHelper.ApiClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;

                    JObject objResult = JObject.Parse(result);

                    foreach (var property in objResult["property"])
                    {
                        string line1 = property["address"]["line1"].ToString();
                        string line2 = property["address"]["line2"].ToString();

                        Dictionary<string, string> propertyAddress = new Dictionary<string, string>();
                        propertyAddress.Add("address", line1);
                        propertyAddress.Add("citystatezip", line2);

                        addressesForSale.Add(propertyAddress);
                    }
                   
                    return addressesForSale;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }


        }

        public static List<Dictionary<string, int>> getZpidOfProperties()
        {
            List<Dictionary<string, int>> zpidOfPropertiesList = new List<Dictionary<string, int>>();
            Dictionary<string, int> PropertyZpidAndPrice = new Dictionary<string, int>();

            ApiHelper.InitializeClient("json");
            var propertyInfo = PropertyProcessor.GetPropertiesForSale();

            ApiHelper.InitializeClient("xml");

            var builder = new UriBuilder("http://www.zillow.com/webservice/GetSearchResults.htm?");
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var property in propertyInfo)
            {
                query["zws-id"] = "X1-ZWz1hgn3c1uknf_7yjzz";
                query["address"] = property["address"];
                query["citystatezip"] = property["citystatezip"];

                builder.Query = query.ToString();
                string url = builder.ToString();

                using (var response = ApiHelper.ApiClient.GetAsync(url).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var xmlResponse = response.Content.ReadAsStringAsync().Result;
                        var xmlNode = new XmlDocument();
                        xmlNode.LoadXml(xmlResponse);
                        // Find a way I can deserialize xml to an object, so I'm able to access to zpid and price amount
                        var jsonText = JsonConvert.SerializeXmlNode(xmlNode);
                        
                        JObject objResult = JObject.Parse(jsonText);

                        // Populate zpid location on objResult and price amount

                        int zpidProperty = int.Parse(objResult["SearchResults:searchresults"]["response"]["results"]["result"]["zpid"].ToString());
                        int priceAmount = int.Parse(objResult["SearchResults:searchresults"]["response"]["results"]["result"]["zestimate"]["amount"]["#text"].ToString());
                        PropertyZpidAndPrice.Add("zpid", zpidProperty);
                        PropertyZpidAndPrice.Add("priceAmount", priceAmount);

                        // Populate List with zpid and price amount Dictionary
                        zpidOfPropertiesList.Add(PropertyZpidAndPrice);
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }

            }

            return zpidOfPropertiesList;
        }

        public static List<REIProperty> GetPropertiesDetailsForSale()
        {
            List<REIProperty> properties = new List<REIProperty>();
            ApiHelper.InitializeClient("xml");
            var zpids = PropertyProcessor.getZpidOfProperties();

            var builder = new UriBuilder("http://www.zillow.com/webservice/GetUpdatedPropertyDetails.htm?");
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var zpid in zpids)
            {
                query["zpid"] = zpid["zpid"].ToString();
                builder.Query = query.ToString();

                string url = builder.ToString();

                using (var response = ApiHelper.ApiClient.GetAsync(url).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        //TODO: I need to get the zpids from Zillow so I can get the property data from the other Zillow url that requires zpid.
                        // Use the code example that Matt gave you in Slack. You need to convert the xml and get the zpids.
                        var xmlResponse = response.Content.ReadAsStringAsync().Result;
                        var xmlNode = new XmlDocument();
                        xmlNode.LoadXml(xmlResponse);
                        var jsonText = JsonConvert.SerializeXmlNode(xmlNode);
                        Console.WriteLine(jsonText);
                        JObject objResult = JObject.Parse(jsonText);

                        REIProperty property = new REIProperty();
                      

                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }

            }
            return properties;
        }
    }
}