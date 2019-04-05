using Candidaturas.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Candidaturas.Controllers
{
    public class DeclaracaoController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

                if (Session["userID"] != null)
                {
                    int userId = (int)Session["userID"];
                    int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                    Certificado certificado = db.Certificadoes.Where(c => c.CandidaturaID == candidaturaId).FirstOrDefault();
                    ViewBag.Candidato = false;
                DadosPessoai dados = db.DadosPessoais.Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();
                Inquerito inquerito = db.Inqueritoes.Where(dp => dp.CandidaturaID == candidaturaId).FirstOrDefault();
                int opcoes = db.Opcoes.Where(dp => dp.CandidaturaId == candidaturaId).Count();

                if (dados != null)
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
                if (opcoes > 0)
                {
                    ViewBag.opcoesEscolhidas = true;
                }
                else
                {
                    ViewBag.opcoesEscolhidas = false;
                }

                if (certificado != null)
                {
                    ViewBag.Candidato = true;
                    ViewBag.NumeroCandidato = candidaturaId;
                }
                    return View("~/Views/Declaracao/Index.cshtml");
                
                }

            return View("~/Views/Login/Index.cshtml");
        }


        public ActionResult DownloadFormulario() {

            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            int userId = (int)Session["userID"];
            int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
            byte[] dForm = db.Certificadoes.Where(dp => dp.CandidaturaID == candidaturaId).Select(dp => dp.FormBin).FirstOrDefault();
            Historico novoHistorico = new Historico();
            novoHistorico.timestamp = System.DateTime.Now;
            novoHistorico.mensagem = "Download de certificado efectuado.";
            novoHistorico.CandidaturaID = candidaturaId;
            db.Historicoes.Add(novoHistorico);
            db.SaveChanges();

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.Private);
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "ComprovativoCandidatura.pdf");
            Response.BinaryWrite(dForm);
            Response.Flush();
            Response.End();
        
            return View("~/Views/Home/Welcome.cshtml");
        }

        public ActionResult RepeatFormulario()
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    try
                    {
                        Historico novoHistorico = new Historico();
                        int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                        Certificado ud = dbModel.Certificadoes.Where(dp => dp.CandidaturaID == candidaturaId).FirstOrDefault();
                        dbModel.Certificadoes.Remove(ud);
                        novoHistorico.timestamp = System.DateTime.Now;
                        novoHistorico.mensagem = "Candidatura: " + candidaturaId + " e Certificado removidos.";
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


                Session["SelectedTab"] = 1;

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
                int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userID).Select(c => c.id).FirstOrDefault();
                Candidatura candidaturaAux = dbModel.Candidaturas.Find(candidaturaId);
                DadosPessoai dadosAux = dbModel.DadosPessoais.Where(d => d.CandidaturaId == candidaturaId).FirstOrDefault();
                List<Opco> opcoesAux = dbModel.Opcoes.Where(o => o.CandidaturaId == candidaturaId).ToList();
                Inquerito inqueritoAux = dbModel.Inqueritoes.Where(i => i.CandidaturaID == candidaturaId).FirstOrDefault();
                List<Documento> documentosAux = dbModel.Documentoes.Where(d => d.CandidaturaID == candidaturaId).ToList();



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

                Certificado formTable = new Certificado();
                Historico novoHistorico = new Historico();
                MemoryStream PDFStream = new MemoryStream();
                PDFRenderer.PdfDocument.Save(PDFStream, true);

                formTable.CandidaturaID = candidaturaId;
                formTable.FormBin = PDFStream.ToArray();
                formTable.DataCriação = System.DateTime.Now;
                formTable.DiaCriação = System.DateTime.Now.Date;
                dbModel.Certificadoes.Add(formTable);
                novoHistorico.timestamp = System.DateTime.Now;
                novoHistorico.mensagem = "Candidatura: " + candidaturaId + " finalizada e Certificado gerado.";
                novoHistorico.CandidaturaID = candidaturaId;
                dbModel.Historicoes.Add(novoHistorico);
                dbModel.SaveChanges();

                ModelState.Clear();

                string utilizador = dbModel.Users.Where(dp => userID == dp.ID).Select(dp => dp.Email).FirstOrDefault();

                string subject = "Portal de Candidaturas à Escola Naval - Formulario ";

                int numeroCandidato = candidaturaId;
                bool opcaoDesatualizada = false;
                bool docDesatualizado = false;
                foreach (Opco opcao in opcoesAux)
                {
                    if (candidaturaAux.DataFinalizacao < opcao.Data)
                    {
                        opcaoDesatualizada = true;
                    }
                }

                foreach (Documento doc in documentosAux)
                {
                    if (candidaturaAux.DataFinalizacao < doc.DataAualizacao)
                    {
                        docDesatualizado = true;
                    }
                }

                string body = "";
                if (candidaturaAux.DataFinalizacao == null)
                {
                    body = "O utilizador com email " + utilizador + " e número de candidato " + numeroCandidato + ", submeteu um novo formulário com sucesso.";
                    Email.SendEmail("tiago.castanho@sqimi.com", subject, body);
                }
                else
                {
                    var send = false;
                    body = "O utilizador com email " + utilizador + " e número de candidato " + numeroCandidato + ", editou a sua candidatura e alterou os seguintes parametros: \n";
                    if (candidaturaAux.DataFinalizacao < dadosAux.DataUltimaAtualizacao) {
                        body = body + "Dados Pessoais; \n";
                        send = true;
                    }
                    if (candidaturaAux.DataFinalizacao < inqueritoAux.DataAtualizacao) {
                        body = body + "Inquérito; \n";
                        send = true;
                    }
                    if (opcaoDesatualizada) {
                        body = body + "Opções; \n";
                        send = true;
                    }
                    if (docDesatualizado) {
                        body = body + "Documentos; \n";
                        send = true;
                    }
                    if (send) Email.SendEmail("tiago.castanho@sqimi.com", subject, body);
                }

                ViewBag.Subtitle = "Novo Formulário submetido - ";
                ViewBag.Goto = "Welcome";
                ViewBag.ConfirmationMessage = "O formulário foi submetido com sucesso.\nPoderá agora aceder ao comprovativo de candidatura.";

                candidaturaAux.DataFinalizacao = System.DateTime.Now;
                dbModel.SaveChanges();

                Session["SelectedTab"] = 5;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }





    }
}