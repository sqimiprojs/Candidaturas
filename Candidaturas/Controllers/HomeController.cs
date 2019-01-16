using Candidaturas.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Candidaturas.Controllers
{
    public class HomeController : Controller
    {
        List<Documento> documentos;
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }



        // GET: Welcome
        public ActionResult Welcome()
        {
            documentos = new List<Documento>();

            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                getSelectedDocumentos(db, userId);

                ViewBag.DocumentosEscolhidos = documentos;
            }
            
            return View("~/Views/Home/Welcome.cshtml");
        }

        [HttpPost]
        public ActionResult UploadDocumento(Documento documento, System.Web.HttpPostedFileBase file)
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {
                    try
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            if (DocumentValidator.IsJpeg(file) || DocumentValidator.IsPdf(file))
                            {
                                file.InputStream.Seek(0, SeekOrigin.Begin);

                                var fileName = Path.GetFileName(file.FileName);
                                var fileType = file.ContentType;

                                MemoryStream target = new MemoryStream();
                                file.InputStream.CopyTo(target);
                                byte[] data = target.ToArray();

                                //criar documento
                                Documento doc = new Documento();

                                doc.Descricao = documento.Descricao;
                                doc.Nome = fileName;
                                doc.Ficheiro = data;
                                doc.Tipo = fileType;

                                dbModel.Documentoes.Add(doc);
                                dbModel.SaveChanges();

                                //criar userDocumento (relação entre user e documento)
                                UserDocumento userDocumento = new UserDocumento();

                                userDocumento.UserId = userId;
                                userDocumento.DocumentoId = doc.ID;

                                dbModel.UserDocumentoes.Add(userDocumento);
                                dbModel.SaveChanges();
                            }
                            else
                            {
                                return View("~/Views/Home/Welcome.cshtml");
                            }
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

                return View("~/Views/Home/Welcome.cshtml");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        public void getSelectedDocumentos(CandidaturaDBEntities1 db, int userId)
        {
            List<int> documentosEscolhidos = db.UserDocumentoes.Where(dp => dp.UserId == userId).Select(dp => dp.DocumentoId).ToList();

            foreach (int documento in documentosEscolhidos)
            {
                Documento doc = db.Documentoes.Where(dp => dp.ID == documento).FirstOrDefault();
                documentos.Add(doc);
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
                        UserDocumento ud = dbModel.UserDocumentoes.Where(dp => dp.DocumentoId == id).Where(dp => dp.UserId == userId).FirstOrDefault();
                        dbModel.UserDocumentoes.Remove(ud);

                        Documento doc = dbModel.Documentoes.Where(dp => dp.ID == id).FirstOrDefault();
                        dbModel.Documentoes.Remove(doc);

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


                return View("~/Views/Home/Welcome.cshtml");
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

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                        Response.ContentType = doc.Tipo;
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + doc.Nome);
                        Response.BinaryWrite(doc.Ficheiro);
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