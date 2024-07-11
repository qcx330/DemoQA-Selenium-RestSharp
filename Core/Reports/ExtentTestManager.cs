using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Utilities;
using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using NUnit.Framework;
using Core.Reports;
using TechTalk.SpecFlow;
using Core.Driver;

namespace Core.Reports
{
    public class ExtentTestManager
    {
        private static AsyncLocal<ExtentTest> _parentTest = new AsyncLocal<ExtentTest>();
        private static AsyncLocal<ExtentTest> _childTest = new AsyncLocal<ExtentTest>();

        public static ExtentTest CreateParentTest(string className, string description = null)
        {
            _parentTest.Value = ExtentReportManager.Instance.CreateTest(className, description);
            return _parentTest.Value;
        }

        public static ExtentTest CreateTest(string testName, string description = null)
        {
            _childTest.Value = _parentTest.Value.CreateNode(testName, description);
            return _childTest.Value;
        }

        public static ExtentTest GetTest()
        {
            return _childTest.Value;
        }
        public static void LogTestOutcome(TestContext context, IWebDriver driver = null)
        {
            var outcome = context.Result.Outcome.Status;
            var stackTrace = string.IsNullOrEmpty(context.Result.StackTrace)
                ? ""
                : string.Format("<pre>{0}</pre>", context.Result.StackTrace);
            Status logStatus;
            var className = context.Test.ClassName;
            var testName = context.Test.Name;
            switch (outcome)
            {
                case TestStatus.Failed:
                    logStatus = Status.Fail;
                    if (driver != null)
                    {
                        var fileLocation = ScreenshotHelper.CaptureScreenshot(driver, className, testName);
                        testName = FileUtils.SanitizeFileName(testName);
                        var mediaEntity = ScreenshotHelper.CaptureScreenShotAndAttachToExtendReport(fileLocation);
                        ReportLog.Fail($"#Test Name:  {testName}  #Status:  {logStatus} {stackTrace}", mediaEntity);
                    }
                    else ReportLog.Fail($"#Test Name:  {testName}  #Status:  {logStatus} {stackTrace}");

                    // ReportLog.Fail("#Screenshot Below: " + ReportLog.AddScreenCaptureFromPath(fileLocation));
                    break;
                case TestStatus.Inconclusive:
                    logStatus = Status.Warning;
                    ReportLog.Skip("#Test Name: " + testName + " #Status: " + logStatus);
                    break;
                case TestStatus.Skipped:
                    logStatus = Status.Skip;
                    ReportLog.Skip("#Test Name: " + testName + " #Status: " + logStatus);
                    break;
                default:
                    logStatus = Status.Pass;
                    ReportLog.Pass("#Test Name: " + testName + " #Status: " + logStatus);
                    break;
            }
        }
        public static void CreateScenarioContext(ScenarioContext context)
        {
            var tags = String.Join(" ", context.ScenarioInfo.Tags);
            var testName = tags + " Scenario: " + context.ScenarioInfo.Title;
            if (context.ScenarioInfo.Arguments.Count > 0)
            {
                testName += " - [" + context.ScenarioInfo.Arguments[0] + "]";
            }

            CreateTest(testName);

            if (tags.ToLower().Contains("skip") || tags.ToLower().Contains("not support"))
                Assert.Inconclusive($@"Ignore test case");

            if (tags.ToLower().Contains("bugs"))
                Assert.Inconclusive($"Bug is existed {tags}");
        }
        public static void UpdateStepContext()
        {
            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            var stepName = ScenarioStepContext.Current.StepInfo.Text;
            var stepLog = stepType + " " + stepName;

            if (ScenarioStepContext.Current.StepInfo.Table != null)
            {
                stepLog += "<br>" + ScenarioStepContext.Current.StepInfo.Table.ToString();
                stepLog = stepLog.Replace("\n", "<br>");
            }

            if (ScenarioStepContext.Current.TestError == null)
                GetTest().Log(Status.Pass, stepLog);
            else
                GetTest().Log(Status.Fail, stepLog);
        }

        public static void UpdateScenarioContext()
        {
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                ? ""
                : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus = Status.Pass;

            if (ScenarioContext.Current.TestError != null)
            {
                if (ScenarioContext.Current.TestError.Message.Contains("Ignore"))
                    logstatus = Status.Skip;
                else if (ScenarioContext.Current.TestError.Message.Contains("Bug"))
                    logstatus = Status.Warning;
                else
                {
                    logstatus = Status.Fail;
                    DateTime time = DateTime.Now;
                    string fileName = "Screenshot_" + time.ToString("ddMMyyyy h_mm_ss") + ".png";
                    string filePath = DriverManager.WebDriver.Capture("TestResults\\" + fileName);
                    GetTest().AddScreenCaptureFromPath(filePath, title: fileName);
                    GetTest().Log(logstatus, "InnerException => " + ScenarioContext.Current.TestError.InnerException);
                    GetTest().Log(logstatus, "StackTrace => " + ScenarioContext.Current.TestError.StackTrace);
                    GetTest().Log(logstatus, "Image => " + filePath);
                }
                GetTest().Log(logstatus, "Message => " + ScenarioContext.Current.TestError.Message);
            }
            GetTest().Log(logstatus, "Test ended with " + logstatus + " - " + stacktrace);
        }
    }
}