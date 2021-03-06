﻿using System;
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

            var result = client.Search("azure");
        }

        [TestMethod, Ignore]
        public void AddSearchData()
        {
            var client = new SearchClient();

            client.Add(new SearchGuru() {
                alias = "stmich",
                techs = new[] { "Web", "Azure" }
            });
        }
    }
}