using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Candidaturas.Models;
using Newtonsoft.Json;

namespace Candidaturas.Controllers
{
    public class DadosPessoaisController : Controller
    {
        string generoEscolhido = null;
        string tipoDocIdEscolhido = null;
        string distritoNaturalEscolhido = null;
        string concelhoEscolhido = null;
        string freguesiaEscolhida = null;
        string estadoCivilEscolhido = null;
        string nacionalidadeEscolhida = null;
        string localidadeEscolhida = null;

        // GET: DadosPessoais
        public ActionResult Index()
        {
            if(Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                if (userId != 0)
                {
                    this.getDadosPessoais(userId);
                }

                this.getDataForDropdownLists(distritoNaturalEscolhido, concelhoEscolhido);
            }

            return View("~/Views/DadosPessoais/AddOrEdit.cshtml");
        }

        //obtém os dados preenchdios pelo utilizador para mostrar no ecrã
        public void getDadosPessoais(int userId)
        {
            LoginDataBaseEntities1 db = new LoginDataBaseEntities1();
            DadosPessoai dadosPessoaisUser = db.DadosPessoais.Where(dp => dp.UserId == userId).FirstOrDefault();

            if(dadosPessoaisUser != null)
            {
                ViewBag.DadosPessoais = dadosPessoaisUser;

                generoEscolhido = dadosPessoaisUser.Genero;
                distritoNaturalEscolhido = dadosPessoaisUser.DistritoNatural;
                concelhoEscolhido = dadosPessoaisUser.Concelho;
                freguesiaEscolhida = dadosPessoaisUser.Freguesia;
                estadoCivilEscolhido = dadosPessoaisUser.EstadoCivil;
                nacionalidadeEscolhida = dadosPessoaisUser.Nacionalidade;
                localidadeEscolhida = dadosPessoaisUser.Localidade;
                tipoDocIdEscolhido = dadosPessoaisUser.TipoDocID;
            }
            else
            {
                string tipoDocId = db.Users.Where(dp => dp.ID == userId).Select(dp => dp.TipoDocID).FirstOrDefault();

                tipoDocIdEscolhido = tipoDocId;
            }

            string nomeCompleto = db.Users.Where(dp => dp.ID == userId).Select(dp => dp.NomeCompleto).FirstOrDefault();
            string ndi = db.Users.Where(dp => dp.ID == userId).Select(dp => dp.NDI).FirstOrDefault();

            ViewBag.NomeCompleto = nomeCompleto;
            ViewBag.NDI = ndi;
        }

        //obtém os dados a serem preenchidos nas drops
        public void getDataForDropdownLists(string distrito, string concelho)
        {
            LoginDataBaseEntities1 db = new LoginDataBaseEntities1();

            IEnumerable<SelectListItem> generos = db.Generoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID.ToString() == generoEscolhido

            });

            IEnumerable<SelectListItem> distritos = db.Distritoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID.ToString() == distritoNaturalEscolhido

            });

            IEnumerable<SelectListItem> concelhos = db.Concelhoes.Where(dp => dp.Distrito == distrito).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID.ToString() == concelhoEscolhido

            });

            IEnumerable<SelectListItem> freguesias = db.Freguesias.Where(dp => dp.Concelho == concelho).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID.ToString() == freguesiaEscolhida

            });

            IEnumerable<SelectListItem> tiposDocumentosId = db.TipoDocumentoIDs.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID.ToString() == tipoDocIdEscolhido

            });

            IEnumerable<SelectListItem> estadosCivis = db.EstadoCivils.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID.ToString() == estadoCivilEscolhido

            });

            IEnumerable<SelectListItem> nacionalidades = db.Nacionalidades.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID.ToString() == nacionalidadeEscolhida

            });

            IEnumerable<SelectListItem> localidades = db.Localidades.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID.ToString() == localidadeEscolhida

            });

            ViewBag.Concelho = concelhos.ToList();
            ViewBag.Genero = generos.ToList();
            ViewBag.Freguesia = freguesias.ToList();
            ViewBag.DistritoNatural = distritos.ToList();
            ViewBag.TipoDocID = tiposDocumentosId.ToList();
            ViewBag.EstadoCivil = estadosCivis.ToList();
            ViewBag.Nacionalidade = nacionalidades.ToList();
            ViewBag.Localidade = localidades.ToList();
        }

        [HttpPost]
        //actualiza lista de concelhos consoante o distrito seleccionado
        public JsonResult updateConcelhos(string distrito)
        {
            LoginDataBaseEntities1 db = new LoginDataBaseEntities1();

            var concelhos = db.Concelhoes.Where(dp => dp.Distrito == distrito).OrderBy(dp => dp.Nome).Select(c => new
            {
                ID = c.ID.ToString(),
                Name = c.Nome
            }).ToList();

            JsonResult jsonConcelhos = new JsonResult();

            jsonConcelhos.Data = concelhos.ToList();

            jsonConcelhos.ContentType = "application / json";

            return jsonConcelhos;
        }

        [HttpPost]
        //actualiza lista de freguesias consoante o concelho seleccionado
        public JsonResult updateFreguesias(string concelho)
        {
            LoginDataBaseEntities1 db = new LoginDataBaseEntities1();

            var freguesias = db.Freguesias.Where(dp => dp.Concelho == concelho).OrderBy(dp => dp.Nome).Select(c => new
            {
                ID = c.ID.ToString(),
                Name = c.Nome
            }).ToList();

            JsonResult jsonFreguesias = new JsonResult();

            jsonFreguesias.Data = freguesias.ToList();

            jsonFreguesias.ContentType = "application / json";

            return jsonFreguesias;
        }

        [HttpGet]
        // GET: User
        public ActionResult AddOrEdit(int id = 0)
        {
            DadosPessoai dadosPessoaisModel = new DadosPessoai();
            return View(dadosPessoaisModel);
        }

        [HttpPost]
        public ActionResult AddOrEdit(DadosPessoai dadosPessoaisModel)
        {
            int userId = (int)Session["userID"];

            using (LoginDataBaseEntities1 dbModel = new LoginDataBaseEntities1())
            {

                try
                {

                    DadosPessoai dadosPessoaisUser = dbModel.DadosPessoais.Where(dp => dp.UserId == userId).FirstOrDefault();

                    dadosPessoaisModel.UserId = userId;

                    //adicionar ou atualizar
                    if (dadosPessoaisUser == null)
                    {
                        dbModel.DadosPessoais.Add(dadosPessoaisModel);
                    }
                    else
                    {
                        dbModel.DadosPessoais.Remove(dadosPessoaisUser);
                        dadosPessoaisUser = dadosPessoaisModel;
                        dbModel.DadosPessoais.Add(dadosPessoaisUser);
                    }

                    //actualiza nome completo
                    User userData = dbModel.Users.Where(dp => dp.ID == userId).FirstOrDefault();
                    userData.NomeCompleto = dadosPessoaisModel.NomeColoquial;
                    userData.TipoDocID = dadosPessoaisModel.TipoDocID;
                    userData.NDI = dadosPessoaisModel.NDI;

                    dbModel.SaveChanges();

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            ModelState.Clear();

            Session["SelectedTab"] = 1;

            return RedirectToAction("Index", "Home");
        }
    }
}