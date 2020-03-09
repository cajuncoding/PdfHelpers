using iTextSharp.text;

namespace PdfHelpers.Resize
{
    /// <summary>
    /// BBernard
    /// Provides option parameters for Resizing of Pdf Page content.
    /// The size of the content is a factor of both Page Size and Margins because margins are implemented
    ///     by iTextSharp Document writer classes.  Therefore we conveniently support specifying both
    ///     to manage the scaled content in the Pdf output.
    ///
    /// Default Page size will be US Letter size
    /// Default Margin Size will be Zero/None
    ///
    /// NOTE: Margin is defaulted to None to prevent unnecessarily padding whitespace around already
    ///         scaled content which likely already had some margin set when it was first written
    /// NOTE: Margin is baked into final Pdf content as bounding boxes, so once compiled there is no
    ///         remaining concept of Margin, just content being imported from existing Pdf!
    /// </summary>
    public class PdfResizeInfo
    {
        public static PdfResizeInfo Default = new PdfResizeInfo();

        public PdfResizeInfo(Rectangle pageSize = null, PdfMarginRectangle marginSize = null, int rotationDegrees = 0)
        {
            //NOTE: Due to namespace conflicts we reference the fully qualified PageSize for iTextSharp.
            this.PageSize = pageSize ?? iTextSharp.text.PageSize.LETTER;
            this.MarginSize = marginSize ?? PdfMarginSize.None;
            this.RotationDegrees = rotationDegrees;
        }

        public Rectangle PageSize { get; set; }
        public PdfMarginRectangle MarginSize { get; set; }
        public int RotationDegrees { get; set; }
    }

}
