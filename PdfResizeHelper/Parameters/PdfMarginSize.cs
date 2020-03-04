using iTextSharp.text;

namespace PdfResizeHelper
{
    public class PdfMarginRectangle : RectangleReadOnly
    {
        public PdfMarginRectangle(float left, float bottom, float right, float top)
            : base(left, bottom, right, top)
        {
        }

        public PdfMarginRectangle(float leftRight, float topBottom)
            : base(leftRight, topBottom, leftRight, topBottom)
        {
        }

        public PdfMarginRectangle(float margin)
            : base(margin, margin, margin, margin)
        {
        }
    }

    public class PdfMarginSize
    {
        public const int DPI = 72;
        public const float HALF_INCH_DPI = (float)0.5 * DPI;

        public static PdfMarginRectangle None = new PdfMarginRectangle(0, 0, 0, 0);
        public static PdfMarginRectangle HalfInch = new PdfMarginRectangle(HALF_INCH_DPI, HALF_INCH_DPI, HALF_INCH_DPI, HALF_INCH_DPI);
        public static PdfMarginRectangle OneInch = new PdfMarginRectangle(DPI, DPI, DPI, DPI);
    }
}
