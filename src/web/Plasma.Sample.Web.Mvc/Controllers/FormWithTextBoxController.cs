using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plasma.Sample.Web.Mvc.Controllers
{
    public class FormWithTextBoxController : Controller
    {
        //
        // GET: /FormWithTextBox/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Show(string textBox)
        {
            ViewData["value"] = textBox;
            return View("Index");
        }
    }
}
