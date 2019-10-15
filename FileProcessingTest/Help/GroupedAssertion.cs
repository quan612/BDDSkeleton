using System;
using System.Diagnostics;

namespace FileProcessingTest.Framework.Help
{
    public class GroupedAssertion
    {
        public String Message { get; private set; }

        public StackTrace StackTrace { get; private set; }

        public GroupedAssertion(String message, StackTrace stackTrace)
        {
            this.Message = message;
            this.StackTrace = stackTrace;
        }
    }
}
