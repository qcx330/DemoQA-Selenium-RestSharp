using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace Core.Reports
{
    public static class ScreenshotHelper
    {
        public static string CaptureScreenshot(this IWebDriver driver, string className, string testName)
        {
            try
            {
                ITakesScreenshot ts = (ITakesScreenshot)driver;
                Screenshot screenshot = ts.GetScreenshot();
                var screenshotDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots", className);
                testName = testName.Replace("\"", "");
                var fileName = string.Format(@"Screenshot_{0}_{1}", testName, DateTime.Now.ToString("yyyyMMdd_HHmmssff"));
                Directory.CreateDirectory(screenshotDirectory);
                var fileLocation = string.Format(@"{0}\{1}.png", screenshotDirectory, fileName);
                screenshot.SaveAsFile(fileLocation);
                return fileLocation;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CaptureScreenshot: " + ex.Message);
                throw;
            }
        }
        public static MediaEntityModelProvider CaptureScreenShotAndAttachToExtendReport(string path)
        {
            return MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build();
        }

        public static string Capture( this IWebDriver driver ,string filePath)
        {
            try
            {
                var screenshotDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                var fileName = string.Format(@"Screenshot_{0}_{1}", DateTime.Now.ToString("yyyyMMdd_HHmmssff"));
                var fileLocation = string.Format(@"{0}\{1}.png", screenshotDirectory, fileName);
                screenshot.SaveAsFile(fileLocation);
                return fullPath;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error capturing screenshot: {e.Message}");
                throw;
            }
        }
    }
}