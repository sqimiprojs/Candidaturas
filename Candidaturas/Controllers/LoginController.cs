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
                var userDetails = db.Users.Where(x => x.Email == model.user.Email && x.Password == hashedUserPassword).FirstOrDefault();

                if (userDetails == null)
                {
                    TempData["LogError"] = "Email não está registado ou palavra-passe está errada.";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    var userAux = db.Users.Where(x => x.Email == model.user.Email && x.Password == hashedUserPassword && x.Edicao == edicao.Sigla).FirstOrDefault();
                    if(userAux == null)
                    {
                        User novoUser = new User();
                        novoUser.Email = userDetails.Email;
                        novoUser.Password = userDetails.Password;
                        novoUser.DataCriacao = userDetails.DataCriacao;
                        novoUser.NomeColoquial = userDetails.NomeColoquial;
                        novoUser.DataNascimento = userDetails.DataNascimento;
                        novoUser.TipoDocID = userDetails.TipoDocID;
                        novoUser.NDI = userDetails.NDI;
                        novoUser.DocumentoValidade = userDetails.DocumentoValidade;
                        novoUser.Militar = userDetails.Militar;
                        novoUser.Militar1.Ramo = userDetails.Militar1.Ramo;
                        novoUser.Militar1.Categoria = userDetails.Militar1.Categoria;
                        novoUser.Militar1.Posto = userDetails.Militar1.Posto;
                        novoUser.Militar1.Classe = userDetails.Militar1.Classe;
                        novoUser.Militar1.NIM = userDetails.Militar1.NIM;
                        novoUser.Edicao = edicao.Sigla;
                        db.Users.Add(novoUser);
                        db.Militars.Add(novoUser.Militar1);
                        db.SaveChanges();

                        var loginNovoUser = db.Users.Where(x => x.Email == model.user.Email && x.Password == hashedUserPassword && x.Edicao == edicao.Sigla).FirstOrDefault();
                        Session["userID"] = loginNovoUser.ID;
                        return RedirectToAction("Index", "Home");
                    } else
                    {
                        Session["userID"] = userAux.ID;
                        return RedirectToAction("Index", "Home");
                    }

                    
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