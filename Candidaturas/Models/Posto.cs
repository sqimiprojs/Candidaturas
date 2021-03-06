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
    
    public partial class Posto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Posto()
        {
            this.DadosPessoais = new HashSet<DadosPessoai>();
            this.Militars = new HashSet<Militar>();
        }
    
        public int Código { get; set; }
        public string Nome { get; set; }
        public string Abreviatura { get; set; }
        public string RamoMilitar { get; set; }
        public string CategoriaMilitar { get; set; }
        public int Ordem { get; set; }
    
        public virtual Categoria Categoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DadosPessoai> DadosPessoais { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Militar> Militars { get; set; }
        public virtual Ramo Ramo { get; set; }
    }
}
