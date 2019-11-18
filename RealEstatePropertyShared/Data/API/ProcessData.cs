using System;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using RealEstatePropertyShared.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;

namespace RealEstatePropertyShared.Data
{
    public static class ProcessData
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
            

            ApiHelper.InitializeClient("json");
            var propertyInfo = ProcessData.GetPropertiesForSale();

            ApiHelper.InitializeClient("xml");

            var builder = new UriBuilder("http://www.zillow.com/webservice/GetSearchResults.htm?");
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var property in propertyInfo)
            {
                Dictionary<string, int> PropertyZpidAndPrice = new Dictionary<string, int>();
                query["zws-id"] = APIKeys.ZillowKey;
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

                       
                        var resultObj = JsonConvert.DeserializeObject<GetSearchResults>(jsonText);

                        var result = resultObj.SearchResults.response.results.result;
                        foreach(var item in result)
                            {


                            var amount = 0;
                            if (int.TryParse(item.zestimate.amount.text, out amount))
                            {
                                // Populate zpid location on objResult and price amount

                                PropertyZpidAndPrice.Add("priceAmount", amount);
                                PropertyZpidAndPrice.Add("zpid", int.Parse(item.zpid));
                            }
                        }
                        


                        if (PropertyZpidAndPrice.Count > 0)
                        {
                            // Populate List with  and price amount Dictionary

                            zpidOfPropertiesList.Add(PropertyZpidAndPrice);

                        }
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }

            }
            Console.WriteLine(zpidOfPropertiesList);
            return zpidOfPropertiesList;
        }

        public static List<RealEstateProperty> GetPropertiesDetailsForSale()
        {
            List<RealEstateProperty> properties = new List<RealEstateProperty>();
            ApiHelper.InitializeClient("xml");
            var listOfZpidAndPrices = ProcessData.getZpidOfProperties();


            var builder = new UriBuilder("http://www.zillow.com/webservice/GetUpdatedPropertyDetails.htm?");
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var itemDictionary in listOfZpidAndPrices)
            {
                query["zws-id"] = APIKeys.ZillowKey;
                query["zpid"] = itemDictionary["zpid"].ToString();
                builder.Query = query.ToString();

                string url = builder.ToString();

                using (var response = ApiHelper.ApiClient.GetAsync(url).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        bool isCode502 = false;
                        var xmlResponse = response.Content.ReadAsStringAsync().Result;

                        if (xmlResponse.Contains("<code>502</code>"))
                            isCode502 = true;


                        if (isCode502 != true)
                        {

                            var xmlNode = new XmlDocument();
                            xmlNode.LoadXml(xmlResponse);
                            var jsonText = JsonConvert.SerializeXmlNode(xmlNode);
                            var reiProperty = JsonConvert.DeserializeObject<REIProperty>(jsonText);

                            reiProperty.UpdatedPropertyDetails.response.editedFacts.price = itemDictionary["zpid"];

                            List<string> images = new List<string>();
                            var imagesUrls = reiProperty.UpdatedPropertyDetails.response.images.image.url;
                            foreach (var image in imagesUrls)
                            {
                                if (image != null)
                                {
                                    images.Add(image);
                                }
                                else
                                {
                                    images.Add("");
                                }
                                
                            }
                         
                             

                            var zpid = checkIfCorrectNumberInput(reiProperty.UpdatedPropertyDetails.response.zpid);
                            var street = reiProperty.UpdatedPropertyDetails.response.address.street;
                            var city = reiProperty.UpdatedPropertyDetails.response.address.city;
                            var state = reiProperty.UpdatedPropertyDetails.response.address.state;
                            var latitude = checkIfCorrectDoubleInput(reiProperty.UpdatedPropertyDetails.response.address.latitude);
                            var longitude = checkIfCorrectDoubleInput(reiProperty.UpdatedPropertyDetails.response.address.longitude);
                            var propertyType = reiProperty.UpdatedPropertyDetails.response.editedFacts.useCode;
                            var bedrooms = checkIfCorrectNumberInput(reiProperty.UpdatedPropertyDetails.response.editedFacts.bedrooms);
                            var bathdrooms = checkIfCorrectDoubleInput(reiProperty.UpdatedPropertyDetails.response.editedFacts.bathrooms);
                            var finishedSqFt = checkIfCorrectNumberInput(reiProperty.UpdatedPropertyDetails.response.editedFacts.finishedSqFt);
                            var lotSizeSqFt = checkIfCorrectNumberInput(reiProperty.UpdatedPropertyDetails.response.editedFacts.lotSizeSqFt);
                            var numRooms = checkIfCorrectNumberInput(reiProperty.UpdatedPropertyDetails.response.editedFacts.numRooms);
                            var roof = reiProperty.UpdatedPropertyDetails.response.editedFacts.roof;
                            var exteriorMaterial = reiProperty.UpdatedPropertyDetails.response.editedFacts.exteriorMaterial;
                            var parkingType = reiProperty.UpdatedPropertyDetails.response.editedFacts.parkingType;
                            var heatingSystem = reiProperty.UpdatedPropertyDetails.response.editedFacts.heatingSystem;
                            var coolingSystem = reiProperty.UpdatedPropertyDetails.response.editedFacts.coolingSystem;
                            var floorCovering = reiProperty.UpdatedPropertyDetails.response.editedFacts.floorCovering;
                            var architecture = reiProperty.UpdatedPropertyDetails.response.editedFacts.architecture;
                            var basement = reiProperty.UpdatedPropertyDetails.response.editedFacts.basement;
                            var numFloors = checkIfCorrectNumberInput(reiProperty.UpdatedPropertyDetails.response.editedFacts.numFloors);
                            var homeDescription = reiProperty.UpdatedPropertyDetails.response.homeDescription;

                            RealEstateProperty _property = new RealEstateProperty()
                            {
                                Zpid = zpid,
                                Street = street,
                                City = city,
                                State = state,
                                Latitude = latitude,
                                Longitude = longitude,
                                Price = reiProperty.UpdatedPropertyDetails.response.editedFacts.price,
                                PropertyType = propertyType,
                                Bedrooms = bedrooms,
                                Bathrooms = bathdrooms,
                                FinishedSqFt = finishedSqFt,
                                LotSizeSqFt = lotSizeSqFt,
                                NumRooms = numRooms,
                                Roof = roof,
                                ExterialMaterial = exteriorMaterial,
                                ParkingType = parkingType,
                                HeatingSystem = heatingSystem,
                                CoolingSystem = coolingSystem,
                                FloorCovering = floorCovering,
                                Architecture = architecture,
                                Basement = basement,
                                Appliances = reiProperty.UpdatedPropertyDetails.response.editedFacts.appliances,
                                NumFloors = numFloors,
                                Images = images,
                                HomeDescription = homeDescription
                            };

                            properties.Add(_property);
                        }
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }

            }
            return properties;
        }

        public static int checkIfCorrectNumberInput(string value) 
        {
            int number;
            bool success = Int32.TryParse(value, out number);
            if (success)
            {
                return number;
            }
            else 
            {
                return 0;
            }

        
        }

        public static double checkIfCorrectDoubleInput(string value)
        {
            double doubleNum;
            bool successDec = Double.TryParse(value, out doubleNum);
            if (successDec)
            {
                return doubleNum;
            }
            else
            {
                return 0.0;
            }
        }
    }
}