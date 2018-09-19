using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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