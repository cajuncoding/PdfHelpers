using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfHelpers.Convert;
using PdfHelpers.Resize;

namespace PdfHelpers.Tests.Resize
{
    [TestClass]
    public class PdfImageConvertTests
    {
        [TestMethod]
        public void PdfImageBytesConversionWithScalingTest()
        {
            var imageBytes = TestHelper.ReadTestDataFileBytes(@"floppy_disks.jpg");

            //*************************************************
            //Setup & Execute Tests...
            //*************************************************
            //RESIZE & scale LETTER Doc down to Postcard size, and validate the last page with the SMALL image is scaled up!
            var resizeInfo = new PdfResizeInfo(PageSize.POSTCARD, PdfMarginSize.None);
            var pdfBytes = PdfConvertHelper.ConvertImageToPdf(imageBytes, resizeInfo, PdfScalingOptions.Default);

            //*************************************************
            //Validate Results...
            //*************************************************
            using (var pdfReader = new PdfReader(pdfBytes))
            {
                Assert.AreEqual(pdfReader.NumberOfPages, 1, "Check Page Count = 1");
            }

            //Handle dynamic rotation validation (for wide images)...
            TestHelper.AssertThatPdfPageSizeIsAsExpected(pdfBytes, resizeInfo);

            //File.WriteAllBytes($@"D:\Temp\PdfResizeHelper\IMAGE CONVERSION TO PDF - {Guid.NewGuid()}.pdf", pdfBytes);
        }

    }
}
