﻿using Aaron.Erp.App_Start.Filter;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aaron.Erp.Controllers
{
    public class HomeController : Controller
    {
        [IgnoreFilter]
        public ActionResult Index(BaseModel model)
        {
            return View();
        }
    }
}
