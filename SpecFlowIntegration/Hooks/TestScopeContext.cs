using System;
using FileProcessingTest.Framework.Contracts.Configurations;
using FileProcessingTest.Framework.Help;

using NUnit.Framework;

namespace SpecFlowIntegration.Hooks
{
    public class TestScopeContext : IDisposable
    {
        
        public SoftAssert softAssert;

        public string testLinkNotes;
        public string clientAccount;
        public int accountId;
        public long fileReceivedId;
        public string fileReceived;
        public string fileSent;
        public IConfigurationContext clientConfigurations;

        public TestScopeContext()
        {
            softAssert = new SoftAssert();
        }

        public void Dispose()
        {
            try
            {
                softAssert.Dispose();
            }
            catch (AssertionException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
