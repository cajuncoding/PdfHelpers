using iTextSharp.text;

namespace PdfHelpers.Resize
{
    public class PdfScaledTemplateInfo
    {
        public ImgTemplate ScaledPdfContent { get; set; }
        public PdfPageOrientation ScaledPageOrientation { get; set; }
        public Rectangle ScaledPageSize { get; set; }
    }

}
