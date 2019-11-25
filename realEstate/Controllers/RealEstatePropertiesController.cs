using RealEstatePropertyShared;
using RealEstatePropertyShared.Data;
using RealEstatePropertyShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult PropertiesForSale(string zipcode)
        {
            if (zipcode == "" || zipcode == null)
            {
               var propertiesList = _reiPropertiesRepository.GetList();
                return View(propertiesList);
            }

            var propertiesByZipcode = _reiPropertiesRepository.GetList(int.Parse(zipcode));

            if (propertiesByZipcode.Count <= 3)
            {

                var propertiesList = ProcessData.GetPropertiesDetailsForSale(zipcode);

                foreach (var property in propertiesList)
                {
                    if (!_reiPropertiesRepository.doesExist(property.Zpid))
                    {
                        _reiPropertiesRepository.Add(property);
                    }
                }

                var propertiesByZipcode2 = _reiPropertiesRepository.GetList(int.Parse(zipcode));
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

        public ActionResult ViewProperties()
        {

            var propertiesList = _reiPropertiesRepository.GetList();

            return View(propertiesList);
        }
    }
}