using iTextSharp.text.pdf;
using PdfHelpers.Resize;

namespace PdfHelper
{
    public static class iTextSharpPdfReaderCustomExtensions
    {
        public static PdfMarginRectangle GetPdfMarginRectangleFromCropBox(this PdfReader pdfReader, int pageNumber)
        {
            var cropBox = pdfReader.GetCropBox(pageNumber);
            var marginSize = new PdfMarginRectangle(cropBox);
            return marginSize;
        }
    }


    public static class iTextSharpPdfCopyCustomExtensions
    {
        public static void AppendPdfDocument(this PdfCopy pdfCopy, byte[] pdfBytes)
        {
            using (var pdfReader = new PdfReader(pdfBytes))
            {
                pdfCopy.AddDocument(pdfReader);
            }
        }
    }
}
