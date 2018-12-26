using Candidaturas.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace Candidaturas.Controllers
{
    public class CandidaturasController : Controller
    {
        List<Exame> exames;
        List<CursoDisplay> cursos;

        // GET: Candidaturas
        public ActionResult Index()
        {

            exames = new List<Exame>();
            cursos = new List<CursoDisplay>();

            if(Session["userID"] != null)
            {
                LoginDataBaseEntities db = new LoginDataBaseEntities();
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

        //obtém os exames seleccionados pelo utilizador
        public void getSelectedExames(LoginDataBaseEntities db, int userId)
        {
            List<int> examesEscolhidos = db.UserExames.Where(dp => dp.UserId == userId).Select(dp => dp.ExameId).ToList();

            foreach (int exame in examesEscolhidos)
            {
                Exame ex = db.Exames.Where(dp => dp.ID == exame).FirstOrDefault();
                exames.Add(ex);
            }
        }

        //obtém os cursos seleccionados pelo utilizador
        public void getSelectedCursos(LoginDataBaseEntities db, int userId)
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
        public void getDataForDropdownLists(LoginDataBaseEntities db)
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

        //adiciona um exame seleccionado
        public ActionResult adicionarExame()
        {
            if(Session["userID"] != null)
            {
                int userId = (int)Session["userID"];
                int exameEscolhido = Convert.ToInt32(Request.Form["ExameSeleccionado"]);

                if (exameEscolhido != 0)
                {
                    using (LoginDataBaseEntities dbModel = new LoginDataBaseEntities())
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
            if(Session["userID"] != null)
            {
                int cursoEscolhido = Convert.ToInt32(Request.Form["CursoSeleccionado"]);
                int userId = (int)Session["userID"];

                if (cursoEscolhido != 0)
                {
                    using (LoginDataBaseEntities dbModel = new LoginDataBaseEntities())
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
            if(Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (LoginDataBaseEntities dbModel = new LoginDataBaseEntities())
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
            if(Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (LoginDataBaseEntities dbModel = new LoginDataBaseEntities())
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
            if(Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (LoginDataBaseEntities dbModel = new LoginDataBaseEntities())
                {
                    try
                    {
                        UserCurso corrente = dbModel.UserCursoes.Where(dp => dp.CursoId == id).Where(dp => dp.UserId == userId).FirstOrDefault();

                        int prioridadeCursoCorrente = corrente.Prioridade;

                        UserCurso anterior = dbModel.UserCursoes.Where(dp => dp.Prioridade == (prioridadeCursoCorrente - 1)).Where(dp => dp.UserId == userId).FirstOrDefault();

                        //verificar se existe um curso anterior
                        if (anterior == null)
                        {
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

                using (LoginDataBaseEntities dbModel = new LoginDataBaseEntities())
                {
                    try
                    {
                        UserCurso corrente = dbModel.UserCursoes.Where(dp => dp.CursoId == id).Where(dp => dp.UserId == userId).FirstOrDefault();

                        int prioridadeCursoCorrente = corrente.Prioridade;

                        UserCurso seguinte = dbModel.UserCursoes.Where(dp => dp.Prioridade == (prioridadeCursoCorrente + 1)).Where(dp => dp.UserId == userId).FirstOrDefault();

                        //verificar se existe um curso seguinte
                        if (seguinte == null)
                        {
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
    }
}