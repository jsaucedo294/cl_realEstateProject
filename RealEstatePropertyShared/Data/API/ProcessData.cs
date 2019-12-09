using System;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using RealEstatePropertyShared.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;

namespace RealEstatePropertyShared.Data
{
    public static class ProcessData
    {

        public static List<Dictionary<string, string>> GetListOfAddresses(string zipcode)
        {
            /*
           *&city=louisville&childtype=neighborhood
           */
            List<Dictionary<string, string>> addressesForSale = new List<Dictionary<string, string>>();

            var builder = new UriBuilder("https://api.gateway.attomdata.com/propertyapi/v1.0.0/property/address");

            var query = HttpUtility.ParseQueryString(builder.Query);
                
           

            if (zipcode != "")
            {
                query["postalcode"] = zipcode;
            }
            else
            {
                query["address1"] = "320 IDLEWYLDE DR";
                query["address2"] = "318 IDLEWYLDE DR";
                query["radius"] = "20";
            }
            
            query["propertytype"] = "SFR";
            query["page"] = "1";
            query["pagesize"] = "10";
            query["orderby"] = "publisheddate Desc";
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
                    var noAddressesReturn = new List<Dictionary<string, string>>();
                    return noAddressesReturn;
                }
            }


        }

        public static List<GetSearchResults> GetZpidAndPrices(List<Dictionary<string, string>> listOfAddresses)
        {
            List<GetSearchResults> propertyList = new List<GetSearchResults>();
            

            ApiHelper.InitializeClient("xml");

            var builder = new UriBuilder("http://www.zillow.com/webservice/GetSearchResults.htm?");
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var property in listOfAddresses)
            {
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

                       
                        var propertyObj = JsonConvert.DeserializeObject<GetSearchResults>(jsonText);

                        var result = propertyObj.SearchResults.response?.results.result;


                        // Populate List with  and price amount Dictionary
                        if (result != null)
                        {
                            propertyList.Add(propertyObj);
                        }

                           
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }

            }
            return propertyList;
        }

        public static List<RealEstateProperty> GetPropertiesDetailsForSale(List<GetSearchResults> listOfProperties)
        {
            List<RealEstateProperty> properties = new List<RealEstateProperty>();

            var builder = new UriBuilder("http://www.zillow.com/webservice/GetUpdatedPropertyDetails.htm?");
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var property in listOfProperties)
            {
                var firstResult = property.SearchResults.response.results.result.First();
                var lastResult = property.SearchResults.response.results.result.Last();

                query["zws-id"] = APIKeys.ZillowKey;
                query["zpid"] = firstResult != null ? firstResult.zpid : lastResult.zpid;
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
                            
                            if (!xmlResponse.Contains("<count>1</count>"))
                            {

                            
                            var reiProperty = JsonConvert.DeserializeObject<REIProperty>(jsonText);
                            
                            var amount = 0.0;
                            var price = 0.0;
                            if (double.TryParse(firstResult.zestimate.amount.text, out amount))
                            {
                                 price = amount;
                            }

                            if (reiProperty.UpdatedPropertyDetails.response != null)
                            { 

                                var images = reiProperty.UpdatedPropertyDetails.response.images?.image.url;
                                List<string> defaultImage = new List<string>() { "/Content/Images/No_photos_available.png" };

                                //TODO: Images are not being added to RealEstateProperty Model.
                                // check -> https://stackoverflow.com/questions/20711986/entity-framework-code-first-cant-store-liststring 
                            

                                var zpid = checkIfCorrectNumberInput(reiProperty.UpdatedPropertyDetails.response?.zpid);
                            
                                var street = reiProperty.UpdatedPropertyDetails.response.address.street;
                                var city = reiProperty.UpdatedPropertyDetails.response.address.city;
                                var state = reiProperty.UpdatedPropertyDetails.response.address.state;
                                var zipcodeAdded = checkIfCorrectNumberInput(reiProperty.UpdatedPropertyDetails.response.address.zipcode);
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
                                    Zipcode = zipcodeAdded,
                                    Latitude = latitude,
                                    Longitude = longitude,
                                    Price = price,
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
                                    Images = ((images == null) ? defaultImage : images),
                                    HomeDescription = homeDescription
                                };

                                //TODO: Only add RealEstatePropery when is not in database
                                properties.Add(_property);
                            }
                            }
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