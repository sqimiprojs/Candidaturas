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
            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                this.getDadosPessoais(userId);
            }

            this.getDataForDropdownLists();

            return View("~/Views/Inquerito/AddOrEdit.cshtml");
        }

        //obtém os dados preenchdios pelo utilizador para mostrar no ecrã
        public void getDadosPessoais(int userId)
        {
            LoginDataBaseEntities db = new LoginDataBaseEntities();
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
            LoginDataBaseEntities db = new LoginDataBaseEntities();

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

        [HttpPost]
        public ActionResult AddOrEdit(Inquerito inqueritoModel)
        {
            if(Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                using (LoginDataBaseEntities dbModel = new LoginDataBaseEntities())
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
                            inqueritoModel.DataCriacao = System.DateTime.Now;
                            inqueritoModel.DataAtualizacao = System.DateTime.Now;
                            dbModel.Inqueritoes.Add(inqueritoModel);
                        }
                        else
                        {
                            DateTime? dataCriacao = inqueritoUser.DataCriacao;
                            dbModel.Inqueritoes.Remove(inqueritoUser);
                            inqueritoUser = inqueritoModel;
                            inqueritoUser.DataCriacao = dataCriacao;
                            inqueritoUser.DataAtualizacao = System.DateTime.Now;
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
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
        }
    }
}