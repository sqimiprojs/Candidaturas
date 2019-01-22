//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Candidaturas.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DadosPessoai
    {
        public int UserId { get; set; }
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
        public Nullable<System.DateTime> DocumentoValidade { get; set; }
    
        public virtual Categoria Categoria1 { get; set; }
        public virtual EstadoCivil EstadoCivil1 { get; set; }
        public virtual Freguesia Freguesia { get; set; }
        public virtual Freguesia Freguesia1 { get; set; }
        public virtual Genero Genero1 { get; set; }
        public virtual Pai Pai { get; set; }
        public virtual Posto Posto1 { get; set; }
        public virtual Ramo Ramo1 { get; set; }
        public virtual TipoDocumentoID TipoDocumentoID { get; set; }
        public virtual User User { get; set; }
        public virtual Form Form { get; set; }
    }
}
