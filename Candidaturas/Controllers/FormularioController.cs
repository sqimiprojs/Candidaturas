using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Candidaturas.Models;


namespace Candidaturas.Controllers
{
    public class FormularioController : Controller
    {
        
        int? generoEscolhido = null;
        int? tipoDocIdEscolhido = null;
        int? distritoNaturalEscolhido = null;
        int? concelhoNaturalEscolhido = null;
        int? freguesiaNaturalEscolhida = null;
        int? distritoMoradaEscolhido = null;
        int? concelhoMoradaEscolhido = null;
        int? freguesiaMoradaEscolhida = null;
        int? localidadeEscolhida = null;
        int? estadoCivilEscolhido = null;
        string nacionalidadeEscolhida = null;

        int? situacaoPaiEscolhido = null;
        int? situacaoMaeEscolhido = null;
        int? conhecimentoEscolaEscolhido = null;

        List<Exame> exames;
        List<CursoDisplay> cursos;

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
                int? tipoDocId = db.Users.Where(dp => dp.ID == userId).Select(dp => dp.TipoDocID).FirstOrDefault();

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
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

            IEnumerable<SelectListItem> generos = db.Generoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == generoEscolhido

            });
            ViewBag.Genero = generos.ToList();

            IEnumerable<SelectListItem> distritosNaturais = db.Distritoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == distritoNaturalEscolhido

            });
            ViewBag.DistritoNatural = distritosNaturais.ToList();

            IEnumerable<SelectListItem> concelhosNaturais = db.Concelhoes.Where(dp => dp.CodigoDistrito == distritoNatural).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == concelhoNaturalEscolhido

            });
            ViewBag.ConcelhoNatural = concelhosNaturais.ToList();

            IEnumerable<SelectListItem> freguesiasNaturais = db.Freguesias.Where(dp => dp.CodigoConcelho == concelhoNatural && dp.CodigoDistrito == distritoNatural).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == freguesiaNaturalEscolhida

            });
            ViewBag.FreguesiaNatural = freguesiasNaturais.ToList();

            IEnumerable<SelectListItem> distritosMoradas = db.Distritoes.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == distritoMoradaEscolhido

            });
            ViewBag.DistritoMorada = distritosMoradas.ToList();

            IEnumerable<SelectListItem> concelhosMoradas = db.Concelhoes.Where(dp => dp.CodigoDistrito == distritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == concelhoMoradaEscolhido

            });
            ViewBag.ConcelhoMorada = concelhosMoradas.ToList();

            IEnumerable<SelectListItem> freguesiasMoradas = db.Freguesias.Where(dp => dp.CodigoConcelho == concelhoMorada && dp.CodigoDistrito == distritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == freguesiaMoradaEscolhida

            });
            ViewBag.FreguesiaMorada = freguesiasMoradas.ToList();

            IEnumerable<SelectListItem> tiposDocumentosId = db.TipoDocumentoIDs.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == tipoDocIdEscolhido

            });
            ViewBag.TipoDocID = tiposDocumentosId.ToList();

            IEnumerable<SelectListItem> estadosCivis = db.EstadoCivils.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == estadoCivilEscolhido

            });
            ViewBag.EstadoCivil = estadosCivis.ToList();

            IEnumerable<SelectListItem> nacionalidades = db.Pais.OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Sigla,
                Text = c.Nome,
                Selected = c.Sigla == nacionalidadeEscolhida

            });
            ViewBag.Nacionalidade = nacionalidades.ToList();

            IEnumerable<SelectListItem> localidades = db.Localidades.Where(dp => dp.CodigoConcelho == concelhoMorada && dp.CodigoDistrito == distritoMorada).OrderBy(dp => dp.Nome).Select(c => new SelectListItem
            {
                Value = c.Codigo.ToString(),
                Text = c.Nome,
                Selected = c.Codigo == localidadeEscolhida

            });
            ViewBag.Localidade = localidades.ToList();

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

                this.getDataForDropdownLists(distritoNaturalEscolhido, concelhoNaturalEscolhido, distritoMoradaEscolhido, concelhoMoradaEscolhido);
            }

            return View("~/Views/Formulario/DadosPessoais.cshtml");
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

                situacaoPaiEscolhido = inqueritoUser.SituacaoPai;
                situacaoMaeEscolhido = inqueritoUser.SituacaoMae;
                conhecimentoEscolaEscolhido = inqueritoUser.ConhecimentoEscola;
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
                Selected = c.ID == situacaoPaiEscolhido
            });

            IEnumerable<SelectListItem> situacoesMae = db.Situacaos.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == situacaoMaeEscolhido
            });

            IEnumerable<SelectListItem> conhecimentosEscola = db.ConhecimentoEscolas.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == conhecimentoEscolaEscolhido
            });

            ViewBag.SituacaoPai = situacoesPai.ToList();
            ViewBag.SituacaoMae = situacoesMae.ToList();
            ViewBag.ConhecimentoEscola = conhecimentosEscola.ToList();
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

            if (Session["userID"] != null)
            {
                CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
                int userId = (int)Session["userID"];

                getDataForDropdownLists(db);
                getSelectedExames(db, userId);
                getSelectedCursos(db, userId);
            }

            dynamic mymodel = new ExpandoObject();
            mymodel.ExamesEscolhidos = exames;
            mymodel.CursosEscolhidos = cursos;

            return View(mymodel);
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
                                //Session["ErrorExame"] = 1;

                                Session["SelectedTab"] = 3;

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

                //Session["ErrorExame"] = 0;

                Session["SelectedTab"] = 3;

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
                                //Session["ErrorCurso"] = 1;

                                Session["SelectedTab"] = 3;

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

                //Session["ErrorCurso"] = 0;

                Session["SelectedTab"] = 3;

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

                Session["SelectedTab"] = 3;

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

                Session["SelectedTab"] = 3;

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

                Session["SelectedTab"] = 3;

                return RedirectToAction("Opcoes", "Formulario");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }


    }
}
