using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plasma.Sample.Web.Mvc.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index(string message)
        {
            ViewData["Message"] = message;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
