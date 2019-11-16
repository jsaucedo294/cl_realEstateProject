using RealEstatePropertyShared;
using RealEstatePropertyShared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace realEstate.Controllers
{
    public class REIPropertiesController : BaseController
    {
        private REIPropertiesRepository _reiPropertiesRepository = null;

        public REIPropertiesController()
        {
            _reiPropertiesRepository = new REIPropertiesRepository(Context);
        }

        // GET: REIProperties
        public ActionResult Index()
        {
        
            _reiPropertiesRepository.Add(ProcessData.GetPropertiesDetailsForSale());
            var reiProperties = _reiPropertiesRepository.GetList();


            return View(reiProperties);
        }

    }
}