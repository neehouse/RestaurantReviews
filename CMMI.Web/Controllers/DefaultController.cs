using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMMI.Web.Controllers
{
    public class DefaultController : Controller
    {
        public ViewResult Index(string path)
        {
            return View("~/default.cshtml");
        }
    }
}