using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Candidaturas.Models;

namespace Candidaturas.Controllers
{
    public class DadosPessoaisController : Controller
    {
        string generoEscolhido = null;
        string tipoDocIdEscolhido = null;
        int? distritoNaturalEscolhido = null;
        int? concelhoNaturalEscolhido = null;
        int? freguesiaNaturalEscolhida = null;
        int? distritoMoradaEscolhido = null;
        int? concelhoMoradaEscolhido = null;
        int? freguesiaMoradaEscolhida = null;
        int? localidadeEscolhida = null;
        string estadoCivilEscolhido = null;
        string nacionalidadeEscolhida = null;

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
            LoginDataBaseEntities db = new LoginDataBaseEntities();
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
        public void getDataForDropdownLists(int? distritoNatural, int? concelhoNatural, int? distritoMorada, int? concelhoMorada)
        {
            LoginDataBaseEntities db = new LoginDataBaseEntities();

            IEnumerable<SelectListItem> generos = db.Generoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == generoEscolhido

            });

            IEnumerable<SelectListItem> distritosNaturais = db.Distritoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == distritoNaturalEscolhido

            });

            IEnumerable<SelectListItem> concelhosNaturais = db.Concelhoes.Where(dp => dp.CodigoDistrito == distritoNatural).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == concelhoNaturalEscolhido

            });

            IEnumerable<SelectListItem> freguesiasNaturais = db.Freguesias.Where(dp => dp.CodigoConcelho == concelhoNatural && dp.CodigoDistrito == distritoNatural).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == freguesiaNaturalEscolhida

            });

            IEnumerable<SelectListItem> distritosMoradas = db.Distritoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == distritoMoradaEscolhido

            });

            IEnumerable<SelectListItem> concelhosMoradas = db.Concelhoes.Where(dp => dp.CodigoDistrito == distritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == concelhoMoradaEscolhido

            });

            IEnumerable<SelectListItem> freguesiasMoradas = db.Freguesias.Where(dp => dp.CodigoConcelho == concelhoMorada && dp.CodigoDistrito == distritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == freguesiaMoradaEscolhida

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

            IEnumerable<SelectListItem> nacionalidades = db.Pais.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Sigla, 
                Text = c.Nome,
                Selected = c.Sigla == nacionalidadeEscolhida 

            });

            IEnumerable<SelectListItem> localidades = db.Localidades.Where(dp => dp.CodigoConcelho == concelhoMorada && dp.CodigoDistrito == distritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == localidadeEscolhida

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
        public JsonResult updateConcelhos(int distrito)
        {
            LoginDataBaseEntities db = new LoginDataBaseEntities();

            var concelhos = db.Concelhoes.Where(dp => dp.CodigoDistrito == distrito).OrderBy(dp => dp.Nome).Select(c => new
            {
                ID = c.Codigo,
                Name = c.Nome
            }).ToList();

            JsonResult jsonConcelhos = new JsonResult
            {
                Data = concelhos.ToList(),

                ContentType = "application / json"
            };

            return jsonConcelhos;
        }

        [HttpPost]
        //actualiza lista de freguesias consoante o concelho seleccionado
        public JsonResult updateFreguesias(int distrito, int concelho)
        {
            LoginDataBaseEntities db = new LoginDataBaseEntities();

            var freguesias = db.Freguesias.Where(dp => dp.CodigoDistrito == distrito && dp.CodigoConcelho == concelho).OrderBy(dp => dp.Nome).Select(c => new
            {
                ID = c.Codigo,
                Name = c.Nome
            }).ToList();

            JsonResult jsonFreguesias = new JsonResult
            {
                Data = freguesias.ToList(),

                ContentType = "application / json"
            };

            return jsonFreguesias;
        }

        [HttpPost]
        //actualiza lista de localidades consoante o distrito seleccionado
        public JsonResult updateLocalidades(int distrito, int concelho)
        {
            LoginDataBaseEntities db = new LoginDataBaseEntities();

            var localidades = db.Localidades.Where(dp => dp.CodigoDistrito == distrito && dp.CodigoConcelho == concelho).OrderBy(dp => dp.Nome).Select(c => new
            {
                ID = c.Codigo,
                Name = c.Nome
            }).ToList();

            JsonResult jsonLocalidades = new JsonResult
            {
                Data = localidades.ToList(),

                ContentType = "application / json"
            };

            return jsonLocalidades;
        }

        [HttpPost]
        public ActionResult AddOrEdit(DadosPessoai dadosPessoaisModel)
        {
            if(Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (LoginDataBaseEntities dbModel = new LoginDataBaseEntities())
                {

                    try
                    {

                        DadosPessoai dadosPessoaisUser = dbModel.DadosPessoais.Where(dp => dp.UserId == userId).FirstOrDefault();

                        dadosPessoaisModel.UserId = userId;

                        //obter dígitos de controlo do cartão de cidadão
                        if(dadosPessoaisModel.TipoDocID == "Cartão do Cidadão" || dadosPessoaisModel.TipoDocID == "Bilhete de Identidade")
                        {
                            int idx = dadosPessoaisModel.NDI.IndexOf("-")+1;
                            int length = dadosPessoaisModel.NDI.Length;
                            string digitosControlo = dadosPessoaisModel.NDI.Substring(idx, length - idx);
                            dadosPessoaisModel.CCDigitosControlo = digitosControlo;
                        }
                        else
                        {
                            dadosPessoaisModel.CCDigitosControlo = String.Empty;
                        }

                        //adicionar ou atualizar
                        if (dadosPessoaisUser == null)
                        {
                            dadosPessoaisModel.DataCriacao = System.DateTime.Now;
                            dadosPessoaisModel.DataUltimaAtualizacao = System.DateTime.Now;
                            dbModel.DadosPessoais.Add(dadosPessoaisModel);
                        }
                        else
                        {
                            DateTime? dataCriacao = dadosPessoaisUser.DataCriacao;
                            dbModel.DadosPessoais.Remove(dadosPessoaisUser);
                            dadosPessoaisUser = dadosPessoaisModel;
                            dadosPessoaisUser.DataCriacao = dataCriacao;
                            dadosPessoaisUser.DataUltimaAtualizacao = System.DateTime.Now;
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
            else
            {
                return RedirectToAction("LogOut", "Login");
            }          
        }
    }
}