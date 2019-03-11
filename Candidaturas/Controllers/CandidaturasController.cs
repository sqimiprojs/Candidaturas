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
    public class CandidaturasController : Controller
    {
        List<Exame> exames;
        List<CursoDisplay> cursos;
        List<String> examesNecessarios;

        // GET: Candidaturas
        public ActionResult Index()
        {
            ViewBag.Title = "Opcoes";
            exames = new List<Exame>();
            cursos = new List<CursoDisplay>();
            examesNecessarios = new List<String>();

            if (Session["userID"] != null)
            {
                CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
                int userId = (int)Session["userID"];
                int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();

                getDataForDropdownLists(db);
                getSelectedExames(db, userId);
                getSelectedCursos(db, userId);

                dynamic mymodel = new ExpandoObject();
                mymodel.ExamesEscolhidos = exames;
                mymodel.CursosEscolhidos = cursos;
                ViewBag.CountCursosEscolhidos = cursos.Count;
                mymodel.ExamesNecessários = examesNecessarios;
                DadosPessoai dados = db.DadosPessoais.Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();
                Inquerito inquerito = db.Inqueritoes.Where(dp => dp.CandidaturaID == candidaturaId).FirstOrDefault();
                
                if(dados != null)
                {
                    ViewBag.dadosPreenchidos = true;
                }
                else
                {
                    ViewBag.dadosPreenchidos = false;
                }

                if (inquerito != null)
                {
                    ViewBag.inqueritoPreenchido = true;
                }
                else
                {
                    ViewBag.inqueritoPreenchido = false;
                }


                return View(mymodel);
            }

            return View("~/Views/Login/Index.cshtml");
        }

        //obtém os exames seleccionados pelo utilizador
        public void getSelectedExames(CandidaturaDBEntities1 db, int userId)
        {
            int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
            List<int> examesEscolhidos = db.UserExames.Where(dp => dp.CandidaturaId == candidaturaId).Select(dp => dp.ExameId).ToList();

            foreach (int exame in examesEscolhidos)
            {
                Exame ex = db.Exames.Where(dp => dp.ID == exame).FirstOrDefault();
                exames.Add(ex);
            }
        }

        //obtém os cursos seleccionados pelo utilizador
        public void getSelectedCursos(CandidaturaDBEntities1 db, int userId)
        {
            int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
            List<int> cursosEscolhidos = db.Opcoes.Where(dp => dp.CandidaturaId == candidaturaId).OrderBy(dp => dp.Prioridade).Select(dp => dp.CursoId).ToList();

            foreach (int curso in cursosEscolhidos)
            {
                CursoDisplay cd = new CursoDisplay();

                cd.nome = db.Cursoes.Where(dp => dp.ID == curso).Select(dp => dp.Nome).First();
                String siglaEdicao = db.Cursoes.Where(dp => dp.ID == curso).Select(dp => dp.Edicao).First();
                cd.prioridade = db.Opcoes.Where(dp => dp.CursoId == curso && dp.CandidaturaId == candidaturaId).Select(dp => dp.Prioridade).FirstOrDefault();
                cd.ID = curso;
                List<int> ExamesId = db.CursoExames.Where(ce => ce.CursoID == curso && ce.Edicao == siglaEdicao).Select(ce => ce.ExameID).ToList();
                List<ExameObrigatorioDisplay> auxiliar = new List<ExameObrigatorioDisplay>();
                foreach(int id in ExamesId)
                {
                    ExameObrigatorioDisplay nome = new ExameObrigatorioDisplay();
                    Exame exame = db.Exames.Where(e => e.ID == id && e.Edicao == siglaEdicao).FirstOrDefault();
                    bool obrigatorio = (bool) db.CursoExames.Where(ce => ce.CursoID == curso && ce.Edicao == siglaEdicao && ce.ExameID == id).Select(ce => ce.Obrigatorio).FirstOrDefault();
                    nome.exame = exame;
                    nome.obrigatorio = obrigatorio;
                    auxiliar.Add(nome);
                }
                cd.ExamesNecessarios = auxiliar;
                cursos.Add(cd);
          
            }
        }

        //obtém os dados a serem preenchidos nas drops (exames, cursos)
        public void getDataForDropdownLists(CandidaturaDBEntities1 db)
        {

            String siglaEdicao = db.Edicaos.Where(e => e.DataInicio < System.DateTime.Now && e.DataFim > System.DateTime.Now).Select(e => e.Sigla).FirstOrDefault();


            IEnumerable<SelectListItem> exames = db.Exames.Where(e => e.Edicao == siglaEdicao).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome
            });

            IEnumerable<SelectListItem> cursos = db.Cursoes.Where(c => c.Edicao == siglaEdicao).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome
            });

            ViewBag.Exame = exames.ToList();
            ViewBag.Curso = cursos.ToList();

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
                            int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                            UserExame exameRegistado = dbModel.UserExames.Where(dp => dp.ExameId == exameEscolhido).Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();
                            Historico novoHistorico = new Historico();

                            if (exameRegistado == null)
                            {
                                UserExame userExame = new UserExame();

                                userExame.ExameId = dbModel.Exames.Where(dp => dp.ID == exameEscolhido).FirstOrDefault().ID;
                                userExame.CandidaturaId = candidaturaId;

                                dbModel.UserExames.Add(userExame);
                                novoHistorico.timestamp = System.DateTime.Now;
                                novoHistorico.mensagem = "Exame: " + dbModel.Exames.Where(dp => dp.ID == exameEscolhido).FirstOrDefault().Nome + " adicionado.";
                                novoHistorico.CandidaturaID = candidaturaId;
                                dbModel.Historicoes.Add(novoHistorico);
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
                            int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                            Opco cursoRegistado = dbModel.Opcoes.Where(dp => dp.CursoId == cursoEscolhido).Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();
                            Historico novoHistorico = new Historico();
                            //ver se já escolheu o curso
                            if (cursoRegistado == null)
                            {
                                Opco userCurso = new Opco();

                                userCurso.CursoId = dbModel.Cursoes.Where(dp => dp.ID == cursoEscolhido).FirstOrDefault().ID;
                                userCurso.CandidaturaId = candidaturaId;


                                //actualizar prioridade
                                Opco ultimoEscolhido = dbModel.Opcoes.Where(dp => dp.CandidaturaId == candidaturaId).OrderByDescending(dp => dp.Prioridade).FirstOrDefault();

                                if (ultimoEscolhido == null)
                                {
                                    userCurso.Prioridade = 1;
                                }
                                else
                                {
                                    userCurso.Prioridade = ultimoEscolhido.Prioridade + 1;
                                }

                                dbModel.Opcoes.Add(userCurso);
                                novoHistorico.timestamp = System.DateTime.Now;
                                novoHistorico.mensagem = "Curso: " + dbModel.Cursoes.Where(dp => dp.ID == cursoEscolhido).FirstOrDefault().Nome + " escolhido.";
                                novoHistorico.CandidaturaID = candidaturaId;
                                dbModel.Historicoes.Add(novoHistorico);

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

                Session["SelectedTab"] = 3;

                return RedirectToAction("Index", "Home");
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
                        int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                        Historico novoHistorico = new Historico();
                        //remover exame
                        UserExame ue = dbModel.UserExames.Where(dp => dp.ExameId == id).Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();
                        dbModel.UserExames.Remove(ue);
                        novoHistorico.timestamp = System.DateTime.Now;
                        novoHistorico.mensagem = "Exame: " + dbModel.Exames.Where(dp => dp.ID == id).FirstOrDefault().Nome + " removido.";
                        novoHistorico.CandidaturaID = candidaturaId;
                        dbModel.Historicoes.Add(novoHistorico);
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

                return RedirectToAction("Index", "Home");
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
                        int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                        Historico novoHistorico = new Historico();
                        //remover curso
                        Opco uc = dbModel.Opcoes.Where(dp => dp.CursoId == id).Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();
                        dbModel.Opcoes.Remove(uc);

                        dbModel.SaveChanges();

                        //actualizar prioridade dos restantes cursos
                        var cursosRestantes = dbModel.Opcoes.Where(dp => dp.CandidaturaId == candidaturaId).OrderBy(dp => dp.ID).ToList();

                        int prioridadeNova = 1;

                        cursosRestantes.ForEach(a =>
                        {
                            a.Prioridade = prioridadeNova;
                            prioridadeNova++;
                        });
                        novoHistorico.timestamp = System.DateTime.Now;
                        novoHistorico.mensagem = "Curso: " + dbModel.Exames.Where(dp => dp.ID == id).FirstOrDefault().Nome + " removido.";
                        novoHistorico.CandidaturaID = candidaturaId;
                        dbModel.Historicoes.Add(novoHistorico);

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

                return RedirectToAction("Index", "Home");
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
                        int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();

                        Opco corrente = dbModel.Opcoes.Where(dp => dp.CursoId == id).Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();

                        int prioridadeCursoCorrente = corrente.Prioridade;

                        Opco anterior = dbModel.Opcoes.Where(dp => dp.Prioridade == (prioridadeCursoCorrente - 1)).Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();

                        //verificar se existe um curso anterior
                        if (anterior == null)
                        {
                            Session["SelectedTab"] = 3;

                            return RedirectToAction("Index", "Home");
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

                return RedirectToAction("Index", "Home");
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
                        int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();

                        Opco corrente = dbModel.Opcoes.Where(dp => dp.CursoId == id).Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();

                        int prioridadeCursoCorrente = corrente.Prioridade;

                        Opco seguinte = dbModel.Opcoes.Where(dp => dp.Prioridade == (prioridadeCursoCorrente + 1)).Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();

                        //verificar se existe um curso seguinte
                        if (seguinte == null)
                        {
                            Session["SelectedTab"] = 3;

                            return RedirectToAction("Index", "Home");
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

                return RedirectToAction("Index", "Home");
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
                Session["SelectedTab"] = 4;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

    }
}
