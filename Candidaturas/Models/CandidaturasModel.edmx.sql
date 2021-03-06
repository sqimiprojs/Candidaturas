
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/22/2019 16:10:44
-- Generated from EDMX file: C:\Users\fabio\Documents\GitHub\Candidaturas\Candidaturas\Models\CandidaturasModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CandidaturaDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Concelho_Distrito]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Concelho] DROP CONSTRAINT [FK_Concelho_Distrito];
GO
IF OBJECT_ID(N'[dbo].[FK_CursoExame_Curso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CursoExame] DROP CONSTRAINT [FK_CursoExame_Curso];
GO
IF OBJECT_ID(N'[dbo].[FK_CursoExame_Exame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CursoExame] DROP CONSTRAINT [FK_CursoExame_Exame];
GO
IF OBJECT_ID(N'[dbo].[FK_DadosPessoais_Categoria]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DadosPessoais] DROP CONSTRAINT [FK_DadosPessoais_Categoria];
GO
IF OBJECT_ID(N'[dbo].[FK_DadosPessoais_EstadoCivil]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DadosPessoais] DROP CONSTRAINT [FK_DadosPessoais_EstadoCivil];
GO
IF OBJECT_ID(N'[dbo].[FK_DadosPessoais_Freguesia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DadosPessoais] DROP CONSTRAINT [FK_DadosPessoais_Freguesia];
GO
IF OBJECT_ID(N'[dbo].[FK_DadosPessoais_FreguesiaMorada]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DadosPessoais] DROP CONSTRAINT [FK_DadosPessoais_FreguesiaMorada];
GO
IF OBJECT_ID(N'[dbo].[FK_DadosPessoais_Genero]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DadosPessoais] DROP CONSTRAINT [FK_DadosPessoais_Genero];
GO
IF OBJECT_ID(N'[dbo].[FK_DadosPessoais_Pais]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DadosPessoais] DROP CONSTRAINT [FK_DadosPessoais_Pais];
GO
IF OBJECT_ID(N'[dbo].[FK_DadosPessoais_Posto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DadosPessoais] DROP CONSTRAINT [FK_DadosPessoais_Posto];
GO
IF OBJECT_ID(N'[dbo].[FK_DadosPessoais_Ramo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DadosPessoais] DROP CONSTRAINT [FK_DadosPessoais_Ramo];
GO
IF OBJECT_ID(N'[dbo].[FK_DadosPessoais_TipoDocumentoID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DadosPessoais] DROP CONSTRAINT [FK_DadosPessoais_TipoDocumentoID];
GO
IF OBJECT_ID(N'[dbo].[FK_DadosPessoais_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DadosPessoais] DROP CONSTRAINT [FK_DadosPessoais_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Documento_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documento] DROP CONSTRAINT [FK_Documento_User];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentoBinario_Documento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentoBinario] DROP CONSTRAINT [FK_DocumentoBinario_Documento];
GO
IF OBJECT_ID(N'[dbo].[FK_ExameUE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserExame] DROP CONSTRAINT [FK_ExameUE];
GO
IF OBJECT_ID(N'[dbo].[FK_Form_DadosPessoais]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Form] DROP CONSTRAINT [FK_Form_DadosPessoais];
GO
IF OBJECT_ID(N'[dbo].[FK_Freguesia_Distrito_Concelho]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Freguesia] DROP CONSTRAINT [FK_Freguesia_Distrito_Concelho];
GO
IF OBJECT_ID(N'[dbo].[FK_Inquerito_ConhecimentoEscola]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inquerito] DROP CONSTRAINT [FK_Inquerito_ConhecimentoEscola];
GO
IF OBJECT_ID(N'[dbo].[FK_Inquerito_SituacaoMae]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inquerito] DROP CONSTRAINT [FK_Inquerito_SituacaoMae];
GO
IF OBJECT_ID(N'[dbo].[FK_Inquerito_SituacaoPai]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inquerito] DROP CONSTRAINT [FK_Inquerito_SituacaoPai];
GO
IF OBJECT_ID(N'[dbo].[FK_Inquerito_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inquerito] DROP CONSTRAINT [FK_Inquerito_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Localidade_Distrito_Concelho]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Localidade] DROP CONSTRAINT [FK_Localidade_Distrito_Concelho];
GO
IF OBJECT_ID(N'[dbo].[FK_Posto_Categoria]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posto] DROP CONSTRAINT [FK_Posto_Categoria];
GO
IF OBJECT_ID(N'[dbo].[FK_Posto_Ramo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posto] DROP CONSTRAINT [FK_Posto_Ramo];
GO
IF OBJECT_ID(N'[dbo].[FK_UserCurso_Curso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserCurso] DROP CONSTRAINT [FK_UserCurso_Curso];
GO
IF OBJECT_ID(N'[dbo].[FK_UserCurso_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserCurso] DROP CONSTRAINT [FK_UserCurso_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserExame_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserExame] DROP CONSTRAINT [FK_UserExame_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categoria]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categoria];
GO
IF OBJECT_ID(N'[dbo].[Concelho]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Concelho];
GO
IF OBJECT_ID(N'[dbo].[ConhecimentoEscola]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConhecimentoEscola];
GO
IF OBJECT_ID(N'[dbo].[Curso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Curso];
GO
IF OBJECT_ID(N'[dbo].[CursoExame]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CursoExame];
GO
IF OBJECT_ID(N'[dbo].[DadosPessoais]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DadosPessoais];
GO
IF OBJECT_ID(N'[dbo].[Distrito]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Distrito];
GO
IF OBJECT_ID(N'[dbo].[Documento]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Documento];
GO
IF OBJECT_ID(N'[dbo].[DocumentoBinario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentoBinario];
GO
IF OBJECT_ID(N'[dbo].[EstadoCivil]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EstadoCivil];
GO
IF OBJECT_ID(N'[dbo].[Exame]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Exame];
GO
IF OBJECT_ID(N'[dbo].[Form]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Form];
GO
IF OBJECT_ID(N'[dbo].[Freguesia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Freguesia];
GO
IF OBJECT_ID(N'[dbo].[Genero]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Genero];
GO
IF OBJECT_ID(N'[dbo].[Inquerito]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Inquerito];
GO
IF OBJECT_ID(N'[dbo].[Localidade]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Localidade];
GO
IF OBJECT_ID(N'[dbo].[Pais]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pais];
GO
IF OBJECT_ID(N'[dbo].[Posto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posto];
GO
IF OBJECT_ID(N'[dbo].[Ramo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ramo];
GO
IF OBJECT_ID(N'[dbo].[Situacao]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Situacao];
GO
IF OBJECT_ID(N'[dbo].[TipoDocumentoID]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TipoDocumentoID];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[UserCurso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserCurso];
GO
IF OBJECT_ID(N'[dbo].[UserExame]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserExame];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categorias'
CREATE TABLE [dbo].[Categorias] (
    [Sigla] nvarchar(10)  NOT NULL,
    [Nome] nvarchar(50)  NOT NULL,
    [Ordem] smallint  NOT NULL
);
GO

-- Creating table 'Concelhoes'
CREATE TABLE [dbo].[Concelhoes] (
    [Nome] varchar(50)  NOT NULL,
    [Codigo] int  NOT NULL,
    [CodigoDistrito] int  NOT NULL
);
GO

-- Creating table 'ConhecimentoEscolas'
CREATE TABLE [dbo].[ConhecimentoEscolas] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nome] varchar(50)  NOT NULL
);
GO

-- Creating table 'Cursoes'
CREATE TABLE [dbo].[Cursoes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nome] varchar(100)  NOT NULL
);
GO

-- Creating table 'DadosPessoais'
CREATE TABLE [dbo].[DadosPessoais] (
    [UserId] int  NOT NULL,
    [NomeColoquial] varchar(50)  NOT NULL,
    [Nomes] varchar(50)  NULL,
    [Apelidos] varchar(50)  NULL,
    [NomePai] varchar(50)  NULL,
    [NomeMae] varchar(50)  NULL,
    [NDI] varchar(50)  NOT NULL,
    [TipoDocID] int  NOT NULL,
    [Genero] int  NULL,
    [EstadoCivil] int  NULL,
    [Nacionalidade] varchar(10)  NULL,
    [DistritoNatural] int  NULL,
    [ConcelhoNatural] int  NULL,
    [FreguesiaNatural] int  NULL,
    [Morada] varchar(50)  NULL,
    [Localidade] int  NULL,
    [RepFinNIF] varchar(50)  NULL,
    [CCDigitosControlo] varchar(50)  NULL,
    [NSegSoc] varchar(50)  NULL,
    [NIF] varchar(50)  NULL,
    [DistritoMorada] int  NULL,
    [ConcelhoMorada] int  NULL,
    [FreguesiaMorada] int  NULL,
    [Telefone] varchar(50)  NULL,
    [CodigoPostal4Dig] smallint  NULL,
    [CodigoPostal3Dig] smallint  NULL,
    [DataCriacao] datetime  NOT NULL,
    [DataUltimaAtualizacao] datetime  NOT NULL,
    [DataNascimento] datetime  NOT NULL,
    [Militar] bit  NOT NULL,
    [Ramo] nvarchar(10)  NULL,
    [Categoria] nvarchar(10)  NULL,
    [Posto] int  NULL,
    [Classe] nvarchar(50)  NULL,
    [NIM] varchar(50)  NULL
);
GO

-- Creating table 'Distritoes'
CREATE TABLE [dbo].[Distritoes] (
    [Nome] varchar(50)  NOT NULL,
    [Codigo] int  NOT NULL
);
GO

-- Creating table 'Documentoes'
CREATE TABLE [dbo].[Documentoes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NOT NULL,
    [Nome] varchar(50)  NULL,
    [Descricao] varchar(50)  NULL,
    [Tipo] varchar(50)  NULL,
    [UploadTime] datetime  NOT NULL
);
GO

-- Creating table 'DocumentoBinarios'
CREATE TABLE [dbo].[DocumentoBinarios] (
    [DocID] int  NOT NULL,
    [DocBinario] varbinary(max)  NULL
);
GO

-- Creating table 'EstadoCivils'
CREATE TABLE [dbo].[EstadoCivils] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nome] varchar(50)  NOT NULL
);
GO

-- Creating table 'Exames'
CREATE TABLE [dbo].[Exames] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nome] varchar(50)  NULL,
    [Código] int  NOT NULL
);
GO

-- Creating table 'Forms'
CREATE TABLE [dbo].[Forms] (
    [UserID] int  NOT NULL,
    [FormBin] varbinary(max)  NULL,
    [DataCriação] datetime  NULL
);
GO

-- Creating table 'Freguesias'
CREATE TABLE [dbo].[Freguesias] (
    [Nome] varchar(100)  NOT NULL,
    [Codigo] int  NOT NULL,
    [CodigoConcelho] int  NOT NULL,
    [CodigoDistrito] int  NOT NULL
);
GO

-- Creating table 'Generoes'
CREATE TABLE [dbo].[Generoes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nome] varchar(50)  NOT NULL
);
GO

-- Creating table 'Inqueritoes'
CREATE TABLE [dbo].[Inqueritoes] (
    [UserId] int  NOT NULL,
    [SituacaoPai] int  NOT NULL,
    [OutraPai] varchar(50)  NULL,
    [SituacaoMae] int  NOT NULL,
    [OutraMae] varchar(50)  NULL,
    [ConhecimentoEscola] int  NOT NULL,
    [Outro] varchar(50)  NULL,
    [CandidatarOutros] bit  NOT NULL,
    [DataCriacao] datetime  NULL,
    [DataAtualizacao] datetime  NULL
);
GO

-- Creating table 'Localidades'
CREATE TABLE [dbo].[Localidades] (
    [CodigoDistrito] int  NOT NULL,
    [CodigoConcelho] int  NOT NULL,
    [Codigo] int  NOT NULL,
    [Nome] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Pais'
CREATE TABLE [dbo].[Pais] (
    [Sigla] varchar(10)  NOT NULL,
    [Nome] varchar(50)  NOT NULL
);
GO

-- Creating table 'Postoes'
CREATE TABLE [dbo].[Postoes] (
    [Código] int  NOT NULL,
    [Nome] nvarchar(50)  NOT NULL,
    [Abreviatura] nvarchar(10)  NOT NULL,
    [RamoMilitar] nvarchar(10)  NOT NULL,
    [CategoriaMilitar] nvarchar(10)  NOT NULL,
    [Ordem] int  NOT NULL
);
GO

-- Creating table 'Ramoes'
CREATE TABLE [dbo].[Ramoes] (
    [Sigla] nvarchar(10)  NOT NULL,
    [Nome] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Situacaos'
CREATE TABLE [dbo].[Situacaos] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nome] varchar(50)  NOT NULL
);
GO

-- Creating table 'TipoDocumentoIDs'
CREATE TABLE [dbo].[TipoDocumentoIDs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Nome] varchar(50)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Password] varbinary(64)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [LoginErrorMessage] varchar(50)  NULL,
    [DataCriacao] datetime  NOT NULL
);
GO

-- Creating table 'UserCursoes'
CREATE TABLE [dbo].[UserCursoes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [CursoId] int  NOT NULL,
    [Prioridade] int  NOT NULL
);
GO

-- Creating table 'UserExames'
CREATE TABLE [dbo].[UserExames] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [ExameId] int  NOT NULL
);
GO

-- Creating table 'CursoExame'
CREATE TABLE [dbo].[CursoExame] (
    [Cursoes_ID] int  NOT NULL,
    [Exames_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Sigla] in table 'Categorias'
ALTER TABLE [dbo].[Categorias]
ADD CONSTRAINT [PK_Categorias]
    PRIMARY KEY CLUSTERED ([Sigla] ASC);
GO

-- Creating primary key on [Codigo], [CodigoDistrito] in table 'Concelhoes'
ALTER TABLE [dbo].[Concelhoes]
ADD CONSTRAINT [PK_Concelhoes]
    PRIMARY KEY CLUSTERED ([Codigo], [CodigoDistrito] ASC);
GO

-- Creating primary key on [ID] in table 'ConhecimentoEscolas'
ALTER TABLE [dbo].[ConhecimentoEscolas]
ADD CONSTRAINT [PK_ConhecimentoEscolas]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Cursoes'
ALTER TABLE [dbo].[Cursoes]
ADD CONSTRAINT [PK_Cursoes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [UserId] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [PK_DadosPessoais]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [Codigo] in table 'Distritoes'
ALTER TABLE [dbo].[Distritoes]
ADD CONSTRAINT [PK_Distritoes]
    PRIMARY KEY CLUSTERED ([Codigo] ASC);
GO

-- Creating primary key on [ID] in table 'Documentoes'
ALTER TABLE [dbo].[Documentoes]
ADD CONSTRAINT [PK_Documentoes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [DocID] in table 'DocumentoBinarios'
ALTER TABLE [dbo].[DocumentoBinarios]
ADD CONSTRAINT [PK_DocumentoBinarios]
    PRIMARY KEY CLUSTERED ([DocID] ASC);
GO

-- Creating primary key on [ID] in table 'EstadoCivils'
ALTER TABLE [dbo].[EstadoCivils]
ADD CONSTRAINT [PK_EstadoCivils]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Exames'
ALTER TABLE [dbo].[Exames]
ADD CONSTRAINT [PK_Exames]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [UserID] in table 'Forms'
ALTER TABLE [dbo].[Forms]
ADD CONSTRAINT [PK_Forms]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [Codigo], [CodigoConcelho], [CodigoDistrito] in table 'Freguesias'
ALTER TABLE [dbo].[Freguesias]
ADD CONSTRAINT [PK_Freguesias]
    PRIMARY KEY CLUSTERED ([Codigo], [CodigoConcelho], [CodigoDistrito] ASC);
GO

-- Creating primary key on [ID] in table 'Generoes'
ALTER TABLE [dbo].[Generoes]
ADD CONSTRAINT [PK_Generoes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [UserId] in table 'Inqueritoes'
ALTER TABLE [dbo].[Inqueritoes]
ADD CONSTRAINT [PK_Inqueritoes]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [CodigoDistrito], [CodigoConcelho], [Codigo] in table 'Localidades'
ALTER TABLE [dbo].[Localidades]
ADD CONSTRAINT [PK_Localidades]
    PRIMARY KEY CLUSTERED ([CodigoDistrito], [CodigoConcelho], [Codigo] ASC);
GO

-- Creating primary key on [Sigla] in table 'Pais'
ALTER TABLE [dbo].[Pais]
ADD CONSTRAINT [PK_Pais]
    PRIMARY KEY CLUSTERED ([Sigla] ASC);
GO

-- Creating primary key on [Código] in table 'Postoes'
ALTER TABLE [dbo].[Postoes]
ADD CONSTRAINT [PK_Postoes]
    PRIMARY KEY CLUSTERED ([Código] ASC);
GO

-- Creating primary key on [Sigla] in table 'Ramoes'
ALTER TABLE [dbo].[Ramoes]
ADD CONSTRAINT [PK_Ramoes]
    PRIMARY KEY CLUSTERED ([Sigla] ASC);
GO

-- Creating primary key on [ID] in table 'Situacaos'
ALTER TABLE [dbo].[Situacaos]
ADD CONSTRAINT [PK_Situacaos]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TipoDocumentoIDs'
ALTER TABLE [dbo].[TipoDocumentoIDs]
ADD CONSTRAINT [PK_TipoDocumentoIDs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserCursoes'
ALTER TABLE [dbo].[UserCursoes]
ADD CONSTRAINT [PK_UserCursoes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserExames'
ALTER TABLE [dbo].[UserExames]
ADD CONSTRAINT [PK_UserExames]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Cursoes_ID], [Exames_ID] in table 'CursoExame'
ALTER TABLE [dbo].[CursoExame]
ADD CONSTRAINT [PK_CursoExame]
    PRIMARY KEY CLUSTERED ([Cursoes_ID], [Exames_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Categoria] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [FK_DadosPessoais_Categoria]
    FOREIGN KEY ([Categoria])
    REFERENCES [dbo].[Categorias]
        ([Sigla])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DadosPessoais_Categoria'
CREATE INDEX [IX_FK_DadosPessoais_Categoria]
ON [dbo].[DadosPessoais]
    ([Categoria]);
GO

-- Creating foreign key on [CategoriaMilitar] in table 'Postoes'
ALTER TABLE [dbo].[Postoes]
ADD CONSTRAINT [FK_Posto_Categoria]
    FOREIGN KEY ([CategoriaMilitar])
    REFERENCES [dbo].[Categorias]
        ([Sigla])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Posto_Categoria'
CREATE INDEX [IX_FK_Posto_Categoria]
ON [dbo].[Postoes]
    ([CategoriaMilitar]);
GO

-- Creating foreign key on [CodigoDistrito] in table 'Concelhoes'
ALTER TABLE [dbo].[Concelhoes]
ADD CONSTRAINT [FK_Concelho_Distrito]
    FOREIGN KEY ([CodigoDistrito])
    REFERENCES [dbo].[Distritoes]
        ([Codigo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Concelho_Distrito'
CREATE INDEX [IX_FK_Concelho_Distrito]
ON [dbo].[Concelhoes]
    ([CodigoDistrito]);
GO

-- Creating foreign key on [CodigoConcelho], [CodigoDistrito] in table 'Freguesias'
ALTER TABLE [dbo].[Freguesias]
ADD CONSTRAINT [FK_Freguesia_Distrito_Concelho]
    FOREIGN KEY ([CodigoConcelho], [CodigoDistrito])
    REFERENCES [dbo].[Concelhoes]
        ([Codigo], [CodigoDistrito])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Freguesia_Distrito_Concelho'
CREATE INDEX [IX_FK_Freguesia_Distrito_Concelho]
ON [dbo].[Freguesias]
    ([CodigoConcelho], [CodigoDistrito]);
GO

-- Creating foreign key on [CodigoConcelho], [CodigoDistrito] in table 'Localidades'
ALTER TABLE [dbo].[Localidades]
ADD CONSTRAINT [FK_Localidade_Distrito_Concelho]
    FOREIGN KEY ([CodigoConcelho], [CodigoDistrito])
    REFERENCES [dbo].[Concelhoes]
        ([Codigo], [CodigoDistrito])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ConhecimentoEscola] in table 'Inqueritoes'
ALTER TABLE [dbo].[Inqueritoes]
ADD CONSTRAINT [FK_Inquerito_ConhecimentoEscola]
    FOREIGN KEY ([ConhecimentoEscola])
    REFERENCES [dbo].[ConhecimentoEscolas]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inquerito_ConhecimentoEscola'
CREATE INDEX [IX_FK_Inquerito_ConhecimentoEscola]
ON [dbo].[Inqueritoes]
    ([ConhecimentoEscola]);
GO

-- Creating foreign key on [CursoId] in table 'UserCursoes'
ALTER TABLE [dbo].[UserCursoes]
ADD CONSTRAINT [FK_UserCurso_Curso]
    FOREIGN KEY ([CursoId])
    REFERENCES [dbo].[Cursoes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserCurso_Curso'
CREATE INDEX [IX_FK_UserCurso_Curso]
ON [dbo].[UserCursoes]
    ([CursoId]);
GO

-- Creating foreign key on [EstadoCivil] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [FK_DadosPessoais_EstadoCivil]
    FOREIGN KEY ([EstadoCivil])
    REFERENCES [dbo].[EstadoCivils]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DadosPessoais_EstadoCivil'
CREATE INDEX [IX_FK_DadosPessoais_EstadoCivil]
ON [dbo].[DadosPessoais]
    ([EstadoCivil]);
GO

-- Creating foreign key on [FreguesiaNatural], [ConcelhoNatural], [DistritoNatural] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [FK_DadosPessoais_Freguesia]
    FOREIGN KEY ([FreguesiaNatural], [ConcelhoNatural], [DistritoNatural])
    REFERENCES [dbo].[Freguesias]
        ([Codigo], [CodigoConcelho], [CodigoDistrito])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DadosPessoais_Freguesia'
CREATE INDEX [IX_FK_DadosPessoais_Freguesia]
ON [dbo].[DadosPessoais]
    ([FreguesiaNatural], [ConcelhoNatural], [DistritoNatural]);
GO

-- Creating foreign key on [FreguesiaMorada], [ConcelhoMorada], [DistritoMorada] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [FK_DadosPessoais_FreguesiaMorada]
    FOREIGN KEY ([FreguesiaMorada], [ConcelhoMorada], [DistritoMorada])
    REFERENCES [dbo].[Freguesias]
        ([Codigo], [CodigoConcelho], [CodigoDistrito])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DadosPessoais_FreguesiaMorada'
CREATE INDEX [IX_FK_DadosPessoais_FreguesiaMorada]
ON [dbo].[DadosPessoais]
    ([FreguesiaMorada], [ConcelhoMorada], [DistritoMorada]);
GO

-- Creating foreign key on [Genero] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [FK_DadosPessoais_Genero]
    FOREIGN KEY ([Genero])
    REFERENCES [dbo].[Generoes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DadosPessoais_Genero'
CREATE INDEX [IX_FK_DadosPessoais_Genero]
ON [dbo].[DadosPessoais]
    ([Genero]);
GO

-- Creating foreign key on [Nacionalidade] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [FK_DadosPessoais_Pais]
    FOREIGN KEY ([Nacionalidade])
    REFERENCES [dbo].[Pais]
        ([Sigla])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DadosPessoais_Pais'
CREATE INDEX [IX_FK_DadosPessoais_Pais]
ON [dbo].[DadosPessoais]
    ([Nacionalidade]);
GO

-- Creating foreign key on [Posto] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [FK_DadosPessoais_Posto]
    FOREIGN KEY ([Posto])
    REFERENCES [dbo].[Postoes]
        ([Código])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DadosPessoais_Posto'
CREATE INDEX [IX_FK_DadosPessoais_Posto]
ON [dbo].[DadosPessoais]
    ([Posto]);
GO

-- Creating foreign key on [Ramo] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [FK_DadosPessoais_Ramo]
    FOREIGN KEY ([Ramo])
    REFERENCES [dbo].[Ramoes]
        ([Sigla])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DadosPessoais_Ramo'
CREATE INDEX [IX_FK_DadosPessoais_Ramo]
ON [dbo].[DadosPessoais]
    ([Ramo]);
GO

-- Creating foreign key on [TipoDocID] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [FK_DadosPessoais_TipoDocumentoID]
    FOREIGN KEY ([TipoDocID])
    REFERENCES [dbo].[TipoDocumentoIDs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DadosPessoais_TipoDocumentoID'
CREATE INDEX [IX_FK_DadosPessoais_TipoDocumentoID]
ON [dbo].[DadosPessoais]
    ([TipoDocID]);
GO

-- Creating foreign key on [UserId] in table 'DadosPessoais'
ALTER TABLE [dbo].[DadosPessoais]
ADD CONSTRAINT [FK_DadosPessoais_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserID] in table 'Forms'
ALTER TABLE [dbo].[Forms]
ADD CONSTRAINT [FK_Form_DadosPessoais]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[DadosPessoais]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserID] in table 'Documentoes'
ALTER TABLE [dbo].[Documentoes]
ADD CONSTRAINT [FK_Documento_User]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Documento_User'
CREATE INDEX [IX_FK_Documento_User]
ON [dbo].[Documentoes]
    ([UserID]);
GO

-- Creating foreign key on [DocID] in table 'DocumentoBinarios'
ALTER TABLE [dbo].[DocumentoBinarios]
ADD CONSTRAINT [FK_DocumentoBinario_Documento]
    FOREIGN KEY ([DocID])
    REFERENCES [dbo].[Documentoes]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ExameId] in table 'UserExames'
ALTER TABLE [dbo].[UserExames]
ADD CONSTRAINT [FK_ExameUE]
    FOREIGN KEY ([ExameId])
    REFERENCES [dbo].[Exames]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExameUE'
CREATE INDEX [IX_FK_ExameUE]
ON [dbo].[UserExames]
    ([ExameId]);
GO

-- Creating foreign key on [SituacaoMae] in table 'Inqueritoes'
ALTER TABLE [dbo].[Inqueritoes]
ADD CONSTRAINT [FK_Inquerito_SituacaoMae]
    FOREIGN KEY ([SituacaoMae])
    REFERENCES [dbo].[Situacaos]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inquerito_SituacaoMae'
CREATE INDEX [IX_FK_Inquerito_SituacaoMae]
ON [dbo].[Inqueritoes]
    ([SituacaoMae]);
GO

-- Creating foreign key on [SituacaoPai] in table 'Inqueritoes'
ALTER TABLE [dbo].[Inqueritoes]
ADD CONSTRAINT [FK_Inquerito_SituacaoPai]
    FOREIGN KEY ([SituacaoPai])
    REFERENCES [dbo].[Situacaos]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inquerito_SituacaoPai'
CREATE INDEX [IX_FK_Inquerito_SituacaoPai]
ON [dbo].[Inqueritoes]
    ([SituacaoPai]);
GO

-- Creating foreign key on [UserId] in table 'Inqueritoes'
ALTER TABLE [dbo].[Inqueritoes]
ADD CONSTRAINT [FK_Inquerito_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RamoMilitar] in table 'Postoes'
ALTER TABLE [dbo].[Postoes]
ADD CONSTRAINT [FK_Posto_Ramo]
    FOREIGN KEY ([RamoMilitar])
    REFERENCES [dbo].[Ramoes]
        ([Sigla])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Posto_Ramo'
CREATE INDEX [IX_FK_Posto_Ramo]
ON [dbo].[Postoes]
    ([RamoMilitar]);
GO

-- Creating foreign key on [UserId] in table 'UserCursoes'
ALTER TABLE [dbo].[UserCursoes]
ADD CONSTRAINT [FK_UserCurso_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserCurso_User'
CREATE INDEX [IX_FK_UserCurso_User]
ON [dbo].[UserCursoes]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'UserExames'
ALTER TABLE [dbo].[UserExames]
ADD CONSTRAINT [FK_UserExame_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserExame_User'
CREATE INDEX [IX_FK_UserExame_User]
ON [dbo].[UserExames]
    ([UserId]);
GO

-- Creating foreign key on [Cursoes_ID] in table 'CursoExame'
ALTER TABLE [dbo].[CursoExame]
ADD CONSTRAINT [FK_CursoExame_Curso]
    FOREIGN KEY ([Cursoes_ID])
    REFERENCES [dbo].[Cursoes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Exames_ID] in table 'CursoExame'
ALTER TABLE [dbo].[CursoExame]
ADD CONSTRAINT [FK_CursoExame_Exame]
    FOREIGN KEY ([Exames_ID])
    REFERENCES [dbo].[Exames]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CursoExame_Exame'
CREATE INDEX [IX_FK_CursoExame_Exame]
ON [dbo].[CursoExame]
    ([Exames_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------