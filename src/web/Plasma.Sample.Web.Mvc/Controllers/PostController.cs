using System.Web.Mvc;

namespace Plasma.Sample.Web.Mvc.Controllers
{
    public class PostController : Controller
    {
        [HttpPost]
        public ActionResult Data(PostResponseModel data)
        {
            return View(data);
        }
    }

    public class PostResponseModel
    {
        public string Value { get; set; }
    }
}