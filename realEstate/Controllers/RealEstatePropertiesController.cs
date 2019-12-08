using RealEstatePropertyShared.Data;
using RealEstatePropertyShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace realEstate.Controllers
{
    public class RealEstatePropertiesController : BaseController
    {
        private REIPropertiesRepository _reiPropertiesRepository = null;

        public RealEstatePropertiesController()
        {
            _reiPropertiesRepository = new REIPropertiesRepository(Context);
        }

        // GET: REIProperties
        public ActionResult Index()
        {
            //TODO: Add input and submit btn to index to enter zipcode to search on database first. if 0 on database then call the api. 

            return View();
        }
        [HttpPost]
        public ActionResult PropertiesForSale(string input)
        {
            var pattern = @"\d{5}";
            var regEx = new Regex(pattern);

            MatchCollection matchedZipcode = regEx.Matches(input);
            if (!string.IsNullOrEmpty(input) && matchedZipcode.Count > 0)
            {

                

                var zipCodeValue = matchedZipcode[0].Value;



                if (zipCodeValue == "" || zipCodeValue == null)
                {
                    var propertiesList = _reiPropertiesRepository.GetList();
                    return View(propertiesList);
                }


                var propertiesByZipcode = _reiPropertiesRepository.GetList(int.Parse(zipCodeValue));

                if (propertiesByZipcode.Count <= 3)
                {

                    // GET DATA FROM API
                    var listOfAddresses = ProcessData.GetListOfAddresses(zipCodeValue);
                    if (listOfAddresses.Count > 0 )
                    { 
                        var getZpidAndPrices = ProcessData.GetZpidAndPrices(listOfAddresses);
                        var propertiesList = ProcessData.GetPropertiesDetailsForSale(getZpidAndPrices);

                        foreach (var property in propertiesList)
                        {
                            if (!_reiPropertiesRepository.doesExist(property.Zpid))
                            {
                                _reiPropertiesRepository.Add(property);
                            }
                        }

                    }
                    var propertiesByZipcode2 = _reiPropertiesRepository.GetList(int.Parse(zipCodeValue));
                    return View(propertiesByZipcode2);

                }
                else
                {
                    return View(propertiesByZipcode);
                }
            }
            else
            {
                var propertiesList = new List<RealEstateProperty>();
                return View(propertiesList);
            }

        }
        public ActionResult Details(int zpid)
        {
            var property = _reiPropertiesRepository.Get(zpid);
            return View(property);
        }

        [HttpPost]
        public ActionResult SaveProperty(string zpid)
        {

            var property = _reiPropertiesRepository.Get(int.Parse(zpid));

            if (property.IsSaved == true)
            {
                property.IsSaved = false;
            }
            else
            {
                property.IsSaved = true;
            }
           

            _reiPropertiesRepository.Update(property);
            return RedirectToAction("ViewSavedProperties");
        }

        public ActionResult FilteredProperties(int zipcode, double? priceRangeMin, int priceRangeMax, int bedroomsNum, int bathroomsNum, int sqrfLandMin, int sqrfLandMax ,int lotSizeMin, int lotSizeMax)
        {
            var propertiesList = _reiPropertiesRepository.GetList(zipcode);

            var sortedList = propertiesList;

            var sortedProperties = sortedList.AsQueryable();

            if (priceRangeMin != 0)
            {
                sortedProperties = sortedProperties.Where(p => p.Price >= priceRangeMin);
            }

            if (priceRangeMax != 0)
            {
                sortedProperties = sortedProperties.Where(p => p.Price <= priceRangeMax);
            }

            if (bedroomsNum != 0)
            {
                sortedProperties = sortedProperties.Where(p => p.Bedrooms >= bedroomsNum);
            }

            if (bathroomsNum != 0)
            {
                sortedProperties = sortedProperties.Where(p => p.Bathrooms >= bathroomsNum);
            }

            if (sqrfLandMin != 0)
            {
                sortedProperties = sortedProperties.Where(p => p.FinishedSqFt >= sqrfLandMin);
            }

            if (sqrfLandMax != 0)
            {
                sortedProperties = sortedProperties.Where(p => p.FinishedSqFt <= sqrfLandMax);
            }

            if (lotSizeMin != 0)
            {
                sortedProperties = sortedProperties.Where(p => p.LotSizeSqFt >= lotSizeMin);
            }

            if (lotSizeMax != 0)
            {
                sortedProperties = sortedProperties.Where(p => p.LotSizeSqFt <= lotSizeMax);
            }


            List<RealEstateProperty> properties = sortedProperties.OrderBy(p => p.Price).ToList();

            return View("PropertiesForSale", properties);
        }
        public ActionResult ViewSavedProperties()
        {

            var propertiesList = _reiPropertiesRepository.GetList();
            var savedProperties = propertiesList.Where(p => p.IsSaved == true).ToList();

            return View(savedProperties);
        }

        [HttpPost]
        public ActionResult DeleteProperty(string id)
        {
            _reiPropertiesRepository.Delete(int.Parse(id));

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult EditProperty(int zpid, double downPaymentPercentage, double rateOfInterest, int numOfYearsToPay, double InitialRepair)
        {
            var property = _reiPropertiesRepository.Get(zpid);
            property.DownPaymentPercent = downPaymentPercentage;
            property.RateOfInterestPercentage = rateOfInterest;
            property.NumOfYearsToPayLoan = numOfYearsToPay;
            property.InitialRepair = InitialRepair;

            _reiPropertiesRepository.Update(property);
            return View("Details", property);
        }
        
    }
}