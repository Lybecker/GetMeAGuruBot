using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetMeAGuru;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestSearch()
        {
            var client = new SearchClient();

            var result = client.Search(string.Empty, "azure", "Redmond", string.Empty);
        }

        [TestMethod]
        public void WriteToDocDB()
        {
            var client = new Ingestion();
            client.PushDocument();            
        }
    }
}