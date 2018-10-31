using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Candidaturas
{
    public class DocumentValidator
    {
        //verifica se o ficheiro tem extensão JPEG
        public static bool HasJpegExtension(HttpPostedFileBase file)
        {
            return Path.GetExtension(file.FileName) == ".jpeg" || Path.GetExtension(file.FileName) == ".jpg";
        }

        //verifica se o ficheiro tem extensão PDF
        public static bool HasPdfExtension(HttpPostedFileBase file)
        {
            return Path.GetExtension(file.FileName) == ".pdf";
        }

        //verifica se o ficheiro tem cabeçalho JPEG
        public static bool HasJpegHeader(HttpPostedFileBase file)
        {
            BinaryReader br = new BinaryReader(file.InputStream);
            ushort soi = br.ReadUInt16();  // Start of Image (SOI) marker (FFD8)
            ushort marker = br.ReadUInt16(); // JFIF marker (FFE0) or EXIF marker(FF01)

            return soi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff;
        }

        //verifica se o ficheiro tem cabeçalho PDF
        public static bool HasPdfHeader(HttpPostedFileBase file)
        {
            var pdfString = "%PDF-";
            var pdfBytes = Encoding.ASCII.GetBytes(pdfString);
            var len = pdfBytes.Length;
            var buf = new byte[len];
            var remaining = len;
            var pos = 0;
            var f = file.InputStream;

            while (remaining > 0)
            {
                var amtRead = f.Read(buf, pos, remaining);
                if (amtRead == 0) return false;
                remaining -= amtRead;
                pos += amtRead;
            }

            return pdfBytes.SequenceEqual(buf);
        }

        //verifica se o ficheiro é do tipo JPEG
        public static bool IsJpeg(HttpPostedFileBase file)
        {
            return HasJpegExtension(file) && HasJpegHeader(file);
        }

        //verifica se o ficheiro é do tipo PDF
        public static bool IsPdf(HttpPostedFileBase file)
        {
            return HasPdfExtension(file) && HasPdfHeader(file);
        }
    }
}