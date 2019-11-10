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

                ApiHelper.InitializeClient("json");
                var propertyInfo = PropertyProcessor.GetPropertiesForSale();
     
                
                return View(propertyInfo);

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