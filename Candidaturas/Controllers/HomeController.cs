﻿using Candidaturas.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Candidaturas.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

            if (Session["SelectedTab"] == null)
            {
                if (Session["userID"] != null)
                {
                    int userId = (int)Session["userID"];
                    int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                    Certificado candidato = db.Certificadoes.Where(u => u.CandidaturaID == candidaturaId).FirstOrDefault();
                    ViewBag.Candidato = false;

                    if (candidato != null)
                    {
                        ViewBag.Candidato = true;
                        ViewBag.NumeroCandidato = candidaturaId;
                        Session["SelectedTab"] = 5;
                        return RedirectToAction("Index", "Home");


                    }
                    else
                    {
                        Session["SelectedTab"] = 1;
                    }
                }
                else
                {
                    Session["SelectedTab"] = 1;
                }
            }

           

                return View();
        }

        // GET: Welcome
        public ActionResult Welcome()
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            List<DocModel> DocumentosUser;
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                DocumentosUser = getSelectedDocumentos(db, userId);
                ViewBag.DocumentosUser = DocumentosUser;
                ViewBag.Formulario = FormIsCreated(db, userId);
                int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
                Certificado candidato = db.Certificadoes.Where(u => u.CandidaturaID == candidaturaId).FirstOrDefault();
                ViewBag.Candidato = false;

                if(candidato != null)
                {
                    ViewBag.Candidato = true;
                    ViewBag.NumeroCandidato = candidaturaId;
                }

                return View("~/Views/Home/Welcome.cshtml");
            }
            
            return View("~/Views/Login/Index.cshtml");
        }

        [HttpPost]
        public ActionResult UploadDocumento(DocModel docMod, System.Web.HttpPostedFileBase file)
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    try
                    {
                        int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();

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
                                    UploadTime = System.DateTime.Now
                                };

                                dbModel.Documentoes.Add(doc);

                                //criar Documento e o Binario
                                DocumentoBinario DocBin = new DocumentoBinario
                                {
                                    DocID = doc.ID,
                                    DocBinario = data
                                };
                                dbModel.DocumentoBinarios.Add(DocBin);
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
                
                return RedirectToAction("Welcome", "Home");
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

        public bool FormIsCreated(CandidaturaDBEntities1 db, int userId)
        {
            int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
            return db.Certificadoes.Where(dp => dp.CandidaturaID == candidaturaId).Any();

        }

        public ActionResult DownloadFormulario() {

            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            int userId = (int)Session["userID"];
            int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();
            byte[] dForm = db.Certificadoes.Where(dp => dp.CandidaturaID == candidaturaId).Select(dp => dp.FormBin).FirstOrDefault();

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
                        int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).Select(c => c.id).FirstOrDefault();

                        Certificado ud = dbModel.Certificadoes.Where(dp => dp.CandidaturaID == candidaturaId).FirstOrDefault();
                        dbModel.Certificadoes.Remove(ud);


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


                return RedirectToAction("DadosPessoais", "Formulario");
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
                        Documento ud = dbModel.Documentoes.Where(dp => dp.ID == id).FirstOrDefault();
                        dbModel.Documentoes.Remove(ud);

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


                return RedirectToAction("Welcome", "Home");
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
                return View("~/Views/Home/Welcome.cshtml");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

    }
}