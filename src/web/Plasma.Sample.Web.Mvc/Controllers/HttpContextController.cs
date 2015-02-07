using System;
using System.Web.Mvc;

namespace Plasma.Sample.Web.Mvc.Controllers
{
    public class HttpContextController : Controller
    {
        public ActionResult GetHttpContextCurrentUser()
        {
            var user = HttpContext.User;

            if (user == null)
            {
                throw new Exception("User was null");
            }

            return View();
        }
    }
}