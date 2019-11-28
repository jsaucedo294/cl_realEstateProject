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
                var getZpidAndPrices = ProcessData.GetZpidAndPrices(listOfAddresses);
                var propertiesList = ProcessData.GetPropertiesDetailsForSale(getZpidAndPrices);

                foreach (var property in propertiesList)
                {
                    if (!_reiPropertiesRepository.doesExist(property.Zpid))
                    {
                        _reiPropertiesRepository.Add(property);
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

        
    }
}