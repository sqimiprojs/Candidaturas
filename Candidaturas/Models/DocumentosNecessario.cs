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
    
    public partial class DocumentosNecessario
    {
        public int ID { get; set; }
        public string Documento { get; set; }
        public string Edicao { get; set; }
    
        public virtual Edicao Edicao1 { get; set; }
    }
}
