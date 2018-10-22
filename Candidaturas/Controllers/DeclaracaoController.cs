using System.Web.Mvc;

namespace Candidaturas.Controllers
{
    public class DeclaracaoController : Controller
    {
        // GET: Declaracao
        public ActionResult Index()
        {
            return View("~/Views/Declaracao/Index.cshtml");
        }
    }
}