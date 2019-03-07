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
    public class InqueritoController : Controller
    {
        Inquerito InqEscolha = new Inquerito();

        // GET: Inquerito
        public ActionResult Index()
        {
            ViewBag.Title = "Inquerito";
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

            if (Session["userID"] != null)
            {
                int userId = (int)Session["userID"];

                this.getDadosPessoaisInquerito(userId);

                int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).FirstOrDefault().id;
                DadosPessoai dados = db.DadosPessoais.Where(dp => dp.CandidaturaId == candidaturaId).FirstOrDefault();
                if (dados != null)
                {
                    ViewBag.dadosPreenchidos = true;
                }
                else
                {
                    ViewBag.dadosPreenchidos = false;
                }
            }
            
            this.getDataForDropdownListsInquerito();

            return View("~/Views/Inquerito/AddOrEdit.cshtml");
        }

        //obtém os dados preenchdios pelo utilizador para mostrar no ecrã
        public void getDadosPessoaisInquerito(int userId)
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            int candidaturaId = db.Candidaturas.Where(c => c.UserId == userId).FirstOrDefault().id;
            Inquerito inqueritoUser = db.Inqueritoes.Where(dp => dp.CandidaturaID == candidaturaId).FirstOrDefault();

            if (inqueritoUser != null)
            {
                ViewBag.Inquerito = inqueritoUser;
                InqEscolha = inqueritoUser;
            }
        }

        //obtém os dados a serem preenchidos nas drops
        public void getDataForDropdownListsInquerito()
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            String siglaEdicao = db.Edicaos.Where(e => e.DataInicio < System.DateTime.Now && e.DataFim > System.DateTime.Now).Select(e => e.Sigla).FirstOrDefault();

            IEnumerable<SelectListItem> situacoesPai = db.Situacaos.Where(s => s.Edicao == siglaEdicao ).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == InqEscolha.SituacaoPai
            });
            ViewBag.SituacaoPai = situacoesPai;

            IEnumerable<SelectListItem> situacoesMae = db.Situacaos.Where(s => s.Edicao == siglaEdicao).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == InqEscolha.SituacaoMae
            });
            ViewBag.SituacaoMae = situacoesMae;

            IEnumerable<SelectListItem> conhecimentosEscola = db.ConhecimentoEscolas.Where(ce => ce.Edicao == siglaEdicao).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Nome,
                Selected = c.ID == InqEscolha.ConhecimentoEscola
            });
            ViewBag.ConhecimentoEscola = conhecimentosEscola;
        }

        [HttpPost]
        public ActionResult Inquerito(Inquerito inqueritoModel) { 
            
            if (Session["userID"] == null)
                return RedirectToAction("LogOut", "Login");
            else
            {
                {
                int userId = (int)Session["userID"];

                using (CandidaturaDBEntities1 dbModel = new CandidaturaDBEntities1())
                {

                    try
                    {
                            int candidaturaId = dbModel.Candidaturas.Where(c => c.UserId == userId).FirstOrDefault().id;
                            Inquerito inqueritoUser = dbModel.Inqueritoes.Where(dp => dp.CandidaturaID == candidaturaId).FirstOrDefault();

                        inqueritoModel.CandidaturaID = candidaturaId;
                            Historico novoHistorico = new Historico();

                            if (inqueritoUser == null)
                        {
                            inqueritoModel.DataCriacao = System.DateTime.Now;
                            inqueritoModel.DataAtualizacao = System.DateTime.Now;
                            dbModel.Inqueritoes.Add(inqueritoModel);
                                novoHistorico.timestamp = System.DateTime.Now;
                                novoHistorico.mensagem = "Inquerito respondido.";
                                novoHistorico.CandidaturaID = candidaturaId;
                                dbModel.Historicoes.Add(novoHistorico);
                            }
                        else
                        {
                            DateTime? dataCriacao = inqueritoUser.DataCriacao;
                            dbModel.Inqueritoes.Remove(inqueritoUser);
                            inqueritoUser = inqueritoModel;
                            inqueritoUser.DataCriacao = dataCriacao;
                            inqueritoUser.DataAtualizacao = System.DateTime.Now;
                            dbModel.Inqueritoes.Add(inqueritoUser);
                                novoHistorico.timestamp = System.DateTime.Now;
                                novoHistorico.mensagem = "Inquerito alterado.";
                                novoHistorico.CandidaturaID = candidaturaId;
                                dbModel.Historicoes.Add(novoHistorico);
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

                Session["SelectedTab"] = 3;

                return RedirectToAction("Index", "Home");
            }
            
            }
        }
    }
}
