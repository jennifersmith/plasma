using System.Web.Mvc;

namespace Plasma.Sample.Web.Mvc.Controllers
{
    public class Html5ElementsController : Controller
    {
        //
        // GET: /Html5Elements/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(string emailBox, string numberBox, string weekBox,
            string monthBox, string dateBox, string timeBox,
            string dateTimeBox, string rangeBox, string telBox, string urlBox)
        {
            ViewData["emailBoxValue"] = emailBox;
            ViewData["numberBoxValue"] = numberBox;
            ViewData["weekBoxValue"] = weekBox;
            ViewData["monthBoxValue"] = monthBox;
            ViewData["dateBoxValue"] = dateBox;
            ViewData["timeBoxValue"] = timeBox;
            ViewData["dateTimeBoxValue"] = dateTimeBox;
            ViewData["rangeBoxValue"] = rangeBox;
            ViewData["telBoxValue"] = telBox;
            ViewData["urlBoxValue"] = urlBox;
            return View("Index");
        }

    }
}
