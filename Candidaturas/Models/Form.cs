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
    
    public partial class Form
    {
        public int UserID { get; set; }
        public byte[] FormBin { get; set; }
        public System.DateTime DataCriação { get; set; }
    
        public virtual DadosPessoai DadosPessoai { get; set; }
    }
}
