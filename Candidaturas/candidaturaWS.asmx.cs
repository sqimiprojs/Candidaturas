using Candidaturas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace Candidaturas
{
    /// <summary>
    /// Summary description for candidaturaWS
    /// </summary>
    [WebService(Namespace = "https://sqimi.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class candidaturaWS : System.Web.Services.WebService
    {
        private CandidatoDTO GetInfoCandidate(CandidaturaDBEntities1 db, int user) {

            CandidatoDTO dape = new CandidatoDTO();
            int candidaturaId = db.Candidaturas.Where(c => c.UserId == user).Select(c => c.id).FirstOrDefault();
            dape.dadosDTO = db.DadosPessoais
                .Where(guy => guy.CandidaturaId == candidaturaId)
                .Select(data => new DadosPessoaisDTO
                {
                    NomeColoquial = data.NomeColoquial,
                    Nomes = data.Nomes,
                    Apelidos = data.Apelidos,
                    NomePai = data.NomePai,
                    NomeMae = data.NomeMae,
                    NDI = data.NDI,
                    TipoDocID = data.TipoDocID,
                    Genero = data.Genero,
                    EstadoCivil = data.EstadoCivil,
                    Nacionalidade = data.Nacionalidade,
                    DistritoNatural = data.DistritoNatural,
                    ConcelhoNatural = data.ConcelhoNatural,
                    FreguesiaNatural = data.FreguesiaNatural,
                    Morada = data.Morada,
                    Localidade = data.Localidade,
                    RepFinNIF = data.RepFinNIF,
                    CCDigitosControlo = data.CCDigitosControlo,
                    NSegSoc = data.NSegSoc,
                    NIF = data.NIF,
                    DistritoMorada = data.DistritoMorada,
                    ConcelhoMorada = data.ConcelhoMorada,
                    FreguesiaMorada = data.FreguesiaMorada,
                    Telefone = data.Telefone,
                    CodigoPostal4Dig = data.CodigoPostal4Dig,
                    CodigoPostal3Dig = data.CodigoPostal3Dig,
                    DataCriacao = data.DataCriacao,
                    DataUltimaAtualizacao = data.DataUltimaAtualizacao,
                    DataNascimento = data.DataNascimento,
                    Militar = data.Militar,
                    Ramo = data.Ramo,
                    Categoria = data.Categoria,
                    Posto = data.Posto,
                    Classe = data.Classe,
                    NIM = data.NIM
                })
                .FirstOrDefault();

            dape.inqueritoDTO = db.Inqueritoes
                .Where(guy => guy.CandidaturaID == candidaturaId)
                .Select(data => new InqueritoDTO
                {
                    SituacaoPai = data.SituacaoPai,
                    OutraPai = data.OutraPai,
                    SituacaoMae = data.SituacaoMae,
                    OutraMae = data.OutraMae,
                    ConhecimentoEscola = data.ConhecimentoEscola,
                    CandidatarOutros = data.CandidatarOutros,
                    Outro = data.Outro
                })
                .FirstOrDefault();

            dape.cursosDTO = db.Opcoes
                .Where(guy => guy.CandidaturaId == candidaturaId)
                .Select(data => new UserCursoDTO
                {
                    CursoId = data.CursoId,
                    Prioridade = data.Prioridade
                })
                .ToList();

            dape.examesDTO = db.UserExames
                .Where(guy => guy.CandidaturaId == candidaturaId)
                .Select(data => new UserExameDTO
                {
                    ExameId = data.ExameId
                })
                .ToList();

            return dape;
        }
        
        [WebMethod]
        public List<CandidatoDTO> GetAllCandidates()
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();

            List<int> us = db.Users.Select(data => data.ID).ToList();
            CandidatoDTO dape = new CandidatoDTO();
            List<CandidatoDTO> Ldape = new List<CandidatoDTO>();

            foreach(int u in us)
            {
                Ldape.Add(GetInfoCandidate(db, u));
            }
            return Ldape;
        }

        [WebMethod]
        public CandidatoDTO GetCandidate(int userid)
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            return GetInfoCandidate(db, userid);

        }

        
    }
}
