using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfHelper;
using PdfHelpers.Resize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PdfHelpers.Merge
{
    public class PdfMergeHelper
    {
        public static byte[] MergePdfFiles(PdfResizeInfo pageSizeInfo, params byte[] pdfFileParams)
        {
            return MergePdfFiles(pageSizeInfo, null, pdfFileParams);
        }

        public static byte[] MergePdfFiles(PdfResizeInfo pageSizeInfo, PdfScalingOptions scalingOptions, params byte[][] pdfFileParams)
        {
            var pdfFileList = pdfFileParams.ToList();
            return MergePdfFiles(pdfFileList, pageSizeInfo, scalingOptions);
        }

        public static byte[] MergePdfFiles(List<byte[]> pdfFileBytesList, PdfResizeInfo pageSizeInfo, PdfScalingOptions scalingOptions = null)
        {
            if (pdfFileBytesList == null || !pdfFileBytesList.Any()) throw new ArgumentNullException(nameof(pdfFileBytesList), "List of Pdf Files to be merged cannot be null or empty.");
            if (pageSizeInfo == null) throw new ArgumentNullException(nameof(pageSizeInfo), "ResizeInfo cannot be null.");

            byte[] outputBytes = null;
            
            //NOTE: For Merging we DISABLE ALL SCALING by default, so that we can preserve Annotations, etc.
            //          but this can be easily set by the client code to enable scaling.
            var pdfScalingOptions = scalingOptions ?? PdfScalingOptions.DisableScaling;

            //Set static references for Pdf Processing...
            PdfReader.unethicalreading = pdfScalingOptions.EnableUnethicalReading;
            Document.Compress = pdfScalingOptions.EnableCompression;

            var targetPageSize = pageSizeInfo.PageSize;
            var targetMarginSize = pageSizeInfo.MarginSize;

            using (var outputMemoryStream = new MemoryStream())
            using (var pdfDocBuilder = new Document(targetPageSize, targetMarginSize.Left, targetMarginSize.Right, targetMarginSize.Top, targetMarginSize.Bottom))
            using (var pdfSmartCopy = new PdfSmartCopy(pdfDocBuilder, outputMemoryStream))
            {
                pdfDocBuilder.Open();

                foreach (var pdfBytes in pdfFileBytesList)
                {
                    if (pdfScalingOptions.EnableScaling)
                    {
                        var scaledPdfBytes = PdfResizeHelper.ResizePdfPageSize(pdfBytes, pageSizeInfo, pdfScalingOptions);
                        pdfSmartCopy.AppendPdfDocument(scaledPdfBytes);
                    }
                    else
                    {
                        pdfSmartCopy.AppendPdfDocument(pdfBytes);
                    }
                }

                pdfDocBuilder.Close();
                outputBytes = outputMemoryStream.ToArray();
            }

            return outputBytes;
        }
    }
}
