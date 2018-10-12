using Candidaturas.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Candidaturas.Controllers
{
    public class DocumentosController : Controller
    {
        List<Documento> documentos;

        // GET: Documentos
        public ActionResult Index()
        {
            documentos = new List<Documento>();

            LoginDataBaseEntities1 db = new LoginDataBaseEntities1();

            if(Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                getSelectedDocumentos(db, userId);

                ViewBag.DocumentosEscolhidos = documentos;
            }

            return View("~/Views/Documentos/Index.cshtml");
        }

        //obtém os documentos adicionados pelo utilizador
        public void getSelectedDocumentos(LoginDataBaseEntities1 db, int userId)
        {
            List<int> documentosEscolhidos = db.UserDocumentoes.Where(dp => dp.UserId == userId).Select(dp => dp.DocumentoId).ToList();

            foreach (int documento in documentosEscolhidos)
            {
                Documento doc = db.Documentoes.Where(dp => dp.ID == documento).FirstOrDefault();
                documentos.Add(doc);
            }
        }

        //verifica se o ficheiro tem extensão JPEG
        public Boolean HasJpegExtension(HttpPostedFileBase file)
        {
            return Path.GetExtension(file.FileName) == ".jpeg" || Path.GetExtension(file.FileName) == ".jpg";
        }

        //verifica se o ficheiro tem extensão PDF
        public Boolean HasPdfExtension(HttpPostedFileBase file)
        {
            return Path.GetExtension(file.FileName) == ".pdf";
        }

        //verifica se o ficheiro tem cabeçalho JPEG
        public bool HasJpegHeader(HttpPostedFileBase file)
        {
            BinaryReader br = new BinaryReader(file.InputStream);
            UInt16 soi = br.ReadUInt16();  // Start of Image (SOI) marker (FFD8)
            UInt16 marker = br.ReadUInt16(); // JFIF marker (FFE0) or EXIF marker(FF01)

            return soi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff;
        }

        //verifica se o ficheiro tem cabeçalho PDF
        public bool HasPdfHeader(HttpPostedFileBase file)
        {
            var pdfString = "%PDF-";
            var pdfBytes = Encoding.ASCII.GetBytes(pdfString);
            var len = pdfBytes.Length;
            var buf = new byte[len];
            var remaining = len;
            var pos = 0;
            var f = file.InputStream;

            while (remaining > 0)
            {
                var amtRead = f.Read(buf, pos, remaining);
                if (amtRead == 0) return false;
                remaining -= amtRead;
                pos += amtRead;
            }

            return pdfBytes.SequenceEqual(buf);
        }

        //verifica se o ficheiro é do tipo JPEG
        public bool IsJpeg(HttpPostedFileBase file)
        {
            return (this.HasJpegExtension(file) && this.HasJpegHeader(file));
        }

        //verifica se o ficheiro é do tipo PDF
        public bool IsPdf(HttpPostedFileBase file)
        {
            return (this.HasPdfExtension(file) && this.HasPdfHeader(file));
        }

        [HttpPost]
        public ActionResult Upload(Documento documento, HttpPostedFileBase file)
        {
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (LoginDataBaseEntities1 dbModel = new LoginDataBaseEntities1())
                {
                    try
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            if (IsJpeg(file) || IsPdf(file))
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
                                Session["ErrorDoc"] = 1;

                                Session["SelectedTab"] = 4;

                                return RedirectToAction("Index", "Home");
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

                Session["ErrorDoc"] = 0;

                Session["SelectedTab"] = 4;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        //remove um documento adicionado pelo utilizador
        public ActionResult removerDocumento(int id)
        {
            if(Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (LoginDataBaseEntities1 dbModel = new LoginDataBaseEntities1())
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

                Session["SelectedTab"] = 4;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }

        //descarrega um documento adicionado pelo utilizador
        public ActionResult DescarregarDocumento(int id)
        {
            if (Session["userID"] != null)
            {
                using (LoginDataBaseEntities1 dbModel = new LoginDataBaseEntities1())
                {
                    try
                    {
                        Documento doc = dbModel.Documentoes.Where(dp => dp.ID == id).FirstOrDefault();

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
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