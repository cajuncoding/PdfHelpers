using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfHelpers.Convert;
using PdfHelpers.Merge;
using PdfHelpers.Resize;
using System;
using System.IO;

namespace PdfHelpers.Tests.Resize
{
    [TestClass]
    public class PdfMergeTests
    {
        [TestMethod]
        public void PdfMergeImagesWithExistingDocTest()
        {
            //*************************************************
            //Setup & Execute Tests...
            //*************************************************
            //RESIZE & scale LETTER Doc down to Postcard size, and validate the last page with the SMALL image is scaled up!
            var resizeInfo = new PdfResizeInfo(PageSize.POSTCARD, PdfMarginSize.None);

            var testDocPdfBytes = TestHelper.ReadTestDataFileBytes(@"TestDoc_01.pdf");

            var floppyDiskPdfBytes = PdfConvertHelper.ConvertImageToPdf(
                TestHelper.GetTestDataFileInfo(@"floppy_disks.jpg"), 
                resizeInfo, 
                PdfScalingOptions.Default
            );

            var satellitePdfBytes = PdfConvertHelper.ConvertImageToPdf(
                TestHelper.GetTestDataFileInfo(@"satellite.jpg"), 
                resizeInfo, 
                PdfScalingOptions.Default
            );

            var mergedPdfBytes = PdfMergeHelper.MergePdfFiles(resizeInfo, PdfScalingOptions.Default, testDocPdfBytes, floppyDiskPdfBytes, satellitePdfBytes);

            //*************************************************
            //Validate Results...
            //*************************************************
            using (var pdfReader = new PdfReader(mergedPdfBytes))
            {
                Assert.AreEqual(pdfReader.NumberOfPages, 6, "Check Page Count = 6");
            }

            //Handle dynamic rotation validation (for wide images)...
            TestHelper.AssertThatPdfPageSizeIsAsExpected(mergedPdfBytes, resizeInfo);

            File.WriteAllBytes($@"D:\Temp\PdfResizeHelper\MERGED PDF FILE - {Guid.NewGuid()}.pdf", mergedPdfBytes);
        }

    }
}
