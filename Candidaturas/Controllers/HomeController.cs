using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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