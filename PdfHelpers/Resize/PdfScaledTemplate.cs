using iTextSharp.text;

namespace PdfHelpers.Resize
{
    public class PdfScaledTemplateInfo
    {
        public ImgTemplate ScaledPdfContent { get; set; }
        public PdfPageOrientation ScaledPageOrientation => this.ScaledPageSize.Width > this.ScaledPageSize.Height ? PdfPageOrientation.Landscape : PdfPageOrientation.Portrait;
        public Rectangle ScaledPageSize { get; set; }
    }

}
