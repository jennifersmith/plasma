using System.Web.Mvc;

namespace Plasma.Sample.Web.Mvc.Controllers
{
    public class AllTheVerbsController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Post(Model data)
        {
            return View(data);
        }

        [HttpPut]
        public ActionResult Put(Model data)
        {
            return View(data);
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPatch]
        public ActionResult Patch(Model data)
        {
            return View(data);
        }

        [HttpHead]
        public ActionResult Head()
        {
            return View();
        }

        [HttpOptions]
        public ActionResult Options()
        {
            return View();
        }

        public ActionResult Trace()
        {
            return View();
        }
    }

    public class Model
    {
        public string Value { get; set; }
    }
}