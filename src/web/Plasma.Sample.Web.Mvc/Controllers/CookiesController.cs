using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plasma.Sample.Web.Mvc.Controllers
{
    public class CookiesController : Controller
    {
        //
        // GET: /Cookies/

        public ActionResult Show()
        {
            return View();
        }

        public ActionResult Set()
        {
            return View();
        }

        public ActionResult Expire()
        {
            return View();
        }

        public ActionResult SetMany()
        {
            return View();
        }

    }
}
