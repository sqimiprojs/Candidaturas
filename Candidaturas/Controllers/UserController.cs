using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Candidaturas.Models;

namespace Candidaturas.Controllers
{
    public class UserController : Controller
    {
        public void getDataForDropdownLists()
        {
            CandidaturaDBEntities db = new CandidaturaDBEntities();

            IEnumerable<SelectListItem> tiposDocumentosId = db.TipoDocumentoIDs.Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome
            });

            ViewBag.TipoDocID = tiposDocumentosId.ToList();
        }

        [HttpGet]
        // GET: User
        public ActionResult NewUser()
        {
            User userModel = new User();
            getDataForDropdownLists();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult NewUser(User userModel)
        {
            string newPassword = Password.GeneratePassword().ToString();

            CaptchaResponse response = CaptchaValidator.ValidateCaptcha(Request["g-recaptcha-response"]);

            if (response.Success)
            {
                using (CandidaturaDBEntities dbModel = new CandidaturaDBEntities())
                {
                    if (!dbModel.Users.Any(u => u.Email == userModel.Email))
                    {
                        try
                        {
                            using (SHA256 mySHA256 = SHA256.Create())
                            {
                                userModel.Password = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                                userModel.DataCriacao = System.DateTime.Now;
                                dbModel.Users.Add(userModel);
                                /**/
                                dbModel.SaveChanges();
                            }                                
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                        {
                            Exception raise = dbEx;
                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                            {
                                foreach (var validationError in validationErrors.ValidationErrors)
                                {
                                    string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                                    raise = new InvalidOperationException(message, raise);
                                }
                            }
                            throw raise;
                        }

                        ModelState.Clear();

                        string subject = "Portal de Candidaturas à Base Naval - Password de Acesso";
                        string body = "A sua password de acesso ao portal de candidaturas é a seguinte: " + newPassword;

                        Email.SendEmail(userModel.Email, subject, body);

                        ViewBag.Subtitle = "Criação de Conta";

                        ViewBag.ConfirmationMessage = "Registo de utilizador efetuado com sucesso. A sua password de acesso foi enviada para o seu email.";

                        return View("~/Views/Shared/Success.cshtml");
                    }

                    return View();
                }
                
            }
            else
            {
                ViewBag.ErrorMessage = "Por favor valide o Recaptcha.";

                getDataForDropdownLists();

                return View();
            }
        }

        [HttpPost]
        //Verifica a existência do email na base de dados
        public Boolean checkEmail(String email)
        {
            CandidaturaDBEntities db = new CandidaturaDBEntities();

            if (!db.Users.Any(u => u.Email == email))
            {
                return false;
            }

            return true;
        }

        [HttpGet]
        // GET: PasswordRecovery
        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecoverPassword(User userModel)
        {
            CandidaturaDBEntities db = new CandidaturaDBEntities();
            string email = userModel.Email;

            User user = db.Users.Where(u => u.Email == email).FirstOrDefault();

            if (user == null)
            {
                ViewBag.PasswordError = "O email indicado não foi encontrado no sistema.";

                return View();
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