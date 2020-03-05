using iTextSharp.text;

namespace PdfHelpers.Resize
{
    public class PdfScaledTemplateInfo
    {
        public ImgTemplate ScaledPdfContent { get; set; }
        public PdfPageOrientation PageOrientation { get; set; }
        public Rectangle TargetPageSize { get; set; }
    }

}
