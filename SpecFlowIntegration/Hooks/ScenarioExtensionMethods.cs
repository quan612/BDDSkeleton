
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using MongoDB.Bson;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;

namespace SpecFlowIntegration.Hooks
{
    public static class ScenarioExtensionMethod
    {
        private static void CreateScenario(ExtentTest extent, StepDefinitionType stepDefinitionType, ScenarioContext scenarioContext)
        {
            if (scenarioContext.TestError == null)
            {
                extent.Log(Status.Info, scenarioContext.StepContext.StepInfo.Text);
                if (scenarioContext.StepContext.StepInfo.Table != null)
                {
                    var data = new string[scenarioContext.StepContext.StepInfo.Table.Rows.Count + 1][];
                    var j = 1;

                    data[0] = new[] { scenarioContext.StepContext.StepInfo.Table.Header.ToJson() };

                    foreach (var t in scenarioContext.StepContext.StepInfo.Table.Rows)
                    {
                        data[j] = new[] { t.Values.ToJson() };
                        j++;
                    }

                    var m = MarkupHelper.CreateTable(data);
                    extent.Log(Status.Info, m);
                }
            }
            else
            {
                extent.Log(Status.Fail, scenarioContext.StepContext.StepInfo.Text);
            }
        }

        public static void StepDefinitionGiven(this ExtentTest extent, ScenarioContext scenarioContext)
        {
            CreateScenario(extent, StepDefinitionType.Given, scenarioContext);
        }

        public static void StepDefinitionWhen(this ExtentTest extent, ScenarioContext scenarioContext)
        {
            CreateScenario(extent, StepDefinitionType.Given, scenarioContext);
        }

        public static void StepDefinitionThen(this ExtentTest extent, ScenarioContext scenarioContext)
        {
            CreateScenario(extent, StepDefinitionType.Given, scenarioContext);
        }
    }
}