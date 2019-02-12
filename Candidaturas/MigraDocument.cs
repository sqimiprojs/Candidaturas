using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using MigraDoc.DocumentObjectModel.Shapes;
using Candidaturas.Models;

namespace Candidaturas
{
    public class MigraDocument
    {
        public static Document CreateDocument(string nome, string descricao, string autor, int tipo, int session)
        {
            // Create a new MigraDoc document
            Document PDFdocument = new Document();
            PDFdocument.Info.Title = nome;
            PDFdocument.Info.Subject = descricao;
            PDFdocument.Info.Author = autor;


            DefineStyles(PDFdocument);
            CreateHeaderFooter(PDFdocument);
            switch (tipo)
            {
                case 1:
                    CreateComprovativo(PDFdocument, session);
                    break;
                default:
                    break;
            }
           

            return PDFdocument;
        }

        /// <summary>
        /// Defines the styles used in the document.
        /// </summary>
        private static void DefineStyles(Document document)
        {
            // Get the predefined style Normal.
            Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Times New Roman";

            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            style = document.Styles["Heading1"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 16;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.Font.Color = Colors.Black;

            style = document.Styles["Heading2"];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.Font.Color = Colors.Black;

            style = document.Styles["Heading3"];
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.Font.Color = Colors.Black;

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Right;

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.Font.Color = Colors.DarkGray;


            // Create a new style called LongText based on style Normal
            style = document.Styles.AddStyle("LongText", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Font.Color = Colors.Black;

            // Create a new style called Centered Text based on style Normal
            style = document.Styles.AddStyle("CenterText", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.Font.Color = Colors.Black;

            // Create a new style called TextBox based on style Normal
            style = document.Styles.AddStyle("TextBox", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Borders.Width = 2;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Shading.Color = Colors.SkyBlue;

            // Create a new style called TOC based on style Normal
            style = document.Styles.AddStyle("TOC", "Normal");
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = Colors.Blue;
        }

        private static void CreateHeaderFooter(Document document)
        {
            Section page1 = document.AddSection();
            //page1.PageSetup.OddAndEvenPagesHeaderFooter = false;
            //page1.PageSetup.StartingNumber = 1;

            HeaderFooter head = page1.Headers.Primary;
            HeaderFooter foot = page1.Footers.Primary;


            string ImgPath = ((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath).Replace("bin/Candidaturas.DLL", "Content/img/logotipo.jpg");
            Image logo = head.AddImage(ImgPath);
            logo.LockAspectRatio = true;
            logo.Width = "2.5cm";
            logo.Top = ShapePosition.Top;
            logo.Left = ShapePosition.Left;
            logo.WrapFormat.Style = WrapStyle.Through;

            Paragraph hp = head.AddParagraph("\t Concurso de Admissão de Cadetes da Marinha - " + DateTime.Now.Year.ToString());
            head.Style = "Header";

            Paragraph fp = foot.AddParagraph("Escola Naval - " + DateTime.Now.Year.ToString());
            foot.Style = "Footer";         

            // Add paragraph to footer for odd pages.
            // Add clone of paragraph to footer for odd pages. Cloning is necessary because an object must
            // not belong to more than one other object. If you forget cloning an exception is thrown.
            page1.Footers.EvenPage.Add(fp.Clone());
        }

        private static void CreateComprovativo(Document document, int session)
        {
            Section page1 = document.LastSection;
            Paragraph title = page1.AddParagraph("\n\nComprovativo de Candidatura", "Heading1");

            CandidatoFullText c = GetInfoCandidato(session);
            Paragraph cod = page1.AddParagraph("\nCódigo de Candidato: "+c.CandidatoNumber,"Heading3");

            string cmil = c.Militar == true ? String.Format(" {0}, {1},", c.NIM, c.Posto) : "";

            string cfem = c.isFeminino == true ? "a" : "o";

            string cchild = ( c.NomePai!=null && c.NomeMae != null ) ? String.Format(" filh{0} de {1} e de {2},", cfem, c.NomePai, c.NomeMae) : "";

            string cnatural = (c.DistritoNatural != null && c.ConcelhoNatural != null ) ? String.Format("natural de {0}, {1},", c.DistritoNatural, c.ConcelhoNatural) : "";

            string cNIF = c.NIF!= null ? String.Format("número de Contribuinte {0},", c.NIF) : "";

            string ccont = c.Telefone!= null ? String.Format(" e contacto {0},", c.Telefone) : "";

            string mensagem = String.Format("\n\nEu, abaixo assinado,{0} {1}, {2} {3} residente em {4}, {5}-{6} {7}, {8}, freguesia de {9}, distrito de {10}, nascid{19} em {11}, {12}, " +
                "nacional de {13} com o {14} número {15} válido até {16}, {17} {18} declaro por minha honra que nunca fui abatid{19} ao Corpo de Alunos da Academia Militar " +
                "ou Academia da Força Aérea por motivos disciplinares ou por incapacidade para o serviço militar e que nunca fui excluíd{19} dos cursos da Escola Naval.",
                cmil, c.NomeColoquial, cchild, cnatural, c.Morada, c.CodigoPostal4Dig, c.CodigoPostal3Dig, c.Localidade, c.ConcelhoMorada, c.FreguesiaMorada, c.DistritoMorada, c.DataNascimento, c.EstadoCivil,
                c.Nacionalidade, c.TipoDocumento, c.NDI, c.ValidadeDoc, c.NIF, ccont, cfem);

            Paragraph Text1 = page1.AddParagraph(mensagem, "LongText");

            string msgAdmissao = String.Format("\n\n\nDesejo ser admitid{0} aos cursos de:", cfem);
            Paragraph Text2 = page1.AddParagraph(msgAdmissao, "LongText");

            string listagem;
            List<CursoDisplay> cursos = GetInfoCursosCandidato(session);
            foreach (CursoDisplay curso in cursos) {
                listagem = String.Format("\n\t{0}. - {1}", curso.prioridade, curso.nome);
                page1.AddParagraph(listagem, "LongText");
            }

            Paragraph Text3 = page1.AddParagraph("\nMais declaro que tomei conhecimento das condições de admissão, das datas de realização das provas de verificação dos " +
                "pré-requisitos de natureza física e médica e da data limite de entrega do certificado de classificações para acesso ao Ensino Superior e que todas as " +
                "declarações prestadas são verdadeiras.", "LongText");

            Paragraph Text4 = page1.AddParagraph();
            Text4.AddFormattedText("\n\nData: ", TextFormat.Bold);
            Text4.AddText(DateTime.Now.Date.ToString("dd/MM/yyyy") +"\t\t");
            Text4.AddFormattedText("Assinatura: ", TextFormat.Bold);
            Text4.AddText("______________________________________");
            Text4.Style = "CenterText";
        }

        private static CandidatoFullText GetInfoCandidato(int userId)
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            DadosPessoai dadosPessoaisUser = db.DadosPessoais
                                            .Where(dp => dp.UserId == userId)
                                            .FirstOrDefault();

            CandidatoFullText alldata = new CandidatoFullText();
            alldata.NomeColoquial = dadosPessoaisUser.NomeColoquial;
            alldata.NomePai = dadosPessoaisUser.NomePai;
            alldata.NomeMae = dadosPessoaisUser.NomeMae;
            alldata.NDI = dadosPessoaisUser.NDI;
            alldata.TipoDocumento = db.TipoDocumentoIDs.Where(dp => dp.ID == dadosPessoaisUser.TipoDocID).OrderBy(dp => dp.Nome).Select(dp => dp.Nome).FirstOrDefault();
            alldata.Genero = db.Generoes.Where(dp => dadosPessoaisUser.Genero == dp.ID).Select(dp => dp.Nome).FirstOrDefault();
            alldata.EstadoCivil = db.EstadoCivils.Where(dp => dp.ID == dadosPessoaisUser.EstadoCivil).OrderBy(dp => dp.Nome).Select(dp => dp.Nome).FirstOrDefault();
            alldata.Nacionalidade = db.Pais.Where(dp => dp.Sigla == dadosPessoaisUser.Nacionalidade).OrderBy(dp => dp.Nome).Select(dp => dp.Nome).FirstOrDefault();
            alldata.DistritoNatural = db.Distritoes.Where(dp => dadosPessoaisUser.DistritoNatural == dp.Codigo).Select(dp => dp.Nome).FirstOrDefault();
            alldata.ConcelhoNatural = db.Concelhoes.Where(dp => dp.CodigoDistrito == dadosPessoaisUser.DistritoNatural && dp.Codigo == dadosPessoaisUser.ConcelhoNatural).Select(dp => dp.Nome).FirstOrDefault();
            alldata.FreguesiaNatural = db.Freguesias.Where(dp => dp.CodigoConcelho == dadosPessoaisUser.ConcelhoNatural && dp.CodigoDistrito == dadosPessoaisUser.DistritoNatural && dp.Codigo == dadosPessoaisUser.FreguesiaNatural).Select(dp => dp.Nome).FirstOrDefault();
            alldata.Morada = dadosPessoaisUser.Morada;
            alldata.Localidade = db.Localidades.Where(dp => dp.CodigoConcelho == dadosPessoaisUser.ConcelhoMorada && dp.CodigoDistrito == dadosPessoaisUser.DistritoMorada && dp.Codigo == dadosPessoaisUser.Localidade).Select(dp => dp.Nome).FirstOrDefault();
            alldata.RepFinNIF = db.Reparticoes.Where(dp => dp.CodigoConcelho == dadosPessoaisUser.ConcelhoMorada && dp.CodigoDistrito == dadosPessoaisUser.DistritoMorada && dp.CodigoFreguesia == dadosPessoaisUser.FreguesiaMorada && dp.Codigo == dadosPessoaisUser.RepFinNIF).Select(dp => dp.Nome).FirstOrDefault(); 
            alldata.CCDigitosControlo = dadosPessoaisUser.CCDigitosControlo;
            alldata.NSegSoc = dadosPessoaisUser.NSegSoc;
            alldata.NIF = dadosPessoaisUser.NIF;
            alldata.DistritoMorada = db.Distritoes.Where(dp => dp.Codigo == dadosPessoaisUser.DistritoMorada).Select(dp => dp.Nome).FirstOrDefault();
            alldata.ConcelhoMorada = db.Concelhoes.Where(dp => dp.CodigoDistrito == dadosPessoaisUser.DistritoMorada && dp.Codigo == dadosPessoaisUser.ConcelhoMorada).Select(dp => dp.Nome).FirstOrDefault();
            alldata.FreguesiaMorada = db.Freguesias.Where(dp => dp.CodigoConcelho == dadosPessoaisUser.ConcelhoMorada && dp.CodigoDistrito == dadosPessoaisUser.DistritoMorada && dp.Codigo == dadosPessoaisUser.FreguesiaMorada).OrderBy(dp => dp.Nome).Select(dp => dp.Nome).FirstOrDefault();
            alldata.Telefone = dadosPessoaisUser.Telefone;
            alldata.CodigoPostal4Dig = dadosPessoaisUser.CodigoPostal4Dig.ToString();
            alldata.CodigoPostal3Dig = dadosPessoaisUser.CodigoPostal3Dig.ToString();
            alldata.DataNascimento = dadosPessoaisUser.DataNascimento.ToString("dd/MM/yyyy");
            alldata.Militar = dadosPessoaisUser.Militar;
            alldata.Ramo = db.Ramoes.Where(dp => dp.Sigla == dadosPessoaisUser.Ramo).OrderBy(dp => dp.Sigla).Select(dp => dp.Nome).FirstOrDefault();
            alldata.Categoria = db.Categorias.Where(dp => dp.Sigla == dadosPessoaisUser.Categoria).OrderBy(dp => dp.Sigla).Select(dp => dp.Nome).FirstOrDefault();
            alldata.Posto = db.Postoes.Where(dp => dp.RamoMilitar == dadosPessoaisUser.Ramo && dp.CategoriaMilitar == dadosPessoaisUser.Categoria && dp.Código == dadosPessoaisUser.Posto).OrderBy(dp => dp.Código).Select(dp => dp.Nome).FirstOrDefault();
            alldata.Classe = dadosPessoaisUser.Classe;
            alldata.NIM = dadosPessoaisUser.NIM;
            alldata.ValidadeDoc = dadosPessoaisUser.DocumentoValidade.ToString("dd/MM/yyyy");
            //para ter feminino nos documentos
            alldata.isFeminino = db.Generoes.Where(dp => dadosPessoaisUser.Genero == dp.ID).Select(dp => dp.Nome).FirstOrDefault() == "Feminino";
            alldata.CandidatoNumber = db.Candidatoes.Where(dp => dp.UserID == userId).Select(dp => dp.Numero).FirstOrDefault().ToString();
            return alldata;
        }

        private static List<CursoDisplay> GetInfoCursosCandidato(int userId)
        {
            CandidaturaDBEntities1 db = new CandidaturaDBEntities1();
            List<UserCurso> ListaCursos = db.UserCursoes
                                            .Where(dp => dp.UserId == userId)
                                            .OrderBy(dp => dp.Prioridade)
                                            .ToList();

            List<CursoDisplay> listcd = new List<CursoDisplay>();
            foreach (UserCurso curso in ListaCursos)
            {

                CursoDisplay cd = new CursoDisplay();
                cd.nome = db.Cursoes.Where(dp => dp.ID == curso.CursoId).Select(dp => dp.Nome).FirstOrDefault();
                cd.prioridade = curso.Prioridade;
                listcd.Add(cd);
                
            }
            return listcd;
        }
    }

         
}