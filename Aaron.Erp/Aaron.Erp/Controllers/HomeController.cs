﻿using Aaron.Common;
using Aaron.Erp.App_Start.Filter;
using Aaron.Models;
using Aaron.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aaron.Erp.Controllers
{
    public class HomeController : Controller
    {
        private ILogHelper logService;

        public static int Age = 1;
        public HomeController(ILogHelper serService)
        {
            logService = serService;
        }

        [IgnoreFilter]
        public ActionResult Index(BaseModel model)
        {
            logService.Info("haha");
            return View();
        }

        public string TestFunc(string name)
        {
            
            return name;
        }
    }
}

