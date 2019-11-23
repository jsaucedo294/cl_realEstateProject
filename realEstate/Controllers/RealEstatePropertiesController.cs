using RealEstatePropertyShared;
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
            
            var reiProperties = _reiPropertiesRepository.GetList();


            return View(reiProperties);
        }
        public ActionResult Details(int zpid)
        {
            var property = _reiPropertiesRepository.Get(zpid);
            return View(property);
        }

    }
}