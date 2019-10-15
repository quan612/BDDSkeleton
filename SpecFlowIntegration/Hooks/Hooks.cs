using System;
using AventStack.ExtentReports;
using BoDi;
using NUnit.Framework;
using SpecFlowIntegration.ReportUtility;
using TechTalk.SpecFlow;

namespace SpecFlowIntegration.Hooks
{
    [Binding]
    public class Hooks
    {
        private TestScopeContext _testScope;
        private ExtentTest _extentTest;
        private readonly IObjectContainer _objectContainer;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void SetUpTestScope()
        {
            
        }

        [AfterTestRun]
        public static void AfterTestSuite()
        {
            SpecFlowReport.Instance.Flush();
            try
            {
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [BeforeFeature()]
        public static void BeforeFeature()
        {
           
        }

        [AfterFeature()]
        public static void AfterFeature()
        {
            
        }

        [BeforeScenario]
        public void CreateScenario(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _testScope = new TestScopeContext();
            _extentTest = SpecFlowReportManager.CreateTest(scenarioContext.ScenarioInfo.Title,
                scenarioContext.ScenarioInfo.Description).AssignCategory(featureContext.FeatureInfo.Title);

            _objectContainer.RegisterInstanceAs<TestScopeContext>(_testScope);
            _objectContainer.RegisterInstanceAs<ExtentTest>(_extentTest);

            _testScope.testLinkNotes = "Executing test case " + scenarioContext.ScenarioInfo.Title + "\n";
        }

        [AfterScenario(Order = 0)]
        public void AfterScenario()
        {
            _extentTest.Log(Status.Info, "The account in test: " + _testScope.accountId);
        }

        [BeforeStep]
        public void BeforeAnyStep(ScenarioContext scenarioContext)
        {
            TestContext.Progress.WriteLine("Running step " + scenarioContext.StepContext.StepInfo.Text);
        }

        [AfterStep]
        public void InsertReportingSteps(ScenarioContext scenarioContext)
        {
            switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case TechTalk.SpecFlow.Bindings.StepDefinitionType.Given:
                    SpecFlowReportManager.GetTest().StepDefinitionGiven(scenarioContext);
                    break;

                case TechTalk.SpecFlow.Bindings.StepDefinitionType.Then:
                    SpecFlowReportManager.GetTest().StepDefinitionThen(scenarioContext);
                    break;

                case TechTalk.SpecFlow.Bindings.StepDefinitionType.When:
                    SpecFlowReportManager.GetTest().StepDefinitionWhen(scenarioContext);
                    break;
            }
        }
    }
}
