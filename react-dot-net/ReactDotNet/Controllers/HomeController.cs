using System.Web.Mvc;

namespace ReactDotNet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Home.cshtml");
        }
    }
}