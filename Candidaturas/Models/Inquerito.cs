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
    
    public partial class Inquerito
    {
        public int UserId { get; set; }
        public string SituacaoPai { get; set; }
        public string OutraPai { get; set; }
        public string SituacaoMae { get; set; }
        public string OutraMae { get; set; }
        public string ConhecimentoEscola { get; set; }
        public string Outro { get; set; }
        public bool CandidatarOutros { get; set; }
        public int ID { get; set; }
        public Nullable<System.DateTime> DataCriacao { get; set; }
        public Nullable<System.DateTime> DataAtualizacao { get; set; }
    
        public virtual ConhecimentoEscola ConhecimentoEscola1 { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual Situacao Situacao1 { get; set; }
        public virtual User User { get; set; }
    }
}
