using System;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using realEstate.Models;
using System.Collections.Generic;

namespace realEstate.Data
{
    public static class PropertyProcessor
    {
        public static List<REIProperty> GetPropertiesForSale()
        {
            /*
           *&city=louisville&childtype=neighborhood
           */
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
                    PropertyModel property = response.Content.ReadAsAsync<PropertyModel>().Result;
                    return property.Property;
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
                query["address"] = property.Address.Line1;
                query["citystatezip"] = $"{ property.Address.Locality}, { property.Address.CountrySubd }";

                builder.Query = query.ToString();
                string url = builder.ToString();

                using (var response = ApiHelper.ApiClient.GetAsync(url).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        //TODO: I need to get the zpids from Zillow so I can get the property data from the other Zillow url that requires zpid.
                        // Use the code example that Matt gave you in Slack. You need to convert the xml and get the zpids.
                        PropertyModel zpid = response.Content.ReadAsAsync<PropertyModel>().Result;
                        zpidOfProperties.Add(zpid.Property);
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }

            }


            return zpidOfProperties;


            

        }
    }
}