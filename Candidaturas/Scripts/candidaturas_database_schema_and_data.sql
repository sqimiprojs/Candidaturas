USE [master]
GO
/****** Object:  Database [LoginDataBase]    Script Date: 13/11/2018 15:02:06 ******/
CREATE DATABASE [LoginDataBase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LoginDataBase', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\LoginDataBase.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LoginDataBase_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\LoginDataBase_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [LoginDataBase] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LoginDataBase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LoginDataBase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LoginDataBase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LoginDataBase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LoginDataBase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LoginDataBase] SET ARITHABORT OFF 
GO
ALTER DATABASE [LoginDataBase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LoginDataBase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LoginDataBase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LoginDataBase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LoginDataBase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LoginDataBase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LoginDataBase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LoginDataBase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LoginDataBase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LoginDataBase] SET DISABLE_BROKER 
GO
ALTER DATABASE [LoginDataBase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LoginDataBase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LoginDataBase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LoginDataBase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LoginDataBase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LoginDataBase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LoginDataBase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LoginDataBase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LoginDataBase] SET  MULTI_USER 
GO
ALTER DATABASE [LoginDataBase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LoginDataBase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LoginDataBase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LoginDataBase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LoginDataBase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LoginDataBase] SET QUERY_STORE = OFF
GO
USE [LoginDataBase]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [LoginDataBase]
GO
/****** Object:  User [SQIMI\Candidaturas]    Script Date: 13/11/2018 15:02:07 ******/
CREATE USER [SQIMI\Candidaturas] FOR LOGIN [SQIMI\Candidaturas] WITH DEFAULT_SCHEMA=[SQIMI\Candidaturas]
GO
/****** Object:  User [Administrator]    Script Date: 13/11/2018 15:02:07 ******/
CREATE USER [Administrator] FOR LOGIN [Administrator] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [SQIMI\Candidaturas]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [SQIMI\Candidaturas]
GO
ALTER ROLE [db_owner] ADD MEMBER [Administrator]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [Administrator]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [Administrator]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [Administrator]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [Administrator]
GO
ALTER ROLE [db_datareader] ADD MEMBER [Administrator]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [Administrator]
GO
ALTER ROLE [db_denydatareader] ADD MEMBER [Administrator]
GO
ALTER ROLE [db_denydatawriter] ADD MEMBER [Administrator]
GO
/****** Object:  Schema [SQIMI\Candidaturas]    Script Date: 13/11/2018 15:02:07 ******/
CREATE SCHEMA [SQIMI\Candidaturas]
GO
/****** Object:  Table [dbo].[Concelho]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Concelho](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Codigo] [int] NOT NULL,
	[CodigoDistrito] [int] NOT NULL,
 CONSTRAINT [PK_Concelho] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC,
	[CodigoDistrito] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConhecimentoEscola]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConhecimentoEscola](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
 CONSTRAINT [PK_ConhecimentoEscola] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Curso]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Curso](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[CodigoCurso] [varchar](50) NOT NULL,
	[CodigoRamo] [varchar](50) NULL,
 CONSTRAINT [PK_Documento] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DadosPessoais]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DadosPessoais](
	[NomeColoquial] [varchar](50) NULL,
	[Nomes] [varchar](50) NULL,
	[Apelidos] [varchar](50) NULL,
	[NomePai] [varchar](50) NULL,
	[NomeMae] [varchar](50) NULL,
	[NDI] [varchar](50) NULL,
	[TipoDocID] [varchar](50) NULL,
	[Genero] [varchar](50) NULL,
	[EstadoCivil] [varchar](50) NULL,
	[Nacionalidade] [varchar](50) NULL,
	[DistritoNatural] [varchar](50) NULL,
	[ConcelhoNatural] [varchar](50) NULL,
	[FreguesiaNatural] [varchar](150) NULL,
	[Morada] [varchar](50) NULL,
	[Localidade] [varchar](50) NULL,
	[UserId] [int] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RepFinNIF] [varchar](50) NULL,
	[CCDigitosControlo] [varchar](50) NULL,
	[NSegSoc] [varchar](50) NULL,
	[NIF] [varchar](50) NULL,
	[DistritoMorada] [varchar](50) NULL,
	[ConcelhoMorada] [varchar](50) NULL,
	[FreguesiaMorada] [varchar](150) NULL,
	[Telefone] [varchar](50) NULL,
	[CodigoPostal4Dig] [nchar](10) NULL,
	[CodigoPostal3Dig] [nchar](10) NULL,
	[DataCriacao] [datetime] NULL,
	[DataUltimaAtualizacao] [datetime] NULL,
 CONSTRAINT [PK_DadosPessoais] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Distrito]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Distrito](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Codigo] [int] NOT NULL,
 CONSTRAINT [PK_Distrito] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documento]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documento](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
	[Descricao] [varchar](50) NULL,
	[Ficheiro] [varbinary](max) NULL,
	[Tipo] [varchar](50) NULL,
 CONSTRAINT [PK_Documento_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoCivil]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoCivil](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
 CONSTRAINT [PK_EstadoCivil] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exame]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exame](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
 CONSTRAINT [PK_Exame] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Freguesia]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Freguesia](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Codigo] [int] NOT NULL,
	[CodigoConcelho] [int] NOT NULL,
	[CodigoDistrito] [int] NOT NULL,
 CONSTRAINT [PK_Freguesia] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC,
	[CodigoConcelho] ASC,
	[CodigoDistrito] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genero]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genero](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
 CONSTRAINT [PK_Genero] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inquerito]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inquerito](
	[UserId] [int] NOT NULL,
	[SituacaoPai] [varchar](50) NOT NULL,
	[OutraPai] [varchar](50) NULL,
	[SituacaoMae] [varchar](50) NOT NULL,
	[OutraMae] [varchar](50) NULL,
	[ConhecimentoEscola] [varchar](50) NOT NULL,
	[Outro] [varchar](50) NULL,
	[CandidatarOutros] [bit] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DataCriacao] [datetime] NULL,
	[DataAtualizacao] [datetime] NULL,
 CONSTRAINT [PK_Inquerito] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Localidade]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Localidade](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
 CONSTRAINT [PK_Localidade] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pais]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pais](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
	[Sigla] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Pais_1] PRIMARY KEY CLUSTERED 
(
	[Sigla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Situacao]    Script Date: 13/11/2018 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Situacao](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
 CONSTRAINT [PK_Situacao] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoDocumentoID]    Script Date: 13/11/2018 15:02:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoDocumentoID](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
 CONSTRAINT [PK_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 13/11/2018 15:02:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NomeCompleto] [varchar](50) NULL,
	[Password] [varchar](100) NULL,
	[NDI] [varchar](50) NULL,
	[Militar] [bit] NOT NULL,
	[DataNascimento] [date] NULL,
	[Email] [varchar](50) NULL,
	[LoginErrorMessage] [varchar](50) NULL,
	[TipoDocID] [varchar](50) NULL,
	[DataCriacao] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCurso]    Script Date: 13/11/2018 15:02:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserCurso](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CursoId] [int] NOT NULL,
	[Prioridade] [int] NOT NULL,
 CONSTRAINT [PK_UserCurso] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserDocumento]    Script Date: 13/11/2018 15:02:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDocumento](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[DocumentoId] [int] NOT NULL,
 CONSTRAINT [PK_UserDocumento] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserExame]    Script Date: 13/11/2018 15:02:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserExame](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ExameId] [int] NOT NULL,
 CONSTRAINT [PK_UserExame] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Inquerito]  WITH CHECK ADD  CONSTRAINT [FK_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Inquerito] CHECK CONSTRAINT [FK_UserId]
GO
ALTER TABLE [dbo].[UserExame]  WITH CHECK ADD  CONSTRAINT [FK_ExameUE] FOREIGN KEY([ExameId])
REFERENCES [dbo].[Exame] ([ID])
GO
ALTER TABLE [dbo].[UserExame] CHECK CONSTRAINT [FK_ExameUE]
GO
ALTER TABLE [dbo].[UserExame]  WITH CHECK ADD  CONSTRAINT [FK_UserUE] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserExame] CHECK CONSTRAINT [FK_UserUE]
GO
USE [master]
GO
ALTER DATABASE [LoginDataBase] SET  READ_WRITE 
GO
