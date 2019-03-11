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
        public ActionResult Authorize(Login model)
        {

            using (CandidaturaDBEntities1 db = new CandidaturaDBEntities1())
            {
                //TODO Tratar cado de password vir null
                byte[] hashedUserPassword = new byte[0];

                using (SHA256 mySHA256 = SHA256.Create())
                {
                    hashedUserPassword = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(model.passwordInput));
                }
                Edicao edicao = db.Edicaos.Where(e => e.DataInicio < System.DateTime.Now && e.DataFim > System.DateTime.Now).First();
                var userDetails = db.Users.Where(x => x.Email == model.user.Email && x.Password == hashedUserPassword && x.Edicao == edicao.Sigla).FirstOrDefault();

                if (userDetails == null)
                {
                    TempData["LogError"] = "Email não está registado ou palavra-passe está errada.";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    
                    Session["userID"] = userDetails.ID;
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