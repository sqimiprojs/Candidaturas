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
    
    public partial class Localidade
    {
        public int CodigoDistrito { get; set; }
        public int CodigoConcelho { get; set; }
        public int Codigo { get; set; }
        public string Nome { get; set; }
    
        public virtual Concelho Concelho { get; set; }
    }
}
