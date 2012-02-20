using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plasma.Sample.Web.Mvc.Controllers
{
    public class FormWithCheckBoxController : Controller
    {
        //
        // GET: /FormWithCheckBox/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(bool checkBox)
        {
            ViewData["value"] = checkBox.ToString();
            return View("Index");
        }
    }
}
