﻿using System;
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
    public class FormularioController : Controller
    {
        DadosPessoai DadosPessoaisEscolha = new DadosPessoai();
        Inquerito InqEscolha = new Inquerito();

        List<Exame> exames;
        List<CursoDisplay> cursos;
        List<String> examesNecessarios;

        /*-Dados Pessoais-*/
        //obtém os dados preenchdios pelo utilizador para mostrar no ecrã
        public void getDadosPessoais(int userId)
        {
            ViewBag.Title = "Dados Pessoais";
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            DadosPessoai dadosPessoaisUser = db.DadosPessoais.Where(dp => dp.UserId == userId).FirstOrDefault();

            if (dadosPessoaisUser != null)
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
        // GET: DadosPessoais
        public ActionResult DadosPessoais()
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                if (userId != 0)
                {
                    this.getDadosPessoais(userId);
                }

                this.getDataForDropdownLists();

                return View("~/Views/Formulario/DadosPessoais.cshtml");
            }

            return View("~/Views/Login/Index.cshtml");
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
                    try
                    {
                        DadosPessoai dadosPessoaisUser = dbModel.DadosPessoais.Where(dp => dp.UserId == userId).FirstOrDefault();

                        dadosPessoaisModel.UserId = userId;

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
                        if (dadosPessoaisUser == null)
                        {
                            dadosPessoaisModel.DataCriacao = System.DateTime.Now;
                            dadosPessoaisModel.DataUltimaAtualizacao = System.DateTime.Now;
                            dbModel.DadosPessoais.Add(dadosPessoaisModel);
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
                return RedirectToAction("Inquerito");
            }
        }


        /*INQUERITO*/
        //obtém os dados preenchdios pelo utilizador para mostrar no ecrã
        public void getDadosPessoaisInquerito(int userId)
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            Inquerito inqueritoUser = db.Inqueritoes.Where(dp => dp.UserId == userId).FirstOrDefault();

            if (inqueritoUser != null)
            {
                ViewBag.Inquerito = inqueritoUser;
                InqEscolha = inqueritoUser;
            }
        }

        //obtém os dados a serem preenchidos nas drops
        public void getDataForDropdownListsInquerito()
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

            IEnumerable<SelectListItem> situacoesPai = db.Situacaos.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == InqEscolha.SituacaoPai
            });
            ViewBag.SituacaoPai = situacoesPai;

            IEnumerable<SelectListItem> situacoesMae = db.Situacaos.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == InqEscolha.SituacaoMae
            });
            ViewBag.SituacaoMae = situacoesMae;

            IEnumerable<SelectListItem> conhecimentosEscola = db.ConhecimentoEscolas.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == InqEscolha.ConhecimentoEscola
            });
            ViewBag.ConhecimentoEscola = conhecimentosEscola;
        }

        
        // GET: Inquerito
        public ActionResult Inquerito()
        {
            ViewBag.Title = "Inquerito";
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                this.getDadosPessoaisInquerito(userId);
            }

            this.getDataForDropdownListsInquerito();

            return View("~/Views/Formulario/Inquerito.cshtml");
        }

        [HttpPost]
        public ActionResult Inquerito(Inquerito inqueritoModel) { 
            
            if (Session["userID"] == null)
                return RedirectToAction("LogOut", "Login");
            else
            {
                {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {

                    try
                    {
                        Inquerito inqueritoUser = dbModel.Inqueritoes.Where(dp => dp.UserId == userId).FirstOrDefault();

                        inqueritoModel.UserId = userId;

                        if (inqueritoUser == null)
                        {
                            inqueritoModel.DataCriacao = System.DateTime.Now;
                            inqueritoModel.DataAtualizacao = System.DateTime.Now;
                            dbModel.Inqueritoes.Add(inqueritoModel);
                        }
                        else
                        {
                            DateTime? dataCriacao = inqueritoUser.DataCriacao;
                            dbModel.Inqueritoes.Remove(inqueritoUser);
                            inqueritoUser = inqueritoModel;
                            inqueritoUser.DataCriacao = dataCriacao;
                            inqueritoUser.DataAtualizacao = System.DateTime.Now;
                            dbModel.Inqueritoes.Add(inqueritoUser);
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
                
                return RedirectToAction("Opcoes", "Formulario");
            }
            
            }
        }

        //obtém os exames seleccionados pelo utilizador
        public void getSelectedExames(CandidaturaDBEntities1 db, int userId)
        {
            List<int> examesEscolhidos = db.UserExames.Where(dp => dp.UserId == userId).Select(dp => dp.ExameId).ToList();

            foreach (int exame in examesEscolhidos)
            {
                Exame ex = db.Exames.Where(dp => dp.ID == exame).FirstOrDefault();
                exames.Add(ex);
            }
        }

        //obtém os cursos seleccionados pelo utilizador
        public void getSelectedCursos(CandidaturaDBEntities1 db, int userId)
        {
            List<int> cursosEscolhidos = db.UserCursoes.Where(dp => dp.UserId == userId).OrderBy(dp => dp.Prioridade).Select(dp => dp.CursoId).ToList();

            foreach (int curso in cursosEscolhidos)
            {
                CursoDisplay cd = new CursoDisplay();

                cd.nome = db.Cursoes.Where(dp => dp.ID == curso).Select(dp => dp.Nome).First();
                cd.prioridade = db.UserCursoes.Where(dp => dp.CursoId == curso && dp.UserId == userId).Select(dp => dp.Prioridade).FirstOrDefault();
                cd.ID = db.Cursoes.Where(dp => dp.ID == curso).Select(dp => dp.ID).First();
                cursos.Add(cd);

                Curso cursoSel = db.Cursoes.Where(dp => dp.ID == curso).FirstOrDefault();
                List<String> exames = cursoSel.Exames.Select(dp => dp.Nome).ToList();
                foreach(String exame in exames)
                {
                    if(!examesNecessarios.Exists(x => x == exame))
                    {
                        examesNecessarios.Add(exame);
                    }                    
                }                
            }
        }

        //obtém os dados a serem preenchidos nas drops (exames, cursos)
        public void getDataForDropdownLists(CandidaturaDBEntities1 db)
        {

            IEnumerable<SelectListItem> exames = db.Exames.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome
            });

            IEnumerable<SelectListItem> cursos = db.Cursoes.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome
            });

            ViewBag.Exame = exames.ToList();
            ViewBag.Curso = cursos.ToList();

        }

        // GET: Candidaturas
        public ActionResult Opcoes()
        {
            ViewBag.Title = "Opcoes";
            exames = new List<Exame>();
            cursos = new List<CursoDisplay>();
            examesNecessarios = new List<String>();

            if (Session["userID"] != null)
            {
                CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
                int userId = (int)Session["userID"];

                getDataForDropdownLists(db);
                getSelectedExames(db, userId);
                getSelectedCursos(db, userId);

                dynamic mymodel = new ExpandoObject();
                mymodel.ExamesEscolhidos = exames;
                mymodel.CursosEscolhidos = cursos;
                mymodel.ExamesNecessários = examesNecessarios;

                return View(mymodel);
            }

            return View("~/Views/Login/Index.cshtml");
        }
        
        //adiciona um exame seleccionado
        public ActionResult adicionarExame()
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];
                int exameEscolhido = Convert.ToInt32(Request.Form["ExameSeleccionado"]);

                if (exameEscolhido != 0)
                {
                    using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                    {

                        try
                        {
                            UserExame exameRegistado = dbModel.UserExames.Where(dp => dp.ExameId == exameEscolhido).Where(dp => dp.UserId == userId).FirstOrDefault();

                            if (exameRegistado == null)
                            {
                                UserExame userExame = new UserExame();

                                userExame.ExameId = dbModel.Exames.Where(dp => dp.ID == exameEscolhido).FirstOrDefault().ID;
                                userExame.UserId = userId;

                                dbModel.UserExames.Add(userExame);

                                dbModel.SaveChanges();
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
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
                }

                ModelState.Clear();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        //adiciona um curso seleccionado
        public ActionResult adicionarCurso()
        {
            if (Session["userID"] != null)
            {
                int cursoEscolhido = Convert.ToInt32(Request.Form["CursoSeleccionado"]);
                int userId = (int)Session["userID"];

                if (cursoEscolhido != 0)
                {
                    using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                    {

                        try
                        {
                            UserCurso cursoRegistado = dbModel.UserCursoes.Where(dp => dp.CursoId == cursoEscolhido).Where(dp => dp.UserId == userId).FirstOrDefault();

                            //ver se já escolheu o curso
                            if (cursoRegistado == null)
                            {
                                UserCurso userCurso = new UserCurso();

                                userCurso.CursoId = dbModel.Cursoes.Where(dp => dp.ID == cursoEscolhido).FirstOrDefault().ID;
                                userCurso.UserId = userId;


                                //actualizar prioridade
                                UserCurso ultimoEscolhido = dbModel.UserCursoes.Where(dp => dp.UserId == userId).OrderByDescending(dp => dp.Prioridade).FirstOrDefault();

                                if (ultimoEscolhido == null)
                                {
                                    userCurso.Prioridade = 1;
                                }
                                else
                                {
                                    userCurso.Prioridade = ultimoEscolhido.Prioridade + 1;
                                }

                                dbModel.UserCursoes.Add(userCurso);

                                dbModel.SaveChanges();
                            }
                            else
                            {
                                return View();
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
                }

                ModelState.Clear();

                return RedirectToAction("Opcoes", "Formulario");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        //remove um exame seleccionado pelo utilizador
        public ActionResult removerExame(int id)
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    try
                    {
                        //remover exame
                        UserExame ue = dbModel.UserExames.Where(dp => dp.ExameId == id).Where(dp => dp.UserId == userId).FirstOrDefault();
                        dbModel.UserExames.Remove(ue);
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
                
                return RedirectToAction("Opcoes", "Formulario");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        //remove um curso seleccionado pelo utilizador
        public ActionResult removerCurso(int id)
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    try
                    {
                        //remover curso
                        UserCurso uc = dbModel.UserCursoes.Where(dp => dp.CursoId == id).Where(dp => dp.UserId == userId).FirstOrDefault();
                        dbModel.UserCursoes.Remove(uc);

                        dbModel.SaveChanges();

                        //actualizar prioridade dos restantes cursos
                        var cursosRestantes = dbModel.UserCursoes.Where(dp => dp.UserId == userId).OrderBy(dp => dp.ID).ToList();

                        int prioridadeNova = 1;

                        cursosRestantes.ForEach(a =>
                        {
                            a.Prioridade = prioridadeNova;
                            prioridadeNova++;
                        });

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
                return RedirectToAction("Opcoes", "Formulario");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        //dar mais prioridade a um curso escolhido
        public ActionResult moverParaCima(int id)
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    try
                    {
                        UserCurso corrente = dbModel.UserCursoes.Where(dp => dp.CursoId == id).Where(dp => dp.UserId == userId).FirstOrDefault();

                        int prioridadeCursoCorrente = corrente.Prioridade;

                        UserCurso anterior = dbModel.UserCursoes.Where(dp => dp.Prioridade == (prioridadeCursoCorrente - 1)).Where(dp => dp.UserId == userId).FirstOrDefault();

                        //verificar se existe um curso anterior
                        if (anterior == null)
                        {
                            return RedirectToAction("Opcoes", "Formulario");
                        }
                        else
                        {
                            corrente.Prioridade = corrente.Prioridade - 1;
                            anterior.Prioridade = anterior.Prioridade + 1;
                        }

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
                return RedirectToAction("Opcoes", "Formulario");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        //dar menos prioridade a um curso escolhido
        public ActionResult moverParaBaixo(int id)
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    try
                    {
                        UserCurso corrente = dbModel.UserCursoes.Where(dp => dp.CursoId == id).Where(dp => dp.UserId == userId).FirstOrDefault();

                        int prioridadeCursoCorrente = corrente.Prioridade;

                        UserCurso seguinte = dbModel.UserCursoes.Where(dp => dp.Prioridade == (prioridadeCursoCorrente + 1)).Where(dp => dp.UserId == userId).FirstOrDefault();

                        //verificar se existe um curso seguinte
                        if (seguinte == null)
                        {
                            return RedirectToAction("Opcoes", "Formulario");
                        }
                        else
                        {
                            corrente.Prioridade = corrente.Prioridade + 1;
                            seguinte.Prioridade = seguinte.Prioridade - 1;
                        }

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
                return RedirectToAction("Opcoes", "Formulario");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        public ActionResult SubmeterCandidatura()
        {
            if (Session["userID"] != null)
            {
                CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1();

                int userID = (int)Session["userID"];

                if (!dbModel.Candidatoes.Any(u => u.UserID == userID))
                {
                    try
                    {
                        Candidato candidato = new Candidato();
                        candidato.UserID = userID;
                        candidato.Sincronizado = false;

                        DateTime now = System.DateTime.Now;

                        string edicao = dbModel.Edicaos.Where(dp => now >= dp.DataInicio && now <= dp.DataFim).Select(dp => dp.Sigla).FirstOrDefault();

                        candidato.Edicao = edicao;

                        dbModel.Candidatoes.Add(candidato);
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
                else
                {
                    Candidato candidato = dbModel.Candidatoes.Where(u => u.UserID == userID).FirstOrDefault();
                    candidato.Sincronizado = false;

                    dbModel.Candidatoes.Add(candidato);
                    dbModel.SaveChanges();
                }

                // Create a MigraDoc document

                Document document = MigraDocument.CreateDocument("ComprovativoCandidatura", "Comprovativo de Candidatura", "Fábio Lourenço", 1, (int)Session["userID"]);

                MigraDoc.Rendering.DocumentRenderer renderer = new DocumentRenderer(document);
                PdfDocumentRenderer PDFRenderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always)
                {
                    Document = document
                };

                PDFRenderer.RenderDocument();
                PDFRenderer.DocumentRenderer = renderer;

                string filename = document.Info.Title + ".pdf";

                Form formTable = new Form();

                MemoryStream PDFStream = new MemoryStream();
                PDFRenderer.PdfDocument.Save(PDFStream, true);

                formTable.UserID = userID;
                formTable.FormBin = PDFStream.ToArray();
                formTable.DataCriação = System.DateTime.Now;

                dbModel.Forms.Add(formTable);
                dbModel.SaveChanges();

                ModelState.Clear();

                string utilizador = dbModel.Users.Where(dp => userID == dp.ID).Select(dp => dp.Email).FirstOrDefault();

                string subject = "Portal de Candidaturas à Escola Naval - Formulario ";

                int numeroCandidato = dbModel.Candidatoes.Where(u => u.UserID == userID).Select(dp => dp.Numero).FirstOrDefault();

                string body = "O utilizador com email " + utilizador + ", e número de candidato " + numeroCandidato + " submeteu um novo formulário com sucesso.";

                Email.SendEmail("sqimi.test@gmail.com", subject, body);

                ViewBag.Subtitle = "Novo Formulário submetido - ";
                ViewBag.Goto = "Welcome";
                ViewBag.ConfirmationMessage = "O formulário foi submetido com sucesso.\nPoderá agora aceder ao comprovativo de candidatura.";

                return View("~/Views/Shared/Success.cshtml");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

    }
}
