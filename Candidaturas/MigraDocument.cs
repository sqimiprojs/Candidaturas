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
        public static Document CreateDocument(string nome, string descricao, string autor)
        {
            // Create a new MigraDoc document
            Document PDFdocument = new Document();
            PDFdocument.Info.Title = nome;
            PDFdocument.Info.Subject = descricao;
            PDFdocument.Info.Author = autor;


            DefineStyles(PDFdocument);
            CreateHeaderFooter(PDFdocument);
            CreateComprovativo(PDFdocument);
            CreatePage(PDFdocument);

            return PDFdocument;
        }

        /// <summary>
        /// Defines the styles used in the document.
        /// </summary>
        public static void DefineStyles(Document document)
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
            style.ParagraphFormat.Borders.Width = 2.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Shading.Color = Colors.SkyBlue;

            // Create a new style called TOC based on style Normal
            style = document.Styles.AddStyle("TOC", "Normal");
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = Colors.Blue;
        }

        public static void CreateHeaderFooter(Document document)
        {
            Section page1 = document.AddSection();
            //page1.PageSetup.OddAndEvenPagesHeaderFooter = false;
            //page1.PageSetup.StartingNumber = 1;

            HeaderFooter head = page1.Headers.Primary;
            HeaderFooter foot = page1.Footers.Primary;


            string ImgPath = ((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath).Replace("bin/Candidaturas.DLL", "Content/img/logotipo.jpg");
            Image logo = head.AddImage(ImgPath);
            logo.LockAspectRatio = true;
            logo.Width = "1.5cm";
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

        public static void CreateComprovativo(Document document)
        {
            Section page1 = document.LastSection;
            Paragraph title = page1.AddParagraph("\n\nComprovativo de Candidatura", "Heading1");

            Paragraph cod = page1.AddParagraph("\nCódigo de Candidato: #123#-Fixed","Heading3");

            //DadosPessoai candidato = GetInfoCandidato(2);

            Paragraph Text1 = page1.AddParagraph("\n\nEu, abaixo assinado, #NOMECompleto#, filho de #Pai# e de #Mae#, natural de #DistritoNatural?#, #ConcelhoNatural?#" +
                ", residente em #Morada#, #CodPostal#, #Localidade#, #Concelho#, freguesia de #Freguesia#, distrito de #Distrito#, nascido em #datanasc, #EstadoCivil#" +
                "Nacionalidade ## com o #Tipo de Documento# nº #NumeroDocumento# válido até #datavalidade#, número de Contribuinte #NIF#, e ?contacto? #número de tel#," +
                " declaro por minha honra que nunca fui abatido ao Corpo de Alunos da Academia Militar ou Academia da Força Aérea por motivos disciplinares ou por " +
                "incapacidade para o serviço militar e que nunca fui excluído dos cursos da Escola Naval.", "LongText");

            Paragraph Text2 = page1.AddParagraph("\n\n\nDesejo ser admitido aos cursos de:", "LongText");
            page1.AddParagraph();
            page1.AddParagraph("1. - #Curso#");
            page1.AddParagraph("2. - #Curso#");
            page1.AddParagraph("3. - #Curso#");

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

        public static void CreatePage(Document document)
        {
            
            //Create a Page
            Section section = document.AddSection();
            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.SpaceAfter = "3cm";
            

            string ImgPath = ((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath).Replace("bin/Candidaturas.DLL", "Content/img/marinha.png");
            section.AddImage(ImgPath);

            paragraph = section.AddParagraph("A sample document that demonstrates the\ncapabilities of MigraDoc");
            paragraph.Format.Font.Size = 16;
            paragraph.Format.Font.Color = Colors.DarkRed;
            paragraph.Format.SpaceBefore = "8cm";
            paragraph.Format.SpaceAfter = "3cm";
            paragraph = section.AddParagraph(ImgPath);
            paragraph = section.AddParagraph("Rendering date: ");
            paragraph.AddDateField();
        }


        public static void DefineParagraphs(Document document)
        {
            Paragraph paragraph = document.LastSection.AddParagraph("Paragraph Layout Overview", "Heading1");
            paragraph.AddBookmark("Paragraphs");

            
            DemonstrateAlignment(document);
            DemonstrateIndent(document);
            DemonstrateFormattedText(document);
            DemonstrateBordersAndShading(document);
        }

        private static void DemonstrateAlignment(Document document)
        {
            string FillerText = "TextoTextoTextoTextoTextoTextoTextoTextoTextoTextoTexto";
            document.LastSection.AddParagraph("Alignment", "Heading2");

            document.LastSection.AddParagraph("Left Aligned", "Heading3");

            Paragraph paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.AddText(FillerText);

            document.LastSection.AddParagraph("Right Aligned", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.AddText(FillerText);

            document.LastSection.AddParagraph("Centered", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddText(FillerText);

            document.LastSection.AddParagraph("Justified", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Justify;
            paragraph.AddText(FillerText);
        }

        private static void DemonstrateIndent(Document document)
        {
            string FillerText = "TextoTextoTextoTextoTextoTextoTextoTextoTextoTextoTexto";
            document.LastSection.AddParagraph("Indent", "Heading2");

            document.LastSection.AddParagraph("Left Indent", "Heading3");

            Paragraph paragraph = document.LastSection.AddParagraph();
            paragraph.Format.LeftIndent = "2cm";
            paragraph.AddText(FillerText);

            document.LastSection.AddParagraph("Right Indent", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.RightIndent = "1in";
            paragraph.AddText(FillerText);

            document.LastSection.AddParagraph("First Line Indent", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.FirstLineIndent = "12mm";
            paragraph.AddText(FillerText);

            document.LastSection.AddParagraph("First Line Negative Indent", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.LeftIndent = "1.5cm";
            paragraph.Format.FirstLineIndent = "-1.5cm";
            paragraph.AddText(FillerText);
        }

        private static void DemonstrateFormattedText(Document document)
        {
            string FillerText = "TextoTextoTextoTextoTextoTextoTextoTextoTextoTextoTexto";
            document.LastSection.AddParagraph("Formatted Text", "Heading2");

            //document.LastSection.AddParagraph("Left Aligned", "Heading3");

            Paragraph paragraph = document.LastSection.AddParagraph();
            paragraph.AddText("Text can be formatted ");
            paragraph.AddFormattedText("bold", TextFormat.Bold);
            paragraph.AddText(", ");
            paragraph.AddFormattedText("italic", TextFormat.Italic);
            paragraph.AddText(", or ");
            paragraph.AddFormattedText("bold & italic", TextFormat.Bold | TextFormat.Italic);
            paragraph.AddText(".");
            paragraph.AddLineBreak();
            paragraph.AddText("You can set the ");
            FormattedText formattedText = paragraph.AddFormattedText("size ");
            formattedText.Size = 15;
            paragraph.AddText("the ");
            formattedText = paragraph.AddFormattedText("color ");
            formattedText.Color = Colors.Firebrick;
            paragraph.AddText("the ");
            formattedText = paragraph.AddFormattedText("font", new MigraDoc.DocumentObjectModel.Font("Verdana"));
            paragraph.AddText(".");
            paragraph.AddLineBreak();
            paragraph.AddText("You can set the ");
            formattedText = paragraph.AddFormattedText("subscript");
            formattedText.Subscript = true;
            paragraph.AddText(" or ");
            formattedText = paragraph.AddFormattedText("superscript");
            formattedText.Superscript = true;
            paragraph.AddText(".");
        }

        private static void DemonstrateBordersAndShading(Document document)
        {
            string FillerText = "TextoTextoTextoTextoTextoTextoTextoTextoTextoTextoTexto";
            document.LastSection.AddPageBreak();
            document.LastSection.AddParagraph("Borders and Shading", "Heading2");

            document.LastSection.AddParagraph("Border around Paragraph", "Heading3");

            Paragraph paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Borders.Width = 2.5;
            paragraph.Format.Borders.Color = Colors.Navy;
            paragraph.Format.Borders.Distance = 3;
            paragraph.AddText(FillerText);

            document.LastSection.AddParagraph("Shading", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Format.Shading.Color = Colors.LightCoral;
            paragraph.AddText(FillerText);

            document.LastSection.AddParagraph("Borders & Shading", "Heading3");

            paragraph = document.LastSection.AddParagraph();
            paragraph.Style = "TextBox";
            paragraph.AddText(FillerText);
        }

    }
}