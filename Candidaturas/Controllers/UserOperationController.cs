using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Candidaturas.Models;

namespace Candidaturas.Controllers
{
    public class UserOperationController : Controller
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

        // UserOperations/NewUser
        [HttpGet]
        // GET: User
        public ActionResult Index()
        {
            User userModel = new User();
            getDataForDropdownLists();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult newUser(User userModel)
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
                            userModel.Password = newPassword;
                            userModel.DataCriacao = System.DateTime.Now;
                            dbModel.Users.Add(userModel);
                            /**/
                            dbModel.SaveChanges();
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
    }
}