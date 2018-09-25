using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Candidaturas.Models;
using Newtonsoft.Json;

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

        /// <summary>  
        /// Validate Captcha  
        /// </summary>  
        /// <param name="response"></param>  
        /// <returns></returns>  
        public static CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = System.Web.Configuration.WebConfigurationManager.AppSettings["recaptchaPrivateKey"];
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
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
            Password pwd = new Password();
            string newPassword = pwd.GeneratePassword().ToString();

            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);

            if (response.Success)
            {
                using (LoginDataBaseEntities1 dbModel = new LoginDataBaseEntities1())
                {

                    try
                    {
                        userModel.Password = newPassword;
                        dbModel.Users.Add(userModel);
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

                pwd.MailPassword(userModel.Email, newPassword);

                return View("Success");
            }
            else
            {
                ViewBag.ErrorMessage = "Por favor valide o Recaptcha.";

                return View();
            }
        }
    }
}