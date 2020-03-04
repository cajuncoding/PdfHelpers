using iTextSharp.text;

namespace PdfResizeHelper
{
    public class PdfScaledTemplateInfo
    {
        public ImgTemplate ScaledPdfTemplate { get; set; }
        public PdfPageOrientation PageOrientation { get; set; }
        public Rectangle TargetPageSize { get; set; }
    }

}
