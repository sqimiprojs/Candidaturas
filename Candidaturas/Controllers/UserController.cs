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
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

            IEnumerable<SelectListItem> tiposDocumentosId = db.TipoDocumentoIDs.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome
            });

            ViewBag.TipoDocID = tiposDocumentosId;

            IEnumerable<SelectListItem> ramos = db.Ramoes.Select(c => new SelectListItem
            {
                Value = c.Sigla,
                Text = c.Nome
            });

            ViewBag.Ramo = ramos;

            IEnumerable<SelectListItem> categorias = db.Categorias.Select(c => new SelectListItem
            {
                Value = c.Sigla,
                Text = c.Nome
            });

            ViewBag.Categoria = categorias;

            IEnumerable<SelectListItem> postos = db.Postoes.Select(c => new SelectListItem
            {
                Value = c.Código.ToString(),
                Text = c.Nome
            });

            ViewBag.Posto = postos;
        }

        [HttpGet]
        // GET: User
        public ActionResult NewUser()
        {
            UserDados model = new UserDados();
            model.user = new User();
            model.dadosPessoais = new DadosPessoai();
            getDataForDropdownLists();
            return View(model);
        }

        [HttpPost]
        public ActionResult NewUser(UserDados model)
        {
            CaptchaResponse response = CaptchaValidator.ValidateCaptcha(Request["g-recaptcha-response"]);

            if (response.Success)
            {
                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    if (!dbModel.Users.Any(u => u.Email == model.user.Email))
                    {
                        try
                        {
                            string newPassword = Password.GeneratePassword().ToString();
                            using (SHA256 mySHA256 = SHA256.Create())
                            {

                                model.user.Password = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                                model.user.DataCriacao = System.DateTime.Now;
                                                                
                                model.dadosPessoais.DataCriacao = System.DateTime.Now;
                                model.dadosPessoais.DataUltimaAtualizacao = System.DateTime.Now;

                                dbModel.Users.Add(model.user);
                                dbModel.DadosPessoais.Add(model.dadosPessoais);

                                dbModel.SaveChanges();

                                ModelState.Clear();

                                string subject = "Portal de Candidaturas à Base Naval - Palavra-passe de Acesso";
                                string body = "A sua palavra-passe de acesso ao portal de candidaturas é: " + newPassword;

                                Email.SendEmail(model.user.Email, subject, body);

                                ViewBag.Subtitle = "Criação de Conta";

                                ViewBag.ConfirmationMessage = "Registo de utilizador efetuado com sucesso. A sua palavra-passe de acesso será enviada para o seu email. Por favor verifique.";

                                return View("~/Views/Shared/Success.cshtml");
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
                    }
                    getDataForDropdownLists();

                    return View(model);
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
        //actualiza lista de postos consoante o ramo e categoria seleccionados
        public JsonResult updatePostos(string ramo, string categoria)
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

            var postos = db.Postoes.Where(dp => dp.RamoMilitar == ramo && dp.CategoriaMilitar == categoria).OrderBy(dp => dp.Nome).Select(c => new
            {
                ID = c.Código,
                Name = c.Nome
            }).ToList();

            JsonResult jsonPostos = new JsonResult
            {
                Data = postos.ToList(),

                ContentType = "application / json"
            };

            return jsonPostos;
        }

        [HttpPost]
        //Verifica a existência do email na base de dados
        public Boolean checkEmail(String email)
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

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
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
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
}