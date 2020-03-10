using iTextSharp.text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfHelpers.Resize;
using System;
using System.IO;

namespace PdfHelpers.Tests.Resize
{
    [TestClass]
    public class PdfResizeHelperTests
    {
        [TestMethod]
        public void PdfResizeHelperSimpleTest()
        {
            var originalPdfBytes = TestHelper.ReadTestDataFileBytes(@"TestDoc_01.pdf");

            //*************************************************
            //Setup & Execute Tests...
            //*************************************************
            //RESIZE & scale LETTER Doc down to Postcard size, and validate the last page with the SMALL image is scaled up!
            var resizeInfo = new PdfResizeInfo(PageSize.POSTCARD, PdfMarginSize.None);
            var resizedBytes = PdfResizeHelper.ResizePdfPageSize(originalPdfBytes, resizeInfo, PdfScalingOptions.Default);

            //*************************************************
            //Validate Results...
            //*************************************************
            TestHelper.AssertThatPdfPageSizeIsAsExpected(resizedBytes, resizeInfo);

            File.WriteAllBytes($@"D:\Temp\PdfResizeHelper\RESIZED OUTPUT TEST - {Guid.NewGuid()}.pdf", resizedBytes);
        }
    }
}
