# BDDSkeleton
A template to develop BDD test using Specflow that integrates the result to TestLink, and generates a report using ExtentReport

## Introduction 

The project implements a Hook to control the flow of the test, from setting up the environment, to teardown the test by logging the result
to TestLink instance server. The framework also generates the report using ExtentReport library.

## Features
+ Using Hook with Specflow step binding.
+ Each step binds to a step in the report.
+ Data driven testing with excel utilities - Map data from excel file to List of Object Models.
+ Soft Assert


## Built With

* [Specflow 2.4](https://www.specflow.org/) - The testing BDD framework
* [ExtentReport 4](https://extentreports.com/docs/versions/4/net/) - 3rd party Report Library
* [TestLinkAPI](https://github.com/freemanke/testlinkapi/ ) - The API to update the result to TestLink Server

## Usage

1. Download the project, extract the folder, and try to build the solution.
2. Adding a new project.
3. Adding a new feature.
4. Define a step definition for the step in feature file.
5. In your project app.config, adding the stepAssembly if you have any separate steps build within.


<specFlow>
    <stepAssemblies>
      <stepAssembly assembly="SpecFlowIntegration" />
    </stepAssemblies>
</specFlow>

6. In order to have the result update on TestLink, you must have:
a. The name of the test must match the testLink prefix case Id, and the name must
match test case 's name.

For example:
```bash
Scenario: CITI-1060 
-> this is the prefix of the test case shows in TestLink

Verify the processing of MT file when field code MASACI is received and indicator H is accompanied
-> name of that exact test case
	
 ```
 
 b. Having a separated hook in your project to define the Test Project, and Test Plan (this is your Hook, not the existing Hook):
 ```bash
 public class Hooks
    {
        private readonly TestScopeContext _testScope;
        private readonly ExtentTest _extentTest;
        private FeatureContext _featureInfo;
        private ScenarioContext _scenarioInfo;

        private const string TestProject = "Citi_US";
        private const string TestPlan = "Automation Test";
        private string _status;

        public Hooks(TestScopeContext testScope, ExtentTest extentTest)
        {
            _testScope = testScope;
            _extentTest = extentTest;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                _featureInfo = FeatureContext.Current;
                _scenarioInfo = ScenarioContext.Current;

                if (_scenarioInfo.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError)
                {
                    throw new Exception("Catch Test Error");
                }
                _testScope.Dispose();

                _status = _scenarioInfo.ScenarioExecutionStatus.ToString();
            }
            catch (Exception e)
            {
                _extentTest.Log(Status.Error, "Exception at: " + e);
                _status = "Failed";
                _testScope.testLinkNotes = _testScope.testLinkNotes + " Test fails at " + e;
            }
            finally
            {
                TestLinkReport.Instance.ProcessResult(
                    _scenarioInfo,
                    TestProject,
                    _featureInfo.FeatureInfo.Title,
                    TestPlan,
                    _status,
                    _testScope.testLinkNotes);
            }
        }
  ```
