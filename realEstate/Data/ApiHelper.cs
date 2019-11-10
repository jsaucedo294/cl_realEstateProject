using realEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace realEstate.Data
{
    public static class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient(string responseFormat) 
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            if (responseFormat == "json")
            {
                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ApiClient.DefaultRequestHeaders.Add("apikey", "58ae5dead96d68af9d39b81ac366a6ed");

            }
            else if (responseFormat == "xml")
            {
                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                ApiClient.DefaultRequestHeaders.Add("zws-id", "ZWz1hgn3c1uknf_7yjzz");
            }

        }
     
    }
}