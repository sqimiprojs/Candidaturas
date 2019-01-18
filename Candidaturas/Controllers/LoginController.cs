using Candidaturas.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Diagnostics;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using System.Drawing;
using System.Reflection;
using PdfSharp.Pdf;

namespace Candidaturas.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Authorize(Login model)
        {

            using (CandidaturaDBEntities1 db = new CandidaturaDBEntities1())
            {
                //TODO Tratar cado de password vir null
                byte[] hashedUserPassword = new byte[0];

                using (SHA256 mySHA256 = SHA256.Create())
                {
                    hashedUserPassword = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(model.passwordInput));
                }

                var userDetails = db.Users.Where(x => x.Email == model.user.Email && x.Password == hashedUserPassword).FirstOrDefault();

                if (userDetails == null)
                {
                    model.user.LoginErrorMessage = "Username ou password errado.";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    Session["userID"] = userDetails.ID;
                    return RedirectToAction("Welcome", "Home");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }


        public ActionResult PDFGen()
        {
            // Create a MigraDoc document
            Document document = MigraDocument.CreateDocument("blank", "Comprovativo de Candidatura", "Fábio Lourenço", 1);

            //string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
            //MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");

            MigraDoc.Rendering.DocumentRenderer renderer = new DocumentRenderer(document);
            PdfDocumentRenderer PDFRenderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always)
            {
                Document = document
            };

            PDFRenderer.RenderDocument();
            PDFRenderer.DocumentRenderer = renderer;
                        
            string filename = document.Info.Title+".pdf";
            
            // Send PDF to browser
            MemoryStream PDFStream = new MemoryStream();
            PDFRenderer.PdfDocument.Save(PDFStream, false);
            Response.Clear();
            Response.GetType();
            Response.ContentType = document.GetType().ToString();
            Response.Cache.SetCacheability(System.Web.HttpCacheability.Private);
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.BinaryWrite(PDFStream.ToArray());
            Response.Flush();
            PDFStream.Close();
            Response.End();
            
            return View();
        }
    }
}