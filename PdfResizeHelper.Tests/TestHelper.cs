using System.IO;
using System.Reflection;

namespace PdfResizeHelper.Tests
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
        /// Helper to quickly & easily read the text of a Test Data file for Unit Tests.
        /// </summary>
        /// <param name="testClass"></param>
        /// <param name="relativeFilePathName"></param>
        /// <returns></returns>
        public static string ReadTestDataFile(string relativeFilePathName)
        {
            var dataFolderPath = GetTestDataPath();
            var filePath = $@"{dataFolderPath}{Path.DirectorySeparatorChar}{SanitizePath(relativeFilePathName)}";
            var textData = File.ReadAllText(filePath);
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
            var dataFolderPath = GetTestDataPath();
            var filePath = Path.Combine(dataFolderPath, SanitizePath(relativeFilePathName));
            var bytes = File.ReadAllBytes(filePath);
            return bytes;
        }

    }
}
