using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;

namespace SpecFlowIntegration.ReportUtility
{
    public class SpecFlowReport
    {
        private static readonly Lazy<ExtentReports> _lazy = new Lazy<ExtentReports>(() => new ExtentReports());

        public static ExtentReports Instance { get { return _lazy.Value; } }

        static SpecFlowReport()
        {
            var dt = DateTime.Now.ToString("yyyy_MM_dd_HH-mm-ss");
            var date = DateTime.Now.ToString("yyyy_MM_dd");
            //var reportDirectory = System.Configuration.ConfigurationManager.AppSettings["ReportPath"];
            var reportDirectory = @"\\timon\Reports";
            var tempDirectory = reportDirectory + "\\" + date;

            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            Console.WriteLine(tempDirectory);
            var reportPath = tempDirectory + "\\ExtentStepLogs_" + dt + ".html";

            var htmlReporter = new ExtentHtmlReporter(reportPath);
            htmlReporter.Config.Theme = Theme.Standard;
            Instance.AttachReporter(htmlReporter);
        }

        private SpecFlowReport()
        {
        }
    }
}
