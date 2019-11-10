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
                        Console.WriteLine($"{line1} {line2} has been added...");
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

        public static List<int> getZpidOfProperties()
        {
            List<int> zpidOfProperties = null;

            ApiHelper.InitializeClient("json");
            var propertyInfo = PropertyProcessor.GetPropertiesForSale();

            ApiHelper.InitializeClient("xml");

            var builder = new UriBuilder("http://www.zillow.com/webservice/GetSearchResults.htm?");
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var property in propertyInfo)
            {
                query["address"] = property["address"];
                query["citystatezip"] = property["citystatezip"];

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
                        
                       
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }

            }

            return zpidOfProperties;
        }

        public static List<REIProperty> GetPropertiesDetailsForSale()
        {
            ApiHelper.InitializeClient("xml");
            var zpids = PropertyProcessor.getZpidOfProperties();

            var builder = new UriBuilder("http://www.zillow.com/webservice/GetUpdatedPropertyDetails.htm?");
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var zpid in zpids)
            {
                query["zpid"] = zpid.ToString();
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
                        property.

                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }

            }
        }
    }
}