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
    
    public partial class Certificado
    {
        public int CandidaturaID { get; set; }
        public byte[] FormBin { get; set; }
        public System.DateTime DataCriação { get; set; }
        public System.DateTime DiaCriação { get; set; }
    
        public virtual Candidatura Candidatura { get; set; }
    }
}