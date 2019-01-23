using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Candidaturas.Models
{

    [Serializable]
    public class CandidatoDTO
    {
        public DadosPessoaisDTO dadosDTO;
        public InqueritoDTO inqueritoDTO;
        public List<UserCursoDTO> cursosDTO;
        public List<UserExameDTO> examesDTO;
    }
    [Serializable]
    public class DadosPessoaisDTO
    {
        public string NomeColoquial { get; set; }
        public string Nomes { get; set; }
        public string Apelidos { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; }
        public string NDI { get; set; }
        public int TipoDocID { get; set; }
        public Nullable<int> Genero { get; set; }
        public Nullable<int> EstadoCivil { get; set; }
        public string Nacionalidade { get; set; }
        public Nullable<int> DistritoNatural { get; set; }
        public Nullable<int> ConcelhoNatural { get; set; }
        public Nullable<int> FreguesiaNatural { get; set; }
        public string Morada { get; set; }
        public Nullable<int> Localidade { get; set; }
        public string RepFinNIF { get; set; }
        public string CCDigitosControlo { get; set; }
        public string NSegSoc { get; set; }
        public string NIF { get; set; }
        public Nullable<int> DistritoMorada { get; set; }
        public Nullable<int> ConcelhoMorada { get; set; }
        public Nullable<int> FreguesiaMorada { get; set; }
        public string Telefone { get; set; }
        public Nullable<short> CodigoPostal4Dig { get; set; }
        public Nullable<short> CodigoPostal3Dig { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public System.DateTime DataUltimaAtualizacao { get; set; }
        public System.DateTime DataNascimento { get; set; }
        public bool Militar { get; set; }
        public string Ramo { get; set; }
        public string Categoria { get; set; }
        public Nullable<int> Posto { get; set; }
        public string Classe { get; set; }
        public string NIM { get; set; }
    }
    [Serializable]
    public class InqueritoDTO
    {
        public int SituacaoPai { get; set; }
        public string OutraPai { get; set; }
        public int SituacaoMae { get; set; }
        public string OutraMae { get; set; }
        public int ConhecimentoEscola { get; set; }
        public string Outro { get; set; }
        public bool CandidatarOutros { get; set; }
    }
    [Serializable]
    public class UserCursoDTO
    {
        public int CursoId { get; set; }
        public int Prioridade { get; set; }
    }
    [Serializable]
    public class UserExameDTO
    {
        public int ExameId { get; set; }
    }
}