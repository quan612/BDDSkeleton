using System;
using System.Runtime.CompilerServices;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;

namespace SpecFlowIntegration.ReportUtility
{
    public class SpecFlowReportManager
    {
        [ThreadStatic]
        private static ExtentTest _childTest;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateTest(string testName, string description)
        {
            _childTest = SpecFlowReport.Instance.CreateTest(testName, description);
            return _childTest;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest GetTest()
        {
            return _childTest;
        }
    }
}

//public class SpecFlowReportManager
//{
//    [ThreadStatic]
//    private static ExtentTest _parentTest;

//    [ThreadStatic]
//    private static ExtentTest _childTest;

//    [MethodImpl(MethodImplOptions.Synchronized)]
//    public static ExtentTest CreateTest(string testName, string description)
//    {
//        _parentTest = SpecFlowReport.Instance.CreateTest<Scenario>(testName, description);
//        return _parentTest;
//    }

//    [MethodImpl(MethodImplOptions.Synchronized)]
//    public static ExtentTest CreateNode(string description)
//    {
//        _childTest = _parentTest.CreateNode<Scenario>(description);
//        return _childTest;
//    }

//    [MethodImpl(MethodImplOptions.Synchronized)]
//    public static ExtentTest GetTest()
//    {
//        return _parentTest;
//    }

//    [MethodImpl(MethodImplOptions.Synchronized)]
//    public static ExtentTest GetNode()
//    {
//        return _childTest;
//    }
//}
