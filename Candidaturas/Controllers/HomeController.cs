using System.Web.Mvc;

namespace Candidaturas.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (Session["SelectedTab"] == null)
            {
                Session["SelectedTab"] = 1;
            }

            return View();
        }
    }
}