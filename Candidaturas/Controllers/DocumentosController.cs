using Candidaturas.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Candidaturas.Controllers
{
    public class DocumentosController : Controller
    {

        // GET: Welcome
        public ActionResult Index()
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            List<DocModel> DocumentosUser;
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];
                Edicao edicao = db.Edicaos.Where(e => e.DataInicio < System.DateTime.Now && e.DataFim > System.DateTime.Now).First();
                List<DocumentosNecessario> documentosNecessarios = db.DocumentosNecessarios.Where(dn => dn.Edicao == edicao.Sigla).ToList();
                ViewBag.DocumentosNecessarios = documentosNecessarios;
                DocumentosUser = getSelectedDocumentos(db, userId);
                ViewBag.DocumentosUser = DocumentosUser;
                int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                DadosPessoai dados = db.DadosPessoais.Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();
                Certificado certificado = db.Certificadoes.Where(c => c.CandidaturaID == candidaturaId).FirstOrDefault();
                if (certificado != null)
                {
                    ViewBag.finalizado = true;
                }
                else
                {
                    ViewBag.finalizado = false;
                }
                if (dados != null)
                {
                    ViewBag.dadosPreenchidos = true;
                }
                else
                {
                    ViewBag.dadosPreenchidos = false;
                }

                return View("~/Views/Documentos/Index.cshtml");
            }
            
            return View("~/Views/Login/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Upload(DocModel docMod, System.Web.HttpPostedFileBase file)
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    try
                    {
                        int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                        Candidatura aux = dbModel.Candidaturas.Where(c => c.UserId == userId).FirstOrDefault();
                        if (file != null && file.ContentLength > 0 && file.ContentLength < Constants.MaxFile*1024*1024)
                        {
                            if (DocumentValidator.IsJpeg(file) || DocumentValidator.IsPdf(file))
                            {
                                if (file.FileName.Length < 50) { 

                                file.InputStream.Seek(0, SeekOrigin.Begin);

                                string fileName = Path.GetFileName(file.FileName);
                                var fileType = file.ContentType;

                                MemoryStream target = new MemoryStream();
                                file.InputStream.CopyTo(target);
                                byte[] data = target.ToArray();

                                //criar documento
                                Documento doc = new Documento
                                {
                                    Descricao = docMod.DocumentoInfo.Descricao,
                                    Nome = fileName,
                                    Tipo = fileType,
                                    CandidaturaID = candidaturaId,
                                    UploadTime = System.DateTime.Now,
                                    DataAualizacao = System.DateTime.Now
                                };

                                dbModel.Documentoes.Add(doc);

                                //criar Documento e o Binario
                                DocumentoBinario DocBin = new DocumentoBinario
                                {
                                    DocID = doc.ID,
                                    DocBinario = data
                                };
                                    Historico novoHistorico = new Historico();
                                dbModel.DocumentoBinarios.Add(DocBin);
                                    novoHistorico.timestamp = System.DateTime.Now;
                                    novoHistorico.mensagem = "Documento: " + fileName + " adicionado.";
                                    novoHistorico.CandidaturaID = candidaturaId;
                                    dbModel.Historicoes.Add(novoHistorico);
                                    aux.DataAlteracao = System.DateTime.Now;
                                    dbModel.SaveChanges();
                                }
                                else
                                {
                                    TempData["LogError"] = "Nome do documento muito longo, máximo são 50 caracteres";
                                }
                            }
                            else
                            {
                                TempData["LogError"] = "Documento tem de ser do tipo .jpeg ou .pdf";

                            }
                        }
                        else {
                            TempData["LogError"] = "Ficheiro com tamanho inválido, máximo é "+Constants.MaxFile+"MB";
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
                ModelState.Clear();

                Session["SelectedTab"] = 4;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        public List<DocModel> getSelectedDocumentos(CandidaturaDBEntities1 db, int userId)
        {
            int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
            List<int> documentosUploaded = db.Documentoes
                                        .Where(dp => dp.CandidaturaID == candidaturaId)
                                        .Select(dp => dp.ID).ToList();
            List<DocModel> dm = new List<DocModel>();
            foreach (int documento in documentosUploaded)
            {
                DocModel doc= new DocModel();
                doc.DocumentoInfo = db.Documentoes.Where(dp => dp.ID == documento).FirstOrDefault();
                dm.Add(doc);
            }
            return dm;
        }
        
        public ActionResult Continuar()
        {
            if (Session["userID"] != null)
            {
                Session["SelectedTab"] = 5;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        //remove um documento adicionado pelo utilizador
        public ActionResult RemoveDocumento(int id)
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    try
                    {
                        int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                        Documento ud = dbModel.Documentoes.Where(dp => dp.ID == id).FirstOrDefault();
                        Candidatura aux = dbModel.Candidaturas.Where(c => c.UserId == userId).FirstOrDefault();
                        Historico novoHistorico = new Historico();
                        dbModel.Documentoes.Remove(ud);
                        novoHistorico.timestamp = System.DateTime.Now;
                        novoHistorico.mensagem = "Documento: " + ud.Nome + " removido.";
                        novoHistorico.CandidaturaID = candidaturaId;
                        dbModel.Historicoes.Add(novoHistorico);
                        aux.DataAlteracao = System.DateTime.Now;
                        dbModel.SaveChanges();


                        List<Documento> restantes = dbModel.Documentoes.Where(d => d.CandidaturaID == candidaturaId).ToList();
                        restantes.ForEach(r =>
                        {
                            r.DataAualizacao = System.DateTime.Now;
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

                Session["SelectedTab"] = 4;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        //descarrega um documento adicionado pelo utilizador
        public ActionResult DownloadDocumento(int id)
        {
            if (Session["userID"] != null)
            {
                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    try
                    {
                        Documento doc = dbModel.Documentoes.Where(dp => dp.ID == id).FirstOrDefault();
                        DocumentoBinario docbin = dbModel.DocumentoBinarios.Where(dp => dp.DocID == id).FirstOrDefault();

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.Cache.SetCacheability(System.Web.HttpCacheability.Private);
                        Response.ContentType = doc.Tipo;
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + doc.Nome);
                        Response.BinaryWrite(docbin.DocBinario);
                        Response.Flush();
                        Response.End();
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