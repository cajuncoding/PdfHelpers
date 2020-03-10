using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfHelpers.Resize;
using System.IO;

namespace PdfHelpers.Convert
{
    public class PdfConvertHelper
    {
        public static byte[] ConvertImageToPdf(string filePath, PdfResizeInfo pageSizeInfo, PdfScalingOptions scalingOptions = null)
        {
            var fileInfo = new FileInfo(filePath);
            if(!fileInfo.Exists) throw new FileNotFoundException($"File specified could not be found [{filePath}].");

            var pdfBytes = ConvertImageToPdf(fileInfo, pageSizeInfo, scalingOptions);
            return pdfBytes;
        }

        public static byte[] ConvertImageToPdf(FileInfo fileInfo, PdfResizeInfo pageSizeInfo, PdfScalingOptions scalingOptions = null)
        {
            var imageBytes = File.ReadAllBytes(fileInfo.FullName);
            var pdfBytes = ConvertImageToPdf(imageBytes, pageSizeInfo, scalingOptions);
            return pdfBytes;
        }

        public static byte[] ConvertImageToPdf(byte[] imageBytes, PdfResizeInfo pageSizeInfo, PdfScalingOptions scalingOptions = null)
        {
            var iTextImage = Image.GetInstance(imageBytes);
            return ConvertToPdf(iTextImage, pageSizeInfo, scalingOptions);
        }

        public static byte[] ConvertToPdf(Image iTextImage, PdfResizeInfo pageSizeInfo, PdfScalingOptions scalingOptions = null)
        {
            var pdfOptions = scalingOptions ?? PdfScalingOptions.Default;

            //BBernard
            //Statically ensure that Compression is enabled...
            Document.Compress = pdfOptions.EnableCompression;

            var marginInfo = pageSizeInfo.MarginSize;

            using (var outputMemoryStream = new MemoryStream())
            using (var pdfDocBuilder = new Document(pageSizeInfo.PageSize, marginInfo.Left, marginInfo.Right, marginInfo.Top, marginInfo.Bottom))
            using (var pdfWriter = PdfWriter.GetInstance(pdfDocBuilder, outputMemoryStream))
            {
                if (pdfOptions.EnableScaling)
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
