using Candidaturas.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

            using (CandidaturaDBEntities db = new CandidaturaDBEntities())
            {
                //TODO Tratar cado de password vir null
                byte[] hashedUserPassword = new byte[0];

                using (SHA256 mySHA256 = SHA256.Create())
                {
                    hashedUserPassword = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(userModel.PasswordInput));
                }

                var userDetails = db.Users.Where(x => x.Email == userModel.Email && x.Password == hashedUserPassword).FirstOrDefault();

                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Username ou password errado.";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    Session["userID"] = userDetails.ID;
                    return RedirectToAction("Welcome", "Home");
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