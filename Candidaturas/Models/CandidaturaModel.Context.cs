﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CandidaturaDBEntities1 : DbContext
    {
        public CandidaturaDBEntities1()
            : base("name=CandidaturaDBEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Candidatura> Candidaturas { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Certificado> Certificadoes { get; set; }
        public virtual DbSet<Concelho> Concelhoes { get; set; }
        public virtual DbSet<ConhecimentoEscola> ConhecimentoEscolas { get; set; }
        public virtual DbSet<Curso> Cursoes { get; set; }
        public virtual DbSet<CursoExame> CursoExames { get; set; }
        public virtual DbSet<DadosPessoai> DadosPessoais { get; set; }
        public virtual DbSet<Distrito> Distritoes { get; set; }
        public virtual DbSet<Documento> Documentoes { get; set; }
        public virtual DbSet<DocumentoBinario> DocumentoBinarios { get; set; }
        public virtual DbSet<Edicao> Edicaos { get; set; }
        public virtual DbSet<EstadoCivil> EstadoCivils { get; set; }
        public virtual DbSet<Exame> Exames { get; set; }
        public virtual DbSet<Freguesia> Freguesias { get; set; }
        public virtual DbSet<Genero> Generoes { get; set; }
        public virtual DbSet<Historico> Historicoes { get; set; }
        public virtual DbSet<Inquerito> Inqueritoes { get; set; }
        public virtual DbSet<Localidade> Localidades { get; set; }
        public virtual DbSet<Opco> Opcoes { get; set; }
        public virtual DbSet<Pai> Pais { get; set; }
        public virtual DbSet<Posto> Postoes { get; set; }
        public virtual DbSet<Ramo> Ramoes { get; set; }
        public virtual DbSet<Repartico> Reparticoes { get; set; }
        public virtual DbSet<Situacao> Situacaos { get; set; }
        public virtual DbSet<TipoDocumentoID> TipoDocumentoIDs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserExame> UserExames { get; set; }
    }
}
