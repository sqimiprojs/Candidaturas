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
    
    public partial class Documento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Documento()
        {
            this.UserDocumentoes = new HashSet<UserDocumento>();
        }
    
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public byte[] Ficheiro { get; set; }
        public string Tipo { get; set; }
    
        public virtual DocumentoBinario DocumentoBinario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserDocumento> UserDocumentoes { get; set; }
    }
}
