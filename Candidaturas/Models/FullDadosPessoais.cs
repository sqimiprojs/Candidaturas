using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Candidaturas.Models
{
    public class FullDadosPessoais
    {
        public string NomeColoquial { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; }
        public string NDI { get; set; }
        public string TipoDocumento { get; set; }
        public string Genero { get; set; }
        public string EstadoCivil { get; set; }
        public string Nacionalidade { get; set; }
        public string DistritoNatural { get; set; }
        public string ConcelhoNatural { get; set; }
        public string FreguesiaNatural { get; set; }
        public string Morada { get; set; }
        public string Localidade { get; set; }
        public string RepFinNIF { get; set; }
        public string CCDigitosControlo { get; set; }
        public string NSegSoc { get; set; }
        public string NIF { get; set; }
        public string DistritoMorada { get; set; }
        public string ConcelhoMorada { get; set; }
        public string FreguesiaMorada { get; set; }
        public string Telefone { get; set; }
        public string CodigoPostal4Dig { get; set; }
        public string CodigoPostal3Dig { get; set; }
        public string DataNascimento { get; set; }
        public bool   Militar { get; set; }
        public string Ramo { get; set; }
        public string Categoria { get; set; }
        public string Posto { get; set; }
        public string Classe { get; set; }
        public string NIM { get; set; }
        public bool   isFeminino { get; set; }
    }
}