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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Inqueritoes = new HashSet<Inquerito>();
            this.UserCursoes = new HashSet<UserCurso>();
            this.UserDocumentoes = new HashSet<UserDocumento>();
            this.UserExames = new HashSet<UserExame>();
        }
    
        public int ID { get; set; }
        public string NomeCompleto { get; set; }
        public byte[] Password { get; set; }
        public string NDI { get; set; }
        public bool Militar { get; set; }
        public System.DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string LoginErrorMessage { get; set; }
        public string TipoDocID { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public string PasswordInput { get; set; }

        public virtual DadosPessoai DadosPessoai { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inquerito> Inqueritoes { get; set; }
        public virtual TipoDocumentoID TipoDocumentoID { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserCurso> UserCursoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserDocumento> UserDocumentoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserExame> UserExames { get; set; }
    }
}
