using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Candidaturas.Models;

namespace Candidaturas.Controllers
{
    public class UserController : Controller
    {

        public void getDataForDropdownLists()
        {
            LoginDataBaseEntities1 db = new LoginDataBaseEntities1();

            IEnumerable<SelectListItem> tiposDocumentosId = db.TipoDocumentoIDs.Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome
            });

            ViewBag.TipoDocID = tiposDocumentosId.ToList();
        }

        [HttpGet]
        // GET: User
        public ActionResult AddOrEdit(int id=0)
        {
            User userModel = new User();
            getDataForDropdownLists();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult AddOrEdit(User userModel)
        {
            string newPassword = Password.GeneratePassword().ToString();

            CaptchaResponse response = CaptchaValidator.ValidateCaptcha(Request["g-recaptcha-response"]);

            if (response.Success)
            {
                using (LoginDataBaseEntities1 dbModel = new LoginDataBaseEntities1())
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
                }
                ModelState.Clear();

                string subject = "Password de Acesso";
                string body = "A password de acesso para a sua conta é a seguinte: " + newPassword;

                Email.SendEmail(userModel.Email, subject, body);

                ViewBag.Subtitle = "Criação de Conta";

                ViewBag.ConfirmationMessage = "Registo de utilizador efetuado com sucesso. A sua password de acesso foi enviada para o seu email.";

                return View("~/Views/Shared/Success.cshtml");
            }
            else
            {
                ViewBag.ErrorMessage = "Por favor valide o Recaptcha.";

                getDataForDropdownLists();

                return View();
            }
        }
    }
}