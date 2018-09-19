using Candidaturas.Models;
using System.Linq;
using System.Web.Mvc;

namespace Candidaturas.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Candidaturas.Models.User userModel)
        {
            using(LoginDataBaseEntities1 db = new LoginDataBaseEntities1())
            {
                var userDetails = db.Users.Where(x => x.Email == userModel.Email && x.Password == userModel.Password).FirstOrDefault();

                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Username ou password errado.";
                    return View("Index", userModel);
                }
                else
                {
                    Session["userID"] = userDetails.ID;
                    Session["userName"] = userDetails.NomeCompleto;
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}