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

                getDataForDropdownLists(db);
                getSelectedExames(db, userId);
                getSelectedCursos(db, userId);

                dynamic mymodel = new ExpandoObject();
                mymodel.ExamesEscolhidos = exames;
                mymodel.CursosEscolhidos = cursos;
                ViewBag.CountCursosEscolhidos = cursos.Count;
                mymodel.ExamesNecessários = examesNecessarios;

                return View(mymodel);
            }

            return View("~/Views/Login/Index.cshtml");
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
                cd.Exames = (db.Cursoes.Where(dp => dp.ID == curso).FirstOrDefault()).Exames.Select(dp => dp.Nome).ToList();
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
                        UserCurso corrente = dbModel.UserCursoes.Where(dp => dp.CursoId == id).Where(dp => dp.UserId == userId).FirstOrDefault();

                        int prioridadeCursoCorrente = corrente.Prioridade;

                        UserCurso anterior = dbModel.UserCursoes.Where(dp => dp.Prioridade == (prioridadeCursoCorrente - 1)).Where(dp => dp.UserId == userId).FirstOrDefault();

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
                        UserCurso corrente = dbModel.UserCursoes.Where(dp => dp.CursoId == id).Where(dp => dp.UserId == userId).FirstOrDefault();

                        int prioridadeCursoCorrente = corrente.Prioridade;

                        UserCurso seguinte = dbModel.UserCursoes.Where(dp => dp.Prioridade == (prioridadeCursoCorrente + 1)).Where(dp => dp.UserId == userId).FirstOrDefault();

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
