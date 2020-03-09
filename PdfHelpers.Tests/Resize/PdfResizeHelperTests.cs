using iTextSharp.text;
using iTextSharp.text.pdf;
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
            //var originalPdfBytes = TestHelper.ReadTestDataFileBytes(@"TestDoc_01.pdf");
            //var originalPdfBytes = TestHelper.ReadTestDataFileBytes(@"ROTATED-INCORRECTLY-TEST-fec3dc70-e2dd-4765-9645-d4930b0b39d5.pdf");
            var originalPdfBytes = File.ReadAllBytes(@"D:\Develop\AEG Presents\Temp\MAES-2622 Investigation (Invoice File Resizing)\Bug\ROTATED-INCORRECTLY-TEST-CONTENT-791ec94a-5926-409e-910c-a39bb6000e9f.pdf");
            //var originalPdfBytes = File.ReadAllBytes(@"D:\Develop\AEG Presents\Temp\MAES-2622 Investigation (Invoice File Resizing)\Bug\ROTATED-INCORRECTLY-TEST-IMAGE-fec3dc70-e2dd-4765-9645-d4930b0b39d5.pdf");

            //*************************************************
            //Setup & Execute Tests...
            //*************************************************
            //RESIZE & scale LETTER Doc down to Postcard size, and validate the last page with the SMALL image is scaled up!
            var resizeInfo = new PdfResizeInfo(PageSize.POSTCARD, PdfMarginSize.None);
            var resizedBytes = PdfResizeHelper.ResizePdfPageSize(originalPdfBytes, resizeInfo, PdfScalingOptions.Default);

            //*************************************************
            //Validate Results...
            //*************************************************
            AssertThatPdfSizeIsAsExpected(resizedBytes, resizeInfo);

            File.WriteAllBytes($@"D:\Temp\PdfResizeHelper\RESIZED OUTPUT TEST - {Guid.NewGuid()}.pdf", resizedBytes);
        }


        public void AssertThatPdfSizeIsAsExpected(byte[] resizedBytes, PdfResizeInfo resizeInfo)
        {
            //*************************************************
            //Validate Results...
            //*************************************************
            using (var pdf = new PdfReader(resizedBytes))
            {
                var targetPageSize = resizeInfo.PageSize;
                for (var i = 1; i <= pdf.NumberOfPages; i++)
                {
                    //NOTE: To correctly validate we must get the Page Size WITH any Rotation being applied;
                    //      which is a  different method that we must call to get all correct details...
                    var currentPageSize = pdf.GetPageSizeWithRotation(i);

                    //Validate Landscape & Rotation...
                    if (currentPageSize.Width <= currentPageSize.Height)
                    {
                        //PORTRAIT
                        Assert.IsTrue(currentPageSize.Rotation == 0 || currentPageSize.Rotation == 180, $"Checking Portrait Page Rotation for Page[{i}]");
                    }
                    else
                    {
                        //LANDSCAPE
                        Assert.IsTrue(currentPageSize.Rotation == 90 || currentPageSize.Rotation == 270, $"Checking Landscape Page Rotation for Page[{i}]");

                        //Rotate the Target Page size so we can safely compare Width & Height...
                        targetPageSize = targetPageSize.Rotate();
                    }

                    //After handling Rotation we can validate the Width & Height of the Sizes!
                    Assert.AreEqual(currentPageSize.Width, targetPageSize.Width, $"Comparing PageSize Width for Page[{i}]");
                    Assert.AreEqual(currentPageSize.Height, targetPageSize.Height, $"Comparing PageSize Height for Page[{i}]");
                }
            }

        }
    }
}
