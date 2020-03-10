using iTextSharp.awt.geom;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace PdfHelpers.Resize
{
    public static class PdfResizeHelper
    {
        public static byte[] ResizePdfPageSize(byte[] pdfBytes, PdfResizeInfo targetSizeInfo, PdfScalingOptions scalingOptions = null)
        {
            if(pdfBytes == null) throw new ArgumentNullException(nameof(pdfBytes), "Pdf Byte Array cannot be null.");
            if (targetSizeInfo == null) throw new ArgumentNullException(nameof(targetSizeInfo), "ResizeInfo cannot be null.");

            //Initialize with Default Scaling Options...
            var pdfScalingOptions = scalingOptions ?? PdfScalingOptions.Default;

            //BBernard
            //Statically ensure that Compression is enabled...
            Document.Compress = pdfScalingOptions.EnableCompression;
            PdfReader.unethicalreading = pdfScalingOptions.EnableUnethicalReading;

            var marginInfo = targetSizeInfo.MarginSize;

            using (var outputMemoryStream = new MemoryStream())
            using (var pdfDocBuilder = new Document(targetSizeInfo.PageSize, marginInfo.Left, marginInfo.Right, marginInfo.Top, marginInfo.Bottom))
            using (var pdfReader = new PdfReader(pdfBytes))
            using (var pdfWriter = PdfWriter.GetInstance(pdfDocBuilder, outputMemoryStream))
            {
                pdfDocBuilder.Open();

                var pageCount = pdfReader.NumberOfPages;
                for (int pageNumber = 1; pageNumber <= pageCount; pageNumber++)
                {
                    //Read the content for the current page...
                    //NOTE: We use the PdfWriter to import the Page (not the Document) to ensures that all required
                    //      references (e.g. Fonts, Symbols, Images, etc.) are all imported into the target Doc builder.
                    PdfImportedPage page = pdfWriter.GetImportedPage(pdfReader, pageNumber);

                    //BBernard
                    //Initialize the current size of the Content using the PdfReader
                    //NOTE: In order to correctly render existing PDF Pages, we must include any Rotation
                    //          that may already be defined in the current size.
                    //NOTE: The existing rotation can only be obtained from the PdfReader, and is not stored
                    //          with the PdfImportedPage, so we must get it by page number reference.
                    var originalPageSizeWithRotation = pdfReader.GetPageSizeWithRotation(pageNumber);

                    //Scale the content for the target parameters...
                    var scaledTemplateInfo = ScalePdfContent(page, originalPageSizeWithRotation, targetSizeInfo, pdfScalingOptions);

                    //Set the Page dimensions processed by the Scaling logic (e.g. Supports dynamic use of Landscape Orientation)...
                    //  and then move the doc cursor to initialize a new page with these settings so we can add the content.
                    pdfDocBuilder.SetPageSize(scaledTemplateInfo.ScaledPageSize);
                    pdfDocBuilder.NewPage();

                    //Add the scaled content to the Document (ie. Pdf Template with the Content Embedded)...
                    pdfDocBuilder.Add(scaledTemplateInfo.ScaledPdfContent);
                }

                pdfDocBuilder.Close();

                byte[] finalFileBytes = outputMemoryStream.ToArray();
                return finalFileBytes;
            }
        }

        public static PdfScaledTemplateInfo ScalePdfContent(Image image, PdfResizeInfo resizeInfo, PdfScalingOptions scalingOptions = null)
        {
            var currentSize = new Rectangle(image);
            var contentTemplate = new ImgTemplate(image);
            return ScalePdfContent(contentTemplate, currentSize, resizeInfo, scalingOptions);
        }

        public static PdfScaledTemplateInfo ScalePdfContent(PdfImportedPage currentPage, Rectangle currentContentSize, PdfResizeInfo resizeInfo, PdfScalingOptions scalingOptions = null)
        {
            var contentTemplate = new ImgTemplate(currentPage);
            return ScalePdfContent(contentTemplate, currentContentSize, resizeInfo, scalingOptions);
        }

        public static PdfScaledTemplateInfo ScalePdfContent(ImgTemplate contentTemplate, Rectangle currentContentSize, PdfResizeInfo resizeInfo, PdfScalingOptions scalingOptions = null)
        {
            if (contentTemplate == null) throw new ArgumentNullException(nameof(contentTemplate), "Pdf Content to be resized cannot be null.");
            if (currentContentSize == null) throw new ArgumentNullException(nameof(currentContentSize), "Current size of Pdf Content cannot be null.");
            if (resizeInfo == null) throw new ArgumentNullException(nameof(resizeInfo), "Target Pdf size information cannot be null.");

            //Initialize with Default Scaling Options...
            var pdfScalingOptions = scalingOptions ?? PdfScalingOptions.Default;

            //Don't mutate the original value...
            //var targetSize = targetDoc.PageSize;
            var pageSize = resizeInfo.PageSize;
            var marginSize = resizeInfo.MarginSize;

            var targetWidth = pageSize.Width - marginSize.Left - marginSize.Right;
            var targetHeight = pageSize.Height - marginSize.Top - marginSize.Bottom;

            bool scalingEnabled = false;
            //Flag to denote which dimension is the constraining one
            //NOTE: This changes based on if we are scaling the size Up or Down!
            switch (pdfScalingOptions.PdfContentScalingMode)
            {
                case PdfResizeScalingMode.ScaleAlways:
                    scalingEnabled = true;
                    break;
                case PdfResizeScalingMode.ScaleDownOnly:
                    scalingEnabled = currentContentSize.Width > targetWidth || currentContentSize.Height > targetHeight;
                    break;
                case PdfResizeScalingMode.ScaleUpOnly:
                    scalingEnabled = currentContentSize.Width < targetWidth || currentContentSize.Height < targetHeight;
                    break;
            }

            //BBernard
            //If Enabled then we handle dynamic rotation based on the input source rotation (if specified)...
            //NOTE: Rotation MUST be handled BEFORE the Scaling to ensure we scale with the appropriate Width & Height
            //      of the existing content!
            if (pdfScalingOptions.EnableDynamicRotationHandling && currentContentSize.Rotation > 0)
            {
                //BBernard
                //Compute Adjustment Rotation for correct rotation...if the input Source has a Rotation Already!
                //NOTE: Specific test scenarios have shown that to rotate content using iText the rotation direction
                //      appears to be counter clockwise in iText, vs the noted rotation of input documents.
                //      Therefore we compute the counter-clockwise equivalent to rotate teh content as we expect in these test cases...
                //For Example: If the input document has a rotation of 270, we need to actually rotate 90
                //              degrees via iTextSharp to ensure proper orientation.
                var rotationDegrees = ((360 - currentContentSize.Rotation) % 360);
                contentTemplate.RotationDegrees = rotationDegrees;
            }

            if (scalingEnabled)
            {
                //Determine if we should force Resize into the specified Target Size or if we should enable Landscape orientation 
                //  (e.g. 90 degree rotation) to accomodate pages that are wider than taller...
                //NOTE: If landscape orientation is enabled then we only need to make adjustments if all of the following are true:
                //      a) the current size is using landscape orientation
                //      b) and the target size is not already in the same orientation (e.g. landscape)
                if (pdfScalingOptions.EnableDynamicLandscapeOrientation
                    && currentContentSize.Width > currentContentSize.Height
                    && targetHeight > targetWidth)
                {
                    pageSize = pageSize.Rotate();
                    //Don't mutate the original value...
                    //var targetSize = targetDoc.PageSize;
                    targetWidth = pageSize.Width - marginSize.Left - marginSize.Right;
                    targetHeight = pageSize.Height - marginSize.Top - marginSize.Bottom;
                }

                //Support Maintaining Aspect Ratio...
                if (pdfScalingOptions.MaintainAspectRatio)
                {
                    contentTemplate.ScaleToFit(targetWidth, targetHeight);
                }
                //Support Skewed Resizing...
                else
                {
                    contentTemplate.ScaleAbsolute(targetWidth, targetHeight);
                }
            }

            //If Enabled then we adjust the position to center the content on the Page...
            if (pdfScalingOptions.EnableContentCentering)
            {
                var x = (targetWidth - contentTemplate.ScaledWidth) / 2;
                var y = (targetHeight - contentTemplate.ScaledHeight) / 2;
                contentTemplate.SetAbsolutePosition(x, y);
            }

            return new PdfScaledTemplateInfo()
            {
                ScaledPdfContent = contentTemplate,
                ScaledPageSize = pageSize
            };
        }

        public static Point ComputeCenteredLocationPoint(Rectangle currentSize, Rectangle targetPageSize)
        {
            var x = (targetPageSize.Width - currentSize.Width) / 2;
            var y = (targetPageSize.Height - currentSize.Height) / 2;
            var centeredPosition = new Point(x, y);
            return centeredPosition;
        }

    }
}