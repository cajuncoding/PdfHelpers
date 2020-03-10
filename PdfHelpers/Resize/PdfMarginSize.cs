using iTextSharp.text;

namespace PdfHelpers.Resize
{
    public class PdfMarginRectangle : Rectangle
    {
        public PdfMarginRectangle(float left, float bottom, float right, float top)
            : base(left, bottom, right, top)
        { }

        public PdfMarginRectangle(float leftRight, float topBottom)
            : base(leftRight, topBottom, leftRight, topBottom)
        { }

        public PdfMarginRectangle(float margin)
            : base(margin, margin, margin, margin)
        { }

        /// <summary>
        /// Convert from an existing Rectangle
        /// NOTE: iText Rectangle are anchored at the Bottom-Left corner so we must compute adjustments for the Top & Right!
        /// </summary>
        /// <param name="marginRect"></param>
        public PdfMarginRectangle(Rectangle marginRect)
            : this(
                marginRect.Left,
                marginRect.Bottom,
                marginRect.Width - marginRect.Right,
                marginRect.Height - marginRect.Top
            )
        { }


        public new PdfMarginRectangle Rotate()
        {
            var rotatedRectangle = base.Rotate();
            var newMarginRect = new PdfMarginRectangle(rotatedRectangle.Left, rotatedRectangle.Bottom, rotatedRectangle.Right, rotatedRectangle.Top)
            {
                Rotation = rotatedRectangle.Rotation
            };
            return newMarginRect;
        }

        public override string ToString()
        {
            return $"Margin Rectangle: [L:{this.Left}, B:{this.Bottom}, R:{this.Right}, T:{this.Top}] (rot: {this.Rotation} degrees)";
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
