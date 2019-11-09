﻿using realEstate.Data;
using realEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace realEstate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new Context())
            {

                var propertyInfo = PropertyProcessor.GetPropertiesForSale().Result;
                
                context.REIProperties.Add(propertyInfo);
                

                var reiProperties = context.REIProperties.ToList();

                
                return View(reiProperties);
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}