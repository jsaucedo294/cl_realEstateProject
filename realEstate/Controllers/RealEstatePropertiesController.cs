﻿using RealEstatePropertyShared;
using RealEstatePropertyShared.Data;
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

            var propertiesFromAPI = ProcessData.GetPropertiesDetailsForSale();
            foreach (var property in propertiesFromAPI)
            {
                if (_reiPropertiesRepository.Get(property.Zpid) == null)
                {
                    _reiPropertiesRepository.Add(property);
                }

            }
            
            var reiProperties = _reiPropertiesRepository.GetList();


            return View(reiProperties);
        }

    }
}