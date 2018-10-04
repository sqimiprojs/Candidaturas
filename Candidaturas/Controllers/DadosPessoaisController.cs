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
        string concelhoNaturalEscolhido = null;
        string freguesiaNaturalEscolhida = null;
        string distritoMoradaEscolhido = null;
        string concelhoMoradaEscolhido = null;
        string freguesiaMoradaEscolhida = null;
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

                this.getDataForDropdownLists(distritoNaturalEscolhido, concelhoNaturalEscolhido, distritoMoradaEscolhido, concelhoMoradaEscolhido);
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
                concelhoNaturalEscolhido = dadosPessoaisUser.ConcelhoNatural;
                freguesiaNaturalEscolhida = dadosPessoaisUser.FreguesiaNatural;
                distritoMoradaEscolhido = dadosPessoaisUser.DistritoMorada;
                concelhoMoradaEscolhido = dadosPessoaisUser.ConcelhoMorada;
                freguesiaMoradaEscolhida = dadosPessoaisUser.FreguesiaMorada;
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
        public void getDataForDropdownLists(string distritoNatural, string concelhoNatural, string distritoMorada, string concelhoMorada)
        {
            LoginDataBaseEntities1 db = new LoginDataBaseEntities1();

            IEnumerable<SelectListItem> generos = db.Generoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == generoEscolhido

            });

            IEnumerable<SelectListItem> distritosNaturais = db.Distritoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == distritoNaturalEscolhido

            });

            string codigoDistritoNatural = db.Distritoes.Where(d => d.Nome == distritoNatural).Select(d => d.Codigo).FirstOrDefault();

            IEnumerable<SelectListItem> concelhosNaturais = db.Concelhoes.Where(dp => dp.CodigoDistrito == codigoDistritoNatural).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == concelhoNaturalEscolhido

            });

            string codigoConcelhoNatural = db.Concelhoes.Where(c => c.Nome == concelhoNatural).Select(c => c.Codigo).FirstOrDefault();

            IEnumerable<SelectListItem> freguesiasNaturais = db.Freguesias.Where(dp => dp.CodigoConcelho == codigoConcelhoNatural && dp.CodigoDistrito == codigoDistritoNatural).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == freguesiaNaturalEscolhida

            });

            IEnumerable<SelectListItem> distritosMoradas = db.Distritoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == distritoMoradaEscolhido

            });

            string codigoDistritoMorada = db.Distritoes.Where(d => d.Nome == distritoMorada).Select(d => d.Codigo).FirstOrDefault();

            IEnumerable<SelectListItem> concelhosMoradas = db.Concelhoes.Where(dp => dp.CodigoDistrito == codigoDistritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == concelhoMoradaEscolhido

            });

            string codigoConcelhoMorada = db.Concelhoes.Where(c => c.Nome == concelhoMorada).Select(c => c.Codigo).FirstOrDefault();

            IEnumerable<SelectListItem> freguesiasMoradas = db.Freguesias.Where(dp => dp.CodigoConcelho == codigoConcelhoMorada && dp.CodigoDistrito == codigoDistritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == freguesiaMoradaEscolhida

            });

            IEnumerable<SelectListItem> tiposDocumentosId = db.TipoDocumentoIDs.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == tipoDocIdEscolhido

            });

            IEnumerable<SelectListItem> estadosCivis = db.EstadoCivils.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == estadoCivilEscolhido

            });

            IEnumerable<SelectListItem> nacionalidades = db.Nacionalidades.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == nacionalidadeEscolhida

            });

            IEnumerable<SelectListItem> localidades = db.Localidades.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == localidadeEscolhida

            });

            ViewBag.Genero = generos.ToList();
            ViewBag.TipoDocID = tiposDocumentosId.ToList();
            ViewBag.EstadoCivil = estadosCivis.ToList();
            ViewBag.DistritoNatural = distritosNaturais.ToList();
            ViewBag.ConcelhoNatural = concelhosNaturais.ToList();
            ViewBag.FreguesiaNatural = freguesiasNaturais.ToList();
            ViewBag.DistritoMorada = distritosMoradas.ToList();
            ViewBag.ConcelhoMorada = concelhosMoradas.ToList();
            ViewBag.FreguesiaMorada = freguesiasMoradas.ToList();
            ViewBag.Nacionalidade = nacionalidades.ToList();
            ViewBag.Localidade = localidades.ToList();
        }

        [HttpPost]
        //actualiza lista de concelhos consoante o distrito seleccionado
        public JsonResult updateConcelhos(string distrito)
        {
            LoginDataBaseEntities1 db = new LoginDataBaseEntities1();

            string codigoDistrito = db.Distritoes.Where(d => d.Nome == distrito).Select(d => d.Codigo).FirstOrDefault();

            var concelhos = db.Concelhoes.Where(dp => dp.CodigoDistrito == codigoDistrito).OrderBy(dp => dp.Nome).Select(c => new
            {
                ID = c.Nome,
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

            string codigoConcelho = db.Concelhoes.Where(c => c.Nome == concelho).Select(c => c.Codigo).FirstOrDefault();

            string codigoDistrito = db.Concelhoes.Where(c => c.Nome == concelho).Select(c => c.CodigoDistrito).FirstOrDefault();

            var freguesias = db.Freguesias.Where(dp => dp.CodigoConcelho == codigoConcelho && dp.CodigoDistrito == codigoDistrito).OrderBy(dp => dp.Nome).Select(c => new
            {
                ID = c.Nome,
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