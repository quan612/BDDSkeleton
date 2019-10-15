using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using TestLinkApi;
using TechTalk.SpecFlow;

namespace TestLinkReporter
{
    public class TestLinkReport
    {
        // Link to api: https://metacpan.org/pod/TestLink::API
        // Using Singleton for the purpose of executing tests in parallel https://csharpindepth.com/articles/Singleton

        private readonly TestLink _testLink;
        private static readonly Lazy<TestLinkReport> Lazy = new Lazy<TestLinkReport>(() => new TestLinkReport());
        public static TestLinkReport Instance => Lazy.Value;

        public void ProcessResult(
            ScenarioContext testContext,
            string testProject,
            string testSuiteName,
            string testPlan,
            string status,
            string executionNotes
            )
        {
            //Getting the test case based on test project, test suite, test plan
            var testPlanRepository = _testLink.getTestPlanByName(testProject, testPlan);
            var testSuites = _testLink.GetTestSuitesForTestPlan(testPlanRepository.id);
            var testSuiteId = GetTestSuiteByName(testSuites, testSuiteName);
            var testCases = _testLink.GetTestCasesForTestSuite(testSuiteId, false);
            var testCaseRepository = GetIndividualTestCaseFromListOfTestCases(testCases, testContext);
            var testCaseStatus = GetTestCaseStatus(status);

            //updating the result on test link
            if (testCaseRepository != null)
                _testLink.ReportTCResult(
                    testCaseRepository.id,
                    testPlanRepository.id,
                    testCaseStatus, 
                    1, 
                    "Timon", 
                    false, 
                    true,
                    executionNotes);
        }

        private TestLinkReport()
        {
            var apiKey = ConfigurationManager.AppSettings["apiKey"];
            var testLinkUrl = ConfigurationManager.AppSettings["testLinkUrl"];
            _testLink = new TestLink(apiKey, testLinkUrl);
        }

        private string GetTestCaseStatus(string status)
        {
            switch (status)
            {
                case "OK":
                    return "p";
                case "Failed":
                    return "f";
                case "Blocked":
                    return "b";
                default:
                    return "f";
            }
        }

        private int GetTestSuiteByName(List<TestSuite> testSuites, string testSuiteName)
        {
            var suite = testSuites.Where(a => a.name == testSuiteName).ToList().FirstOrDefault();
            if (suite != null)
                return suite.id;
            throw new Exception("Cannot find test suite");
        }

        private TestCaseFromTestSuite GetIndividualTestCaseFromListOfTestCases(
            List<TestCaseFromTestSuite> testCases, ScenarioContext testContext)
        {
            var currentTestCase = testContext.ScenarioInfo.Description.Replace("\t", "");
            var testCaseRepository = testCases.Where(a => a.name == currentTestCase)
                .ToList().FirstOrDefault();
          
            if (testCaseRepository != null)
                return testCaseRepository;
            throw new Exception("Cannot find test case");
        }
    }
}
