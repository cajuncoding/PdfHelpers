using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfHelpers.Resize;
using System.IO;

namespace PdfHelpers.Convert
{
    public class PdfConvertHelper
    {
        public byte[] ConvertToPdf(Image iTextImage, PdfResizeInfo pageSizeInfo, PdfScalingOptions scalingOptions = null)
        {
            var marginInfo = pageSizeInfo.MarginSize;

            using (var outputMemoryStream = new MemoryStream())
            using (var pdfDocBuilder = new Document(pageSizeInfo.PageSize, marginInfo.Left, marginInfo.Right, marginInfo.Top, marginInfo.Bottom))
            using (var pdfWriter = PdfWriter.GetInstance(pdfDocBuilder, outputMemoryStream))
            {
                if (scalingOptions != null)
                {
                    var scaledContent = PdfResizeHelper.ScalePdfContent(iTextImage, pageSizeInfo, scalingOptions);
                    pdfDocBuilder.SetPageSize(scaledContent.ScaledPageSize);

                    pdfDocBuilder.Open();
                    pdfDocBuilder.Add(scaledContent.ScaledPdfContent);
                }
                else
                {
                    pdfDocBuilder.Open();
                    pdfDocBuilder.Add(iTextImage);
                }

                pdfDocBuilder.Close();

                var pdfBytes = outputMemoryStream.ToArray();
                return pdfBytes;
            }
        }
    }
}
