using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Candidaturas.Models;

namespace Candidaturas.Controllers
{
    public class InqueritoController : Controller
    {
        string situacaoPaiEscolhido = null;
        string situacaoMaeEscolhido = null;
        string conhecimentoEscolaEscolhido = null;

        // GET: Inquerito
        public ActionResult Index()
        {
            int userId = (int)Session["userID"];

            if (userId != 0)
            {
                this.getDadosPessoais(userId);
            }

            this.getDataForDropdownLists();

            return View("~/Views/Inquerito/AddOrEdit.cshtml");
        }

        //obtém os dados preenchdios pelo utilizador para mostrar no ecrã
        public void getDadosPessoais(int userId)
        {
            LoginDataBaseEntities1 db = new LoginDataBaseEntities1();
            Inquerito inqueritoUser = db.Inqueritoes.Where(dp => dp.UserId == userId).FirstOrDefault();

            if(inqueritoUser != null)
            {
                ViewBag.Inquerito = inqueritoUser;

                situacaoPaiEscolhido = inqueritoUser.SituacaoPai;
                situacaoMaeEscolhido = inqueritoUser.SituacaoMae;
                conhecimentoEscolaEscolhido = inqueritoUser.ConhecimentoEscola;
            }
        }

        //obtém os dados a serem preenchidos nas drops
        public void getDataForDropdownLists()
        {
            LoginDataBaseEntities1 db = new LoginDataBaseEntities1();

            IEnumerable<SelectListItem> situacoesPai = db.Situacaos.Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == situacaoPaiEscolhido
            });

            IEnumerable<SelectListItem> situacoesMae = db.Situacaos.Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == situacaoMaeEscolhido
            });

            IEnumerable<SelectListItem> conhecimentosEscola = db.ConhecimentoEscolas.Select(c => new SelectListItem
            {
                Value = c.Nome,
                Text = c.Nome,
                Selected = c.Nome == conhecimentoEscolaEscolhido
            });

            ViewBag.SituacaoPai = situacoesPai.ToList();
            ViewBag.SituacaoMae = situacoesMae.ToList();
            ViewBag.ConhecimentoEscola = conhecimentosEscola.ToList();
        }

        [HttpGet]
        // GET: User
        public ActionResult AddOrEdit(int id = 0)
        {
            Inquerito inqueritoModel = new Inquerito();
            return View(inqueritoModel);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Inquerito inqueritoModel)
        {
            int userId = (int)Session["userID"];

            using (LoginDataBaseEntities1 dbModel = new LoginDataBaseEntities1())
            {

                try
                {

                    Inquerito inqueritoUser = dbModel.Inqueritoes.Where(dp => dp.UserId == userId).FirstOrDefault();

                    inqueritoModel.UserId = userId;

                    if (inqueritoModel.SituacaoMae == null)
                    {
                        inqueritoModel.SituacaoMae = String.Empty;
                    }

                    if (inqueritoModel.SituacaoPai == null)
                    {
                        inqueritoModel.SituacaoPai = String.Empty;
                    }

                    if (inqueritoModel.ConhecimentoEscola == null)
                    {
                        inqueritoModel.ConhecimentoEscola = String.Empty;
                    }

                    if (inqueritoUser == null)
                    {
                        dbModel.Inqueritoes.Add(inqueritoModel);
                    }
                    else
                    {
                        dbModel.Inqueritoes.Remove(inqueritoUser);
                        inqueritoUser = inqueritoModel;
                        dbModel.Inqueritoes.Add(inqueritoUser);
                    }

                    dbModel.SaveChanges();

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            ModelState.Clear();

            Session["SelectedTab"] = 2;

            return RedirectToAction("Index", "Home");
        }
    }
}