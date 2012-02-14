using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Plasma.Sample.Web.Mvc.Controllers
{
    public class GotoPageController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ThreeOhOne()
        {
            return new ThreeOhOneRedirectResult("Default", new RouteValueDictionary {{"Controller", "GotoPage"}, {"Action", "Index"}});
        }

        public ActionResult ThreeOhTwo()
        {
            return RedirectToAction("Index");
        }
    }

    public class ThreeOhOneRedirectResult : RedirectToRouteResult
    {
        public ThreeOhOneRedirectResult(RouteValueDictionary routeValues) : base(routeValues)
        {
        }

        public ThreeOhOneRedirectResult(string routeName, RouteValueDictionary routeValues) : base(routeName, routeValues)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);
            context.HttpContext.Response.StatusCode = 301;
        }
    }
}
