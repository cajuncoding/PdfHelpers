using iTextSharp.awt.geom;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfResizeHelper.Parameters;
using System;
using System.IO;

namespace PdfResizeHelper
{

    public class PdfResizeHelper
    {
        public static byte[] ResizePdfPageSize(byte[] pdfBytes, PdfResizeInfo targetSizeInfo, PdfScalingOptions scalingOptions = null)
        {
            if(pdfBytes == null) throw new ArgumentNullException(nameof(pdfBytes), "Pdf Byte Array cannot be null.");
            if (targetSizeInfo == null) throw new ArgumentNullException(nameof(targetSizeInfo), "ResizeInfo cannot be null.");

            //Initialize with Default Scaling Options...
            var pdfScalingOptions = scalingOptions ?? PdfScalingOptions.Default;

            //BBernard
            //Statically ensure that Compression is enabled...
            Document.Compress = true;

            using (var outputMemoryStream = new MemoryStream())
            using (var targetDoc = new Document(targetSizeInfo.PageSize))
            using (var pdfReader = new PdfReader(pdfBytes))
            using (var pdfWriter = PdfWriter.GetInstance(targetDoc, outputMemoryStream))
            {
                targetDoc.Open();

                var marginSizeRect = targetSizeInfo.MarginSize;
                targetDoc.SetMargins(
                    marginSizeRect.Left,
                    marginSizeRect.Right,
                    marginSizeRect.Top,
                    marginSizeRect.Bottom
                );

                var pageCount = pdfReader.NumberOfPages;
                for (int pageNumber = 1; pageNumber <= pageCount; pageNumber++)
                {
                    //Read the content for the current page...
                    PdfImportedPage page = pdfWriter.GetImportedPage(pdfReader, pageNumber);

                    //Scale the content for the target parameters...
                    var scaledTemplateInfo = ScalePdfContentForTargetDoc(page, targetDoc, pdfScalingOptions);

                    //Set the Page dimensions processed by the Scaling logic (e.g. Supports dynamic use of Landscape Orientation)...
                    //  and then move the doc cursor to initialize a new page with these settings so we can add the content.
                    targetDoc.SetPageSize(scaledTemplateInfo.TargetPageSize);
                    targetDoc.NewPage();

                    //Add the scaled content to the Document (ie. Pdf Template with the Content Embedded)...
                    targetDoc.Add(scaledTemplateInfo.ScaledPdfTemplate);
                }

                targetDoc.Close();

                byte[] finalFileBytes = outputMemoryStream.ToArray();
                return finalFileBytes;
            }
        }

        public static PdfScaledTemplateInfo ScalePdfContentForTargetDoc(PdfImportedPage currentPage, Document targetDoc, PdfScalingOptions scalingOptions = null)
        {
            if (currentPage == null) throw new ArgumentNullException(nameof(currentPage), "Pdf Imported Page cannot be null.");
            if (targetDoc == null) throw new ArgumentNullException(nameof(targetDoc), "Target Pdf Document builder cannot be null.");

            var pdfTemplateHelper = new ImgTemplate(currentPage);

            //Initialize with Default Scaling Options...
            var pdfScalingOptions = scalingOptions ?? PdfScalingOptions.Default;

            //Don't mutate the original value...
            //var targetSize = targetDoc.PageSize;
            var targetSize = targetDoc.PageSize;
            var targetWidth = targetSize.Width - targetDoc.LeftMargin - targetDoc.RightMargin;
            var targetHeight = targetSize.Height - targetDoc.TopMargin - targetDoc.BottomMargin;
            var pageOrientation = PdfPageOrientation.Portrait;

            //Determine if we should force Resize into the specified Target Size or if we should enable Landscape orientation 
            //  (e.g. 90 degree rotation) to accomodate pages that are wider than taller...
            //NOTE: If landscape orientation is enabled then we only need to make adjustments if all of the following are true:
            //      a) the current size is using landscape orientation
            //      b) and the target size is not already in the same orientation (e.g. landscape)
            if (pdfScalingOptions.EnableDynamicLandscapeOrientation
                && pdfTemplateHelper.Width > pdfTemplateHelper.Height 
                && targetHeight > targetWidth)
            {
                targetSize = targetDoc.PageSize.Rotate();
                //Don't mutate the original value...
                //var targetSize = targetDoc.PageSize;
                targetWidth = targetSize.Width - targetDoc.LeftMargin - targetDoc.RightMargin;
                targetHeight = targetSize.Height - targetDoc.TopMargin - targetDoc.BottomMargin;

                pageOrientation = PdfPageOrientation.Landscape;
            }

            bool scalingEnabled = false;
            //Flag to denote which dimension is the constraining one
            //NOTE: This changes based on if we are scaling the size Up or Down!
            switch (pdfScalingOptions.PdfContentScalingMode)
            {
                case PdfResizeScalingMode.ScaleAlways:
                    scalingEnabled = true;
                    break;
                case PdfResizeScalingMode.ScaleDownOnly:
                    scalingEnabled = pdfTemplateHelper.Width > targetWidth || pdfTemplateHelper.Height > targetHeight;
                    break;
                case PdfResizeScalingMode.ScaleUpOnly:
                    scalingEnabled = pdfTemplateHelper.Width < targetWidth || pdfTemplateHelper.Height < targetHeight;
                    break;
            }

            //Support Maintaining Aspect Ratio...
            if (scalingEnabled && pdfScalingOptions.MaintainAspectRatio)
            {
                pdfTemplateHelper.ScaleToFit(targetWidth, targetHeight);
            }
            //Support Skewed Resizing...
            else if (scalingEnabled)
            {
                pdfTemplateHelper.ScaleAbsolute(targetWidth, targetHeight);
            }
            //Do nothing if scaling is not enabled due to parameters and current size details...
            //else { }

            return new PdfScaledTemplateInfo()
            {
                ScaledPdfTemplate = pdfTemplateHelper,
                PageOrientation = pageOrientation,
                TargetPageSize = targetSize
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