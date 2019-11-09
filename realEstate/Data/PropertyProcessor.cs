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
        public static async Task<REIProperty> GetPropertiesForSale()
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


            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    PropertyModel property = await response.Content.ReadAsAsync<PropertyModel>();
                    return property.Property;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }


        }
    }
}