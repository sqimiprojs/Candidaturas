using Candidaturas.Models;
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

                    Candidato candidato = db.Candidatoes.Where(u => u.UserID == userId).FirstOrDefault();
                    ViewBag.Candidato = false;

                if (candidato != null)
                {
                    ViewBag.Candidato = true;
                    ViewBag.NumeroCandidato = candidato.Numero;
                }
                    return View("~/Views/Declaracao/Index.cshtml");
                
                }

            return View("~/Views/Login/Index.cshtml");
        }


        public ActionResult DownloadFormulario() {

            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            int userId = (int)Session["userID"];
            byte[] dForm = db.Forms.Where(dp => dp.UserID == userId).Select(dp => dp.FormBin).FirstOrDefault();

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
                        Form ud = dbModel.Forms.Where(dp => dp.UserID == userId).FirstOrDefault();
                        dbModel.Forms.Remove(ud);

                        Candidato candidato = dbModel.Candidatoes.Where(dp => dp.UserID == userId).FirstOrDefault();
                        dbModel.Candidatoes.Remove(candidato);

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
    
        

  

    }
}