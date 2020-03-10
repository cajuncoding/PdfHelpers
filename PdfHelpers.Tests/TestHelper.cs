using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfHelper;
using PdfHelpers.Resize;
using System;
using System.IO;
using System.Reflection;

namespace PdfHelpers.Tests
{
    public class TestHelper
    {
        private static readonly string _appPath = SanitizePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

        /// <summary>
        /// BBernard
        /// Helper to quickly & easily get the current TestData folder path using the currently executing Unit Test Excutable.
        /// </summary>
        /// <param name="subPath"></param>
        /// <returns></returns>
        public static string GetTestDataPath(string subPath = "")
        {
            var sanitizedSubPath = SanitizePath(subPath);
            var testDataPath = $@"{_appPath}{Path.DirectorySeparatorChar}TestData{Path.DirectorySeparatorChar}{sanitizedSubPath}";
            return testDataPath;
        }

        private static string SanitizePath(string path)
        {
            return path.Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        /// <summary>
        /// BBernard
        /// Helper to get the FileInfo path of a test data file...
        /// </summary>
        /// <param name="relativeFilePathName"></param>
        /// <returns></returns>
        public static FileInfo GetTestDataFileInfo(string relativeFilePathName)
        {
            var dataFolderPath = GetTestDataPath();
            var filePath = Path.Combine(dataFolderPath, SanitizePath(relativeFilePathName));
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists) throw new ArgumentException("File does not exist.");

            return fileInfo;
        }

        /// <summary>
        /// BBernard
        /// Helper to quickly & easily read the text of a Test Data file for Unit Tests.
        /// </summary>
        /// <param name="testClass"></param>
        /// <param name="relativeFilePathName"></param>
        /// <returns></returns>
        public static string ReadTestDataFile(string relativeFilePathName)
        {
            var fileInfo = GetTestDataFileInfo(relativeFilePathName);
            var textData = File.ReadAllText(fileInfo.FullName);
            return textData;
        }

        /// <summary>
        /// BBernard
        /// Helper to quickly & easily read the text of a Test Data file for Unit Tests.
        /// </summary>
        /// <param name="testClass"></param>
        /// <param name="relativeFilePathName"></param>
        /// <returns></returns>
        public static byte[] ReadTestDataFileBytes(string relativeFilePathName)
        {
            var fileInfo = GetTestDataFileInfo(relativeFilePathName);
            var bytes = File.ReadAllBytes(fileInfo.FullName);
            return bytes;
        }

        public static void AssertThatPdfPageSizeIsAsExpected(byte[] pdfBytes, PdfResizeInfo resizeInfo)
        {
            //*************************************************
            //Validate Results...
            //*************************************************
            using (var pdfReader = new PdfReader(pdfBytes))
            {
                for (var pageNumber = 1; pageNumber <= pdfReader.NumberOfPages; pageNumber++)
                {
                    //NOTE: Init/Reset the target page sizes for validation on each page since we may be mutating/rotating
                    //      them to validate Landscape/Rotated outputs...
                    var targetPageSize = resizeInfo.PageSize;
                    var targetMarginSize = resizeInfo.MarginSize;


                    //NOTE: To correctly validate we must get the Page Size WITH any Rotation being applied;
                    //      which is a  different method that we must call to get all correct details...
                    var currentPageSize = pdfReader.GetPageSizeWithRotation(pageNumber);

                    //Validate Landscape & Rotation...
                    if (currentPageSize.Width <= currentPageSize.Height)
                    {
                        //PORTRAIT
                        Assert.IsTrue(currentPageSize.Rotation == 0 || currentPageSize.Rotation == 180, $"Checking Portrait Page Rotation for Page[{pageNumber}]");
                    }
                    else
                    {
                        //LANDSCAPE
                        Assert.IsTrue(currentPageSize.Rotation == 90 || currentPageSize.Rotation == 270, $"Checking Landscape Page Rotation for Page[{pageNumber}]");

                        //Rotate the Target Page size so we can safely compare Width & Height...
                        targetPageSize = targetPageSize.Rotate();
                        targetMarginSize = targetMarginSize.Rotate();
                    }

                    //After handling Rotation we can validate the Width & Height of the Sizes!
                    Assert.AreEqual(currentPageSize.Width, targetPageSize.Width, $"Comparing PageSize Width for Page[{pageNumber}]");
                    Assert.AreEqual(currentPageSize.Height, targetPageSize.Height, $"Comparing PageSize Height for Page[{pageNumber}]");

                    //After handling Rotation we can validate the Margins!
                    var marginSize = pdfReader.GetPdfMarginRectangleFromCropBox(pageNumber);
                    Assert.AreEqual(marginSize.Left, targetMarginSize.Left, "Check Page Margin Left Matches");
                    Assert.AreEqual(marginSize.Right, targetMarginSize.Right, "Check Page Margin Right Matches");
                    Assert.AreEqual(marginSize.Top, targetMarginSize.Top, "Check Page Margin Top Matches");
                    Assert.AreEqual(marginSize.Bottom, targetMarginSize.Bottom, "Check Page Margin Bottom Matches");

                }
            }

        }

    }
}
