using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Candidaturas.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace Candidaturas.Controllers
{
    public class DadosPessoaisController : Controller
    {
        DadosPessoai DadosPessoaisEscolha = new DadosPessoai();

        // GET: DadosPessoais
        public ActionResult Index()
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                if (userId != 0)
                {
                    this.getDadosPessoais(userId);
                }

                this.getDataForDropdownLists();

                return View("~/Views/DadosPessoais/AddOrEdit.cshtml");
            }

            return View("~/Views/Login/Index.cshtml");
        }

        //obtém os dados preenchdios pelo utilizador para mostrar no ecrã
        public void getDadosPessoais(int userId)
        {
            ViewBag.Title = "Dados Pessoais";
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
            User user = db.Users.Where(u => u.ID == userId).FirstOrDefault();
            DadosPessoai dadosPessoaisUser = db.DadosPessoais.Where(u => u.CandidaturaId == candidaturaId).FirstOrDefault();

            if (dadosPessoaisUser == null)
            {
                 dadosPessoaisUser = new DadosPessoai();

                if (user != null)
                {
                    dadosPessoaisUser.NomeColoquial = user.NomeColoquial;
                    dadosPessoaisUser.DataNascimento = user.DataNascimento;
                    dadosPessoaisUser.TipoDocID = user.TipoDocID;
                    dadosPessoaisUser.NomeColoquial = user.NomeColoquial;
                    dadosPessoaisUser.NDI = user.NDI;
                    dadosPessoaisUser.DocumentoValidade = user.DocumentoValidade;
                    dadosPessoaisUser.Militar = user.Militar;
                    dadosPessoaisUser.Ramo = user.Ramo;
                    dadosPessoaisUser.Categoria = user.Categoria;
                    dadosPessoaisUser.Posto = user.Posto;
                    dadosPessoaisUser.Classe = user.Classe;
                    dadosPessoaisUser.NIM = user.NIM;

                    ViewBag.DadosPessoais = dadosPessoaisUser;

                    DadosPessoaisEscolha = dadosPessoaisUser;
                    ViewData["dn"] = dadosPessoaisUser.DistritoNatural != null;
                    ViewData["dm"] = dadosPessoaisUser.DistritoMorada != null;
                    ViewData["mil"] = dadosPessoaisUser.Militar;
                }
            }
            else
            {
                ViewBag.DadosPessoais = dadosPessoaisUser;

                DadosPessoaisEscolha = dadosPessoaisUser;
                ViewData["dn"] = dadosPessoaisUser.DistritoNatural != null;
                ViewData["dm"] = dadosPessoaisUser.DistritoMorada != null;
                ViewData["mil"] = dadosPessoaisUser.Militar;
            }

        }

        //obtém os dados a serem preenchidos nas drops
        public void getDataForDropdownLists()
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

            IEnumerable<SelectListItem> generos = db.Generoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == DadosPessoaisEscolha.Genero

            });
            ViewBag.Genero = generos.ToList();

            IEnumerable<SelectListItem> distritosNaturais = db.Distritoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == DadosPessoaisEscolha.DistritoNatural

            });
            ViewBag.DistritoNatural = distritosNaturais.ToList();

            IEnumerable<SelectListItem> concelhosNaturais = db.Concelhoes.Where(dp => dp.CodigoDistrito == DadosPessoaisEscolha.DistritoNatural).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == DadosPessoaisEscolha.ConcelhoNatural

            });
            ViewBag.ConcelhoNatural = concelhosNaturais.ToList();

            IEnumerable<SelectListItem> freguesiasNaturais = db.Freguesias.Where(dp => dp.CodigoConcelho == DadosPessoaisEscolha.ConcelhoNatural && dp.CodigoDistrito == DadosPessoaisEscolha.DistritoNatural).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == DadosPessoaisEscolha.FreguesiaNatural

            });
            ViewBag.FreguesiaNatural = freguesiasNaturais.ToList();

            IEnumerable<SelectListItem> distritosMoradas = db.Distritoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == DadosPessoaisEscolha.DistritoMorada

            });
            ViewBag.DistritoMorada = distritosMoradas.ToList();

            IEnumerable<SelectListItem> concelhosMoradas = db.Concelhoes.Where(dp => dp.CodigoDistrito == DadosPessoaisEscolha.DistritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == DadosPessoaisEscolha.ConcelhoMorada

            });
            ViewBag.ConcelhoMorada = concelhosMoradas.ToList();

            IEnumerable<SelectListItem> freguesiasMoradas = db.Freguesias.Where(dp => dp.CodigoConcelho == DadosPessoaisEscolha.ConcelhoMorada && dp.CodigoDistrito == DadosPessoaisEscolha.DistritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == DadosPessoaisEscolha.FreguesiaMorada

            });
            ViewBag.FreguesiaMorada = freguesiasMoradas.ToList();

            IEnumerable<SelectListItem> tiposDocumentosId = db.TipoDocumentoIDs.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == DadosPessoaisEscolha.TipoDocID

            });
            ViewBag.TipoDocID = tiposDocumentosId.ToList();

            IEnumerable<SelectListItem> estadosCivis = db.EstadoCivils.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == DadosPessoaisEscolha.EstadoCivil

            });
            ViewBag.EstadoCivil = estadosCivis.ToList();

            IEnumerable<SelectListItem> nacionalidades = db.Pais.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Sigla,
                Text = c.Nome,
                Selected = c.Sigla == DadosPessoaisEscolha.Nacionalidade

            });
            ViewBag.Nacionalidade = nacionalidades.ToList();

            IEnumerable<SelectListItem> localidades = db.Localidades.Where(dp => dp.CodigoConcelho == DadosPessoaisEscolha.ConcelhoMorada && dp.CodigoDistrito == DadosPessoaisEscolha.DistritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == DadosPessoaisEscolha.Localidade

            });
            ViewBag.Localidade = localidades.ToList();

            IEnumerable<SelectListItem> ramos = db.Ramoes.OrderBy(dp => dp.Sigla).Select(c => new SelectListItem
            {
                Value = c.Sigla,
                Text = c.Nome,
                Selected = c.Sigla == DadosPessoaisEscolha.Ramo

            });
            ViewBag.Ramo = ramos.ToList();

            IEnumerable<SelectListItem> categorias = db.Categorias.OrderBy(dp => dp.Sigla).Select(c => new SelectListItem
            {
                Value = c.Sigla,
                Text = c.Nome,
                Selected = c.Sigla == DadosPessoaisEscolha.Categoria

            });
            ViewBag.Categoria = categorias.ToList();

            IEnumerable<SelectListItem> postos = db.Postoes.Where(dp => dp.RamoMilitar == DadosPessoaisEscolha.Ramo && dp.CategoriaMilitar == DadosPessoaisEscolha.Categoria).OrderBy(dp => dp.Código).Select(c => new SelectListItem
            {
                Value = c.Código.ToString(),
                Text = c.Nome,
                Selected = c.Código == DadosPessoaisEscolha.Posto

            });
            ViewBag.Posto = postos.ToList();

            IEnumerable<SelectListItem> reparticoes = db.Reparticoes.Where(dp => dp.CodigoDistrito == DadosPessoaisEscolha.DistritoMorada && dp.CodigoConcelho == DadosPessoaisEscolha.ConcelhoMorada && dp.CodigoFreguesia == DadosPessoaisEscolha.FreguesiaMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == DadosPessoaisEscolha.RepFinNIF

            });
            ViewBag.RepFinNIF = reparticoes.ToList();
        }

        [HttpPost]
        //actualiza lista de concelhos consoante o distrito seleccionado
        public JsonResult updateConcelhos(int distrito)
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

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
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

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
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

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
        //actualiza lista de localidades consoante o distrito seleccionado
        public JsonResult updateReparticoes(int distrito, int concelho, int freguesia)
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

            var reparticoes = db.Reparticoes.Where(dp => dp.CodigoDistrito == distrito && dp.CodigoConcelho == concelho && dp.CodigoFreguesia == freguesia).OrderBy(dp => dp.Nome).Select(c => new
            {
                ID = c.Codigo,
                Name = c.Nome
            }).ToList();

            JsonResult jsonReparticoes = new JsonResult
            {
                Data = reparticoes.ToList(),

                ContentType = "application / json"
            };

            return jsonReparticoes;
        }

        [HttpPost]
        public ActionResult DadosPessoais(DadosPessoai dadosPessoaisModel)
        {
            if (Session["userID"] == null)
                return RedirectToAction("LogOut", "Login");
            else
            {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    Edicao edicao = dbModel.Edicaos.Where(e => e.DataInicio < System.DateTime.Now && e.DataFim > System.DateTime.Now).First();
                    var userDetails = dbModel.Users.Where(x => x.ID == userId).FirstOrDefault();
                    Candidatura candidatura = dbModel.Candidaturas.Where(c => c.UserId == userDetails.ID && c.Edicao == edicao.Sigla).FirstOrDefault();
                    if (candidatura == null)
                    {
                        Candidatura novaCandidatura = new Candidatura();

                        Candidatura ultimaCandidatura = dbModel.Candidaturas.Where(c => c.Edicao == edicao.Sigla).OrderByDescending(c => c.id).FirstOrDefault();
                        if (ultimaCandidatura == null)
                        {
                            novaCandidatura.id = 1;
                        }
                        else
                        {
                            novaCandidatura.id = ultimaCandidatura.id + 1;
                        }
                        novaCandidatura.Edicao = edicao.Sigla;
                        novaCandidatura.UserId = userDetails.ID;
                        novaCandidatura.DataAlteracao = System.DateTime.Now;
                        dbModel.Candidaturas.Add(novaCandidatura);
                        dbModel.SaveChanges();

                        Historico novoHistorico = new Historico();
                        novoHistorico.timestamp = System.DateTime.Now;
                        novoHistorico.mensagem = "Candidatura criada para o user: " + userDetails.Email;
                        int candidaturaAux = dbModel.Candidaturas.Where(c => c.UserId == userDetails.ID && c.Edicao == edicao.Sigla).Select(c => c.id).First();
                        novoHistorico.CandidaturaID = candidaturaAux;
                        dbModel.Historicoes.Add(novoHistorico);
                        dbModel.SaveChanges();

                    }
                    int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();

                    try
                    {
                        DadosPessoai dadosPessoaisUser = dbModel.DadosPessoais.Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();

                        dadosPessoaisModel.CandidaturaId = candidaturaId;

                        //obter dígitos de controlo do cartão de cidadão
                        if (dadosPessoaisModel.TipoDocID == 4 || dadosPessoaisModel.TipoDocID == 2)
                        {
                            int idx = dadosPessoaisModel.NDI.IndexOf("-") + 1;
                            int length = dadosPessoaisModel.NDI.Length;
                            string digitosControlo = dadosPessoaisModel.NDI.Substring(idx, length - idx);
                            dadosPessoaisModel.CCDigitosControlo = digitosControlo;
                        }
                        else
                        {
                            dadosPessoaisModel.CCDigitosControlo = String.Empty;
                        }

                        //adicionar ou atualizar
                        Historico novoHistorico = new Historico();

                        if (dadosPessoaisUser == null)
                        {
                            dadosPessoaisModel.DataCriacao = System.DateTime.Now;
                            dadosPessoaisModel.DataUltimaAtualizacao = System.DateTime.Now;
                            dbModel.DadosPessoais.Add(dadosPessoaisModel);
                            novoHistorico.timestamp = System.DateTime.Now;
                            novoHistorico.mensagem = "Dados pessoais inseridos.";
                            novoHistorico.CandidaturaID = candidaturaId;
                            dbModel.Historicoes.Add(novoHistorico);
                        }
                        else
                        {
                            DateTime dataCriacao = dadosPessoaisUser.DataCriacao;
                            dbModel.DadosPessoais.Remove(dadosPessoaisUser);
                            dadosPessoaisUser = dadosPessoaisModel;
                            if (dadosPessoaisModel.NIM != null)
                            {
                                dadosPessoaisUser.Militar = true;
                            }
                            dadosPessoaisUser.DataCriacao = dataCriacao;
                            dadosPessoaisUser.DataUltimaAtualizacao = System.DateTime.Now;
                            dbModel.DadosPessoais.Add(dadosPessoaisUser);
                            novoHistorico.timestamp = System.DateTime.Now;
                            novoHistorico.mensagem = "Dados pessoais alterados.";
                            novoHistorico.CandidaturaID = candidaturaId;
                            dbModel.Historicoes.Add(novoHistorico);
                        }

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

                Session["SelectedTab"] = 2;

                return RedirectToAction("Index", "Home");
            }
        }

    }
}
