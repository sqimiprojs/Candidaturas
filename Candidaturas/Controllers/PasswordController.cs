using Candidaturas.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Candidaturas.Controllers
{
    public class PasswordController : Controller
    {
        CandidaturaDBEntities db = new CandidaturaDBEntities();

        // GET: PasswordRecovery
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RequestNewPassword(User userModel)
        {
            string email = userModel.Email;

            User user = db.Users.Where(u => u.Email == email).FirstOrDefault();

            if(user == null)
            {
                ViewBag.PasswordError = "O email indicado não foi encontrado no sistema.";

                return View("Index");
            }
            else
            {
                string newPassword = Password.GeneratePassword();

                using (SHA256 mySHA256 = SHA256.Create())
                {
                    user.Password = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                    db.SaveChanges();
                }                    

                string subject = "Recuperação de Password";
                string body = "A sua nova password é a seguinte: " + newPassword;

                Email.SendEmail(email, subject, body);

                ViewBag.Subtitle = "Recuperação de Password";

                ViewBag.ConfirmationMessage = "Pedido de recuperação de password efetuado com sucesso. Por favor verifique a sua caixa de email.";

                return View("~/Views/Shared/Success.cshtml");
            }
        }
    }
}