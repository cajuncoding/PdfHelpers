﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfResizeHelper.Parameters;
using System.IO;

namespace PdfResizeHelper.Tests
{
    [TestClass]
    public class TestPdfResizeHelper
    {
        [TestMethod]
        public void PdfResizeHelperSimpleTest()
        {
            var originalPdfBytes = TestHelper.ReadTestDataFileBytes(@"TestDoc_01.pdf");

            var pdfBytes = File.ReadAllBytes("");

            //*************************************************
            //Setup & Execute Tests...
            //*************************************************

            //RESIZE & scale LETTER Doc down to Postcard size, and validate the last page with the SMALL image is scaled up!
            var targetSizeInfo = new PdfResizeInfo(PageSize.POSTCARD, PdfMarginSize.None);
            var resizedBytes = PdfResizeHelper.ResizePdfPageSize(originalPdfBytes, targetSizeInfo, PdfScalingOptions.Default);

            //*************************************************
            //Validate Results...
            //*************************************************
            using (var pdf = new PdfReader(resizedBytes))
            using (var doc = new Document(targetSizeInfo.PageSize))
            {
                var targetPageSize = targetSizeInfo.PageSize;

                for (var i = 1; i <= pdf.NumberOfPages; i++)
                {
                    var pageSize = pdf.GetPageSize(i);
                    //NOTE: Our Width & Height will match even for Landscape pages because when implemented correctly in Pdf rendering, then
                    //      Landscape pages will have the same Width & Height but also have a Rotation of 90 degrees!
                    Assert.AreEqual(pageSize.Width, targetPageSize.Width, $"Comparing PageSize Width for Page[{i}]");
                    Assert.AreEqual(pageSize.Height, targetPageSize.Height, $"Comparing PageSize Height for Page[{i}]");
                }
            }

            //var fileName = $@"D:\Temp\PdfResizeHelper\RESIZED OUTPUT TEST - {Guid.NewGuid()}.pdf";
            //File.WriteAllBytes(fileName, resizedBytes);
        }
    }
}
