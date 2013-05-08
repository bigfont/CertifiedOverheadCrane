using System.Web.Mvc;
using Orchard.Themes;

namespace BigFont.DealerDashboard.Controllers
{
    [Themed]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("HelloWorld");
        }
    }
}